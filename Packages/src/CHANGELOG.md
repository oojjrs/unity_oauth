# Changelog

## 1.2.0

- 플랫폼 로그인 전략 분리를 도입하며 패키지 유지보수를 재개했습니다.
- 패키지 루트를 `Assets`에서 `Packages/src`로 이동했습니다. 기존 manifest의 설치 경로를 변경해야 합니다.
- Unity Authentication과 Core 예외를 OAuth 소유 예외 타입으로 변환해 소비 게임의 SDK 직접 결합을 제거했습니다.
- `AuthenticationSignInInterface`로 익명 로그인 외의 플랫폼 로그인을 주입할 수 있게 했습니다.
- 인증 완료 후 GameObject 전체가 아닌 `Authenticator` 컴포넌트만 제거하도록 수명 주기를 변경했습니다.
- Unity Authentication `3.6.1`과 Services Core `1.16.0`을 직접 의존성으로 사용합니다.
