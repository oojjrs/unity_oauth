# OOJJRS' Unity AuthenticationService Helper

`Authenticator`는 UGS 초기화와 인증 흐름을 실행한다. 아직 로그인하지 않은 상태면 익명 로그인한 뒤 UGS Player ID와 플레이어 이름을 콜백으로 전달한다.

인증 흐름이 끝나면 `Authenticator`가 붙은 GameObject 전체를 제거한다. 애플리케이션 종료 중에는 제거하지 않는다.

`Authenticator.CallbackInterface`에는 Unity SDK 예외를 직접 노출하지 않는다. 인증 오류는 `MyAuthenticationException`, 요청 오류는 `AuthenticationRequestFailedException`, 취소는 `OperationCanceledException`으로 전달하며 원본 SDK 예외는 `InnerException`에 보존한다.
