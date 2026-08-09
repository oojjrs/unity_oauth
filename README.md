# unity_oauth

Unity Gaming Services Authentication의 초기화, 로그인 전략 실행, UGS Player ID와 플레이어 이름 조회를 담당하는 Unity 패키지다.

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

1.2에서 플랫폼 로그인 전략 분리를 도입하며 패키지 유지보수를 재개했습니다.

패키지 루트가 `Assets`에서 `Packages/src`로 이동했습니다. 기존 프로젝트의 manifest에서 `?path=/Assets`를 `?path=/Packages/src`로 변경해야 합니다. 런타임 asmdef와 `Authenticator` 스크립트 GUID는 유지됩니다. `Authenticator.CallbackInterface`의 Unity SDK 예외 매개변수는 아래의 OAuth 소유 예외 타입으로 교체되었습니다.

## 책임

- Unity Gaming Services 초기화
- 주입된 로그인 전략 실행
- 기본 익명 로그인
- UGS Player ID와 플레이어 이름 반환
- UGS 예외를 SDK 독립적인 `AuthenticationServiceException`, `AuthenticationRequestFailedException`으로 변환
- 취소 오류 전달
- 로그인 전략에서 발생한 일반 오류를 SDK 독립적인 `AuthenticationFlowException`으로 감싸 주 콜백 계약에 전달

Steamworks.NET, EOS SDK, STOVE SDK와 플랫폼별 사용자 타입은 참조하지 않는다.

## 로그인 전략

`Authenticator`와 같은 GameObject에 `AuthenticationSignInInterface`를 구현한 컴포넌트가 있으면 해당 전략을 실행한다. 구현이 없으면 `AnonymousAuthenticationSignIn`으로 익명 로그인한다.

플랫폼 티켓 발급과 UGS의 플랫폼별 로그인 API 매핑은 별도 조립 어댑터가 담당한다. 이 패키지의 코어는 특정 플랫폼을 알지 않는다.

인증이 끝나면 `Authenticator` 컴포넌트만 제거된다. 같은 GameObject에 배치한 플랫폼 컴포넌트와 콜백 수신기는 계속 유지된다.

## 오류 계약

소비 게임은 Unity Authentication 또는 Unity Services Core 예외 타입을 직접 참조할 필요가 없다. `Authenticator.CallbackInterface`는 다음 타입만 노출한다.

- `AuthenticationServiceException`: 인증 오류 코드와 SDK 독립적인 알림 목록
- `AuthenticationRequestFailedException`: UGS 요청 오류 코드
- `OperationCanceledException`: 취소
- `AuthenticationFlowException`: 플랫폼 로그인 전략을 포함한 그 밖의 인증 흐름 오류

원본 SDK 예외는 각 래퍼의 `InnerException`으로 보존된다. UGS가 알림 목록을 제공하지 않으면 `Notifications`는 빈 목록이다.
