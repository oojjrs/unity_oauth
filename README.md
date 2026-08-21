# UnityOauth

> [!WARNING]
> **Obsolete:** 이 패키지는 더 이상 유지보수되지 않습니다.

Unity Gaming Services를 초기화하고 인증 결과의 Player ID와 플레이어 이름을 전달하는 런타임 패키지다.

## 설치

Unity 6000.0 이상에서 Package Manager의 `Install package from git URL...`에 다음 주소를 입력한다.

```text
https://github.com/oojjrs/unity_oauth.git?path=/Packages/src
```

## 구성 요소

| 구성 요소 | 종류 | 용도 |
| --- | --- | --- |
| [`Authenticator`](Packages/src/Runtime/Authenticator.cs) | 추상 `MonoBehaviour` | UGS 초기화와 공통 인증 흐름을 실행하고 로그인을 `SignInAsync`에 위임한다. |
| [`AnonymousAuthenticator`](Packages/src/Runtime/AnonymousAuthenticator.cs) | `MonoBehaviour` | UGS 익명 로그인을 수행하는 기본 구현이다. |
| `Authenticator.CallbackInterface` | 인터페이스 | 인증 결과와 SDK 독립적인 오류를 선택적으로 받는다. |
| `MyAuthenticationException`, `MyRequestFailedException` | 예외 | Unity SDK 인증·요청 오류를 패키지 소유 타입으로 전달한다. |

## 사용

기본 익명 인증에는 `AnonymousAuthenticator`를 GameObject에 추가한다. 결과가 필요하면 같은 GameObject에 `Authenticator.CallbackInterface` 구현 컴포넌트를 추가하며, 다른 로그인 방식은 `Authenticator`를 상속해 `SignInAsync`를 구현한다.

## 범위

- 애플리케이션 종료 중이 아니면 인증 흐름이 끝난 뒤 `Authenticator`가 붙은 GameObject가 제거된다.
- Steamworks.NET, EOS SDK, STOVE SDK와 플랫폼별 사용자 모델은 이 패키지의 범위가 아니다.

자세한 동작은 [패키지 문서](Packages/src/Documentation~/index.md), 변경 사항은 [CHANGELOG](Packages/src/CHANGELOG.md)를 참고한다.
