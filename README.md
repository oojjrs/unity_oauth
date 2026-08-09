# unity_oauth

Unity Gaming Services Authentication의 초기화, 익명 로그인, UGS Player ID와 플레이어 이름 조회를 담당하는 Unity 패키지다.

## 설치

프로젝트의 `Packages/manifest.json`에 추가한다.

```json
{
  "dependencies": {
    "com.oojjrs.oauth": "https://github.com/oojjrs/unity_oauth.git?path=/Packages/src"
  }
}
```

## 1.2 마이그레이션

1.2에서 패키지 구조와 SDK 독립적인 예외 계약을 정리하며 유지보수를 재개했습니다.

패키지 루트가 `Assets`에서 `Packages/src`로 이동했습니다. 기존 프로젝트의 manifest에서 `?path=/Assets`를 `?path=/Packages/src`로 변경해야 합니다. 런타임 asmdef와 `Authenticator` 스크립트 GUID는 유지됩니다. `Authenticator.CallbackInterface`의 Unity SDK 예외 매개변수는 아래의 OAuth 소유 예외 타입으로 교체되었습니다.

## 책임

- Unity Gaming Services 초기화
- 로그인되지 않은 경우 익명 로그인
- UGS Player ID와 플레이어 이름 반환
- UGS 예외를 SDK 독립적인 `MyAuthenticationException`, `AuthenticationRequestFailedException`으로 변환
- 취소 오류 전달

Steamworks.NET, EOS SDK, STOVE SDK와 플랫폼별 사용자 타입은 참조하지 않는다.

## 인증 흐름

`Authenticator`는 UGS가 초기화 완료 또는 초기화 중인 상태가 아니면 초기화를 요청한다. 아직 로그인하지 않은 상태면 익명 로그인한 뒤 UGS Player ID와 플레이어 이름을 콜백으로 전달한다.

인증 흐름이 끝나면 `Authenticator`가 붙은 GameObject 전체를 제거한다. 애플리케이션 종료 중에는 제거하지 않는다.

## 오류 계약

소비 게임은 Unity Authentication 또는 Unity Services Core 예외 타입을 직접 참조할 필요가 없다. `Authenticator.CallbackInterface`는 다음 타입만 노출한다.

- `MyAuthenticationException`: 인증 오류 코드와 SDK 독립적인 `MyNotification` 목록
- `AuthenticationRequestFailedException`: UGS 요청 오류 코드
- `OperationCanceledException`: 취소

원본 SDK 예외는 각 래퍼의 `InnerException`으로 보존된다.
