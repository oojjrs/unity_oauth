# OOJJRS' Unity AuthenticationService Helper

`Authenticator`는 UGS 초기화와 공통 인증 흐름을 실행하고 로그인 구현은 `SignInAsync()`에 위임한다. `AnonymousAuthenticator`는 기본 익명 로그인 구현이며, 인증 후 UGS Player ID와 플레이어 이름을 콜백으로 전달한다.

인증 흐름이 끝나면 `Authenticator` 파생 컴포넌트가 붙은 GameObject 전체를 제거한다. GameObject 파괴로 취소된 흐름은 오류 콜백을 호출하지 않는다. 애플리케이션 종료 중에는 제거하지 않는다.

콜백 컴포넌트는 선택 사항이다. `Authenticator.CallbackInterface`에는 Unity SDK 예외를 직접 노출하지 않는다. 인증 오류는 `MyAuthenticationException`, 요청 오류는 `MyRequestFailedException`, GameObject 파괴 취소가 아닌 작업 취소는 `OperationCanceledException`으로 전달하며 원본 SDK 예외는 `InnerException`에 보존한다.
