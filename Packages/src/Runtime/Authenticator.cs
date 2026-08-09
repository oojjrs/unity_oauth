using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace oojjrs.oauth
{
    public class Authenticator : MonoBehaviour
    {
        public interface CallbackInterface
        {
            ILogger Logger { get; }

            void OnAuthenticated(string account, string nickname);
            void OnError(MyAuthenticationException e);
            void OnError(OperationCanceledException e);
            void OnError(AuthenticationRequestFailedException e);
        }

        private bool _isQuitting = false;

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private async void Start()
        {
            await RunAsync();

            if (_isQuitting == false)
                Destroy(gameObject);
        }

        private Task RunAsync()
        {
            var callback = GetComponent<CallbackInterface>();
            return RunAsync(callback, callback as UnityEngine.Object);
        }

        private async Task RunAsync(CallbackInterface callback, UnityEngine.Object callbackObject)
        {
            var logger = (callbackObject != null) ? (callback.Logger ?? Debug.unityLogger) : Debug.unityLogger;
            if (callbackObject == null)
            {
                // 경고 로깅을 이상하게 해야되네 -.-
                logger.Log(LogType.Warning, $"{name}> DON'T HAVE CALLBACK FUNCTION.");
            }

            try
            {
                if (UnityServices.State is not (ServicesInitializationState.Initialized or ServicesInitializationState.Initializing))
                    await UnityServices.InitializeAsync();

                if (IsAlive() == false)
                    return;

                if (AuthenticationService.Instance.IsSignedIn == false)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    if (IsAlive() == false)
                        return;

                    logger.Log($"{name}> Sign in anonymously succeeded!");
                }

                var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                if (IsAlive() == false)
                    return;

                callback?.OnAuthenticated(AuthenticationService.Instance.PlayerId, playerName);
            }
            catch (AuthenticationException e)
            {
                callback?.OnError(new MyAuthenticationException(e.ErrorCode, e.Message, e, e.Notifications));
            }
            catch (OperationCanceledException e)
            {
                callback?.OnError(e);
            }
            catch (RequestFailedException e)
            {
                callback?.OnError(new AuthenticationRequestFailedException(e.ErrorCode, e.Message, e));
            }

            bool IsAlive()
            {
                return (this != null) && (callbackObject != null) && (destroyCancellationToken.IsCancellationRequested == false);
            }
        }
    }
}
