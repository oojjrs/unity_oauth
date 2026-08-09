# OOJJRS' Unity AuthenticationService Helper

Unity Gaming Services Authentication의 초기화, 익명 로그인, UGS Player ID와 플레이어 이름 조회를 담당하는 패키지다.

## 설치

소비 프로젝트의 `Packages/manifest.json`에 추가한다.

```json
{
  "dependencies": {
    "com.oojjrs.oauth": "https://github.com/oojjrs/unity_oauth.git?path=/Packages/src"
  }
}
```

1.2부터 패키지 경로가 `?path=/Assets`에서 `?path=/Packages/src`로 바뀌었다. 런타임 asmdef와 `Authenticator` 스크립트 GUID는 유지된다.

## 책임 경계

- UGS 초기화 요청
- 로그인되지 않은 경우 익명 로그인
- UGS Player ID와 플레이어 이름 반환
- Unity SDK 예외를 OAuth 소유 예외로 변환

Steamworks.NET, EOS SDK, STOVE SDK와 플랫폼별 사용자 타입은 참조하지 않는다.

## 오류 계약

`Authenticator.CallbackInterface`는 아래의 성공·실패 콜백을 모두 구현한다.

- `MyAuthenticationException`: 인증 오류 코드와 SDK 독립적인 `MyNotification` 목록
- `AuthenticationRequestFailedException`: UGS 요청 오류 코드
- `OperationCanceledException`: 취소

원본 SDK 예외는 `InnerException`에 보존된다.

인증 흐름이 끝나면 `Authenticator`가 붙은 GameObject 전체를 제거한다. 애플리케이션 종료 중에는 제거하지 않는다.
