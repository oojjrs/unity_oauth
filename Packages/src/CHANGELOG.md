# Changelog

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
