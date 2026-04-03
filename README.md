# unity_oauth

> ⚠️ **DEPRECATED - 더 이상 유지보수되지 않습니다**
>
> 이 저장소는 더 이상 사용되지 않으며 유지보수도 중단되었습니다.  
> 향후 업데이트, 버그 수정, 기능 추가는 이루어지지 않습니다.
>
> ❌ 신규 프로젝트 사용 금지  
>
> 👉 대신 https://github.com/oojjrs/unity_onet 사용을 권장합니다.

---

Unity Gaming Services Authentication을 빠르게 붙이기 위한 경량 유틸리티입니다.

이 패키지는 `Authenticator` 컴포넌트 하나로 다음 흐름을 처리합니다.

1. UGS 초기화  
2. 익명 로그인  
3. 플레이어 이름 조회  
4. 콜백 인터페이스로 결과 전달  
5. 작업 종료 후 자기 자신 제거  

---

## 특징

- MonoBehaviour 기반의 단순한 진입 구조  
- UGS 미초기화 상태라면 자동 초기화  
- 익명 로그인 자동 수행  
- PlayerId + PlayerName 콜백 제공  
- CancellationToken 기반 취소 처리  
- 예외 타입별 에러 콜백 제공  
- 완료 후 GameObject 자동 제거  

---

## 요구 사항

- Unity 6000.0 이상  
- Unity Gaming Services 프로젝트 연동 완료  
- Authentication 패키지 사용 가능  

---

## 설치

### UPM Git URL

    https://github.com/oojjrs/unity_oauth.git?path=/Assets

---

## 패키지 정보

- Package Name: `com.oojjrs.oauth`  
- Assembly Definition: `oojjrs.oauth`  

---

## 빠른 시작

### 1. Authenticator 추가

빈 GameObject 생성 후 Authenticator 추가

### 2. 콜백 구현체 추가

같은 오브젝트에 Authenticator.CallbackInterface 구현 컴포넌트 추가

    using System;
    using System.Threading;
    using Unity.Services.Authentication;
    using Unity.Services.Core;
    using UnityEngine;
    using oojjrs.oauth;

    [RequireComponent(typeof(Authenticator))]
    public class MyAuthReceiver : MonoBehaviour, Authenticator.CallbackInterface
    {
        private CancellationTokenSource _cts;

        public CancellationToken CancellationToken => _cts?.Token ?? default;
        public ILogger Logger => Debug.unityLogger;

        private void Awake()
        {
            _cts = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public void OnAuthenticated(string account, string nickname)
        {
            Debug.Log($"Authenticated - Account: {account}, Nickname: {nickname}");
        }

        public void OnError(AuthenticationException e)
        {
            Debug.LogException(e);
        }

        public void OnError(OperationCanceledException e)
        {
            Debug.Log($"{name}> Authentication canceled.");
        }

        public void OnError(RequestFailedException e)
        {
            Debug.LogException(e);
        }
    }

---

## 동작 방식

Authenticator는 Start()에서 자동 실행됨

1. 콜백 구현체 검색  
2. UGS 초기화 (필요 시)  
3. 익명 로그인  
4. PlayerName 조회  
5. 콜백 호출  
6. GameObject 제거  

---

## 주의 사항

- 콜백 구현체는 반드시 같은 GameObject에 있어야 함  
- 완료 후 GameObject 전체가 제거됨  
- 취소 토큰 구현 권장  
- 현재 익명 로그인만 지원  

---

## Status

This project is deprecated and kept for reference only.
