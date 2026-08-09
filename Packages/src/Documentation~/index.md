# OOJJRS' Unity AuthenticationService Helper

`Authenticator`는 UGS 초기화와 인증 흐름을 실행한다. 같은 GameObject의 `AuthenticationSignInInterface` 구현을 사용하며, 구현이 없으면 익명 로그인을 수행한다.

플랫폼 SDK에서 인증 증명을 발급받는 책임과 Steam·Epic·STOVE별 UGS API 매핑은 이 코어 패키지 밖에 둔다.

`Authenticator.CallbackInterface`에는 Unity SDK 예외를 직접 노출하지 않는다. 인증 오류는 `AuthenticationServiceException`, 요청 오류는 `AuthenticationRequestFailedException`, 플랫폼 전략의 일반 오류는 `AuthenticationFlowException`으로 전달하며 원본 예외는 `InnerException`에 보존한다.
