# unity_oauth
Unity Gaming Services(UGS) Authentication을 간단히 붙이기 위한 경량 래퍼입니다.  
`Authenticator` + `AuthenticatorReceiver` 구조로 **UGS 초기화 → 익명 로그인 → 플레이어 이름 조회 → 콜백 통지**까지의 흐름을 처리합니다.

---

## 요구 사항

- Unity 6000.0 LTS 이상 권장
- 의존성 패키지 (자동 해결)
  - `com.unity.services.core`
  - `com.unity.services.authentication`
- UGS Project 연결(프로젝트 ID 설정)

---

## 설치

### 1) Git URL로 UPM 추가

Unity Package Manager → **Add package from git URL…**

```text
https://github.com/oojjrs/unity_oauth.git?path=/Assets
```

### 2) 수동 소스 포함

`Assets/` 하위에 `Authenticator.cs`, `AuthenticatorReceiver.cs`를 직접 추가해도 동작하지만, 권장하지 않습니다.

---

## 빠른 시작 (Quick Start)

1. **Scene에 컴포넌트 배치**

- 빈 GameObject를 만들고 `Authenticator`를 붙입니다.
- 같은 GameObject에 `Authenticator.CallbackInterface`를 구현하는 스크립트를 함께 붙입니다.

2. **콜러에서 실행**

```csharp
// 같은 GameObject에 붙어있는 CallbackInterface 구현체를 찾아 실행
await GetComponent<Authenticator>().RunAsync();
```

---

## 사용 예시

```csharp
using oojjrs.oauth;
using UnityEngine;

[RequireComponent(typeof(Authenticator))]
public class MyAuthReceiver : MonoBehaviour, Authenticator.CallbackInterface
{
    public System.Threading.CancellationToken CancellationToken => null /* 필요 시 구현 */;
    public ILogger Logger => null; // 필요 시 커스텀 로거 주입

    public void OnAuthenticated(string accountName)
    {
        Debug.Log(accountName);
    }

    public void OnError(Unity.Services.Authentication.AuthenticationException e)
    {
        Debug.LogException(e);
    }

    public void OnError(System.OperationCanceledException e)
    {
        Debug.Log($"{name}> CANCELED.");
    }

    public void OnError(Unity.Services.Core.RequestFailedException e)
    {
        Debug.LogException(e);
    }

    private async void Start()
    {
        // Authenticator 컴포넌트 찾아 실행
        await GetComponent<Authenticator>().RunAsync();
    }
}
```

`AuthenticatorReceiver.cs`를 그대로 사용하면 Hub.App에 AccountName을 세팅하고 Debug 로그까지 출력해 줍니다.  
필요에 따라 직접 구현체를 만들어도 됩니다.

---

## 동작 방식

- **UGS 초기화**  
  아직 초기화되지 않았다면 `UnityServices.InitializeAsync()`를 호출합니다.
- **생존성(Alive) 체크**  
  매 단계 후 “컴포넌트가 파괴되지 않았는지”와 “취소 토큰이 취소되지 않았는지”를 확인하여 조기 반환합니다.
- **로그인**  
  로그인 상태가 아니면 **익명 로그인**(`SignInAnonymouslyAsync`)을 시도합니다. (추후 여러 옵션 추가 예정)
- **플레이어 이름 조회**  
  `GetPlayerNameAsync()`로 표시용 플레이어 이름을 받아 콜백으로 반환합니다.
- **오류 처리**  
  `AuthenticationException`, `OperationCanceledException`, `RequestFailedException`을 별도로 포착하여 각각의 `OnError(...)`로 위임합니다.

---

## API

### `class Authenticator : MonoBehaviour`

- `Task RunAsync()`  
  같은 GameObject에서 `CallbackInterface` 구현 컴포넌트를 찾아 실행합니다.
- `Task RunAsync(CallbackInterface callback)`  
  명시적으로 콜백 인스턴스를 전달하여 실행합니다.

### `interface Authenticator.CallbackInterface`

- `CancellationToken CancellationToken { get; }`  
  진행 중 취소 제어. 컴포넌트 파괴 시 `Cancel`되도록 구현하는 것을 권장합니다.
- `ILogger Logger { get; }`  
  Unity 표준 로거. 필요 시 커스텀 래퍼 사용 가능.
- `void OnAuthenticated(string accountName)`  
  성공 시 호출. UGS 플레이어 이름이 전달됩니다.
- `void OnError(AuthenticationException e)`  
- `void OnError(OperationCanceledException e)`  
- `void OnError(RequestFailedException e)`  
  예외 유형별 후처리를 분리할 수 있습니다.

---

## 베스트 프랙티스 & 주의 사항

- **메인 스레드에서 호출**: UGS API는 대부분 메인 스레드 컨텍스트에서의 후속 처리(UI 갱신 등)를 가정합니다.
- **취소 토큰 설계**: `MonoBehaviour` 수명과 연동해 파괴 시 토큰을 취소하세요.
- **콜백 동거 보장**: `RunAsync()`(매개변수 없는 오버로드)는 내부적으로 `GetComponent<CallbackInterface>()`를 사용하므로 **같은 GameObject**에 콜백 구현체를 붙여야 합니다. 없으면 경고만 남기고 종료합니다.
- **프로덕션 전환**: 익명 로그인 대신 플랫폼 계정 연동(Apple/Google/Steam 등)이 필요하면 `AuthenticationService`의 대응 메서드로 교체하고, 성공 후 플레이어 이름 조회/콜백 패턴은 동일하게 유지할 수 있습니다.

---

## 라이선스
없음
