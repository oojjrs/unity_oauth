# Changelog

## 1.3.0

- `Authenticator`를 플랫폼 로그인용 추상 기반 클래스로 변경하고 `SignInAsync()` 재정의 지점을 추가했습니다.
- 기본 익명 로그인 컴포넌트 `AnonymousAuthenticator`를 추가하고 기존 `Authenticator` 스크립트 GUID를 이전해 기존 scene/prefab 참조가 기본 익명 구현을 계속 가리키도록 했습니다.
- `AuthenticationSignInInterface`, `AnonymousAuthenticationSignIn`, `AuthenticationNotification`을 제거했습니다.
- `AuthenticationRequestFailedException`을 `MyRequestFailedException`으로 변경하고 `Authenticator.CallbackInterface` 계약을 갱신했습니다.
- GameObject 파괴 토큰을 비동기 흐름 전에 보존하고 파괴로 인한 취소는 오류 콜백 없이 종료하도록 변경했습니다.
- SDK 알림이 없으면 `MyAuthenticationException.Notifications`가 빈 목록을 반환하도록 변경했습니다.

## 1.2.2

- `Authenticator.CallbackInterface`의 `CancellationToken` 요구를 제거하고 `Authenticator`의 파괴 취소 토큰으로 인증 흐름의 생존 여부를 판단하도록 변경했습니다.

## 1.2.1

- `Authenticator`의 인증과 수명 주기 흐름을 UnityOnet에서 분리하기 직전의 `MyNetAuthenticator` 동작과 맞췄습니다.
- 주입된 로그인 전략 대신 로그인되지 않은 상태에서 익명 로그인을 직접 실행하도록 복원했습니다.
- 인증 흐름이 끝나면 `Authenticator` 컴포넌트만이 아닌 GameObject 전체를 제거하도록 복원했습니다.
- `AuthenticationServiceException`을 `MyAuthenticationException`으로 교체하고 SDK 독립적인 알림을 중첩 `MyNotification`으로 제공하도록 변경했습니다.
- `AuthenticationFlowException`을 제거하고 `AuthenticationRequestFailedException`이 `Exception`을 직접 상속하도록 복원했습니다.

## 1.2.0

- 플랫폼 로그인 전략 분리를 도입하며 패키지 유지보수를 재개했습니다.
- 패키지 루트를 `Assets`에서 `Packages/src`로 이동했습니다. 기존 manifest의 설치 경로를 변경해야 합니다.
- Unity Authentication과 Core 예외를 OAuth 소유 예외 타입으로 변환해 소비 게임의 SDK 직접 결합을 제거했습니다.
- `AuthenticationSignInInterface`로 익명 로그인 외의 플랫폼 로그인을 주입할 수 있게 했습니다.
- 인증 완료 후 GameObject 전체가 아닌 `Authenticator` 컴포넌트만 제거하도록 수명 주기를 변경했습니다.
- Unity Authentication `3.6.1`과 Services Core `1.16.0`을 직접 의존성으로 사용합니다.
