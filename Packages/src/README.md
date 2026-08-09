# OOJJRS' Unity AuthenticationService Helper

Unity Gaming Services Authentication의 초기화, 로그인 전략 실행, UGS Player ID와 플레이어 이름 조회를 담당하는 패키지다.

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

- UGS 초기화와 완료 대기
- 주입된 `AuthenticationSignInInterface` 실행
- 전략이 없을 때 익명 로그인
- UGS Player ID와 플레이어 이름 반환
- Unity SDK 예외를 OAuth 소유 예외로 변환

Steamworks.NET, EOS SDK, STOVE SDK와 플랫폼별 사용자 타입은 참조하지 않는다. 플랫폼 티켓 발급과 UGS 플랫폼 로그인 API 호출은 별도 조립 어댑터가 담당한다.

## 오류 계약

`Authenticator.CallbackInterface`는 아래의 성공·실패 콜백을 모두 구현한다. 일반 플랫폼 오류도 별도 선택 인터페이스 없이 같은 계약의 `AuthenticationFlowException` 콜백으로 전달된다.

- `AuthenticationServiceException`: 인증 오류 코드와 SDK 독립적인 알림 목록
- `AuthenticationRequestFailedException`: UGS 요청 오류 코드
- `OperationCanceledException`: 취소
- `AuthenticationFlowException`: 플랫폼 전략을 포함한 그 밖의 인증 흐름 오류

원본 SDK 예외는 `InnerException`에 보존되며, UGS 알림이 없으면 `Notifications`는 빈 목록이다.

인증이 끝나면 `Authenticator` 컴포넌트만 제거된다. 같은 GameObject의 플랫폼 컴포넌트와 콜백 수신기는 유지된다.
