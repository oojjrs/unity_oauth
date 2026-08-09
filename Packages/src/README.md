# OOJJRS' Unity AuthenticationService Helper

Unity Gaming Services Authentication의 초기화, 로그인, UGS Player ID와 플레이어 이름 조회를 담당하는 패키지다.

## 설치

소비 프로젝트의 `Packages/manifest.json`에 추가한다.

```json
{
  "dependencies": {
    "com.oojjrs.oauth": "https://github.com/oojjrs/unity_oauth.git?path=/Packages/src"
  }
}
```

1.2부터 패키지 경로가 `?path=/Assets`에서 `?path=/Packages/src`로 바뀌었다. 1.3에서는 `Authenticator`가 추상 기반 클래스가 되고 기본 익명 구현은 `AnonymousAuthenticator`로 분리되었다. 기존 `Authenticator` 스크립트 GUID는 `AnonymousAuthenticator`가 이어받아 기존 scene/prefab 참조를 유지한다.

## 책임 경계

- UGS 초기화 요청
- 로그인되지 않은 경우 구성된 로그인 구현 실행
- UGS Player ID와 플레이어 이름 반환
- Unity SDK 예외를 OAuth 소유 예외로 변환

Steamworks.NET, EOS SDK, STOVE SDK와 플랫폼별 사용자 타입은 참조하지 않는다.

## 오류 계약

콜백 컴포넌트는 선택 사항이다. 사용하는 경우 `Authenticator.CallbackInterface`의 `Logger`, `OnAuthenticated`와 아래 오류 콜백을 모두 구현한다.

- `MyAuthenticationException`: 인증 오류 코드와 SDK 독립적인 `MyNotification` 목록
- `MyRequestFailedException`: UGS 요청 오류 코드
- `OperationCanceledException`: GameObject 파괴 취소가 아닌 작업 취소

원본 SDK 예외는 `InnerException`에 보존된다.

기본 익명 로그인에는 `AnonymousAuthenticator`를 사용한다. 다른 플랫폼 로그인은 `Authenticator`를 상속하고 `SignInAsync()`를 재정의한다.

인증 흐름이 끝나면 `Authenticator` 파생 컴포넌트가 붙은 GameObject 전체를 제거한다. GameObject 파괴로 취소된 흐름은 오류 콜백을 호출하지 않는다. 애플리케이션 종료 중에는 제거하지 않는다.
