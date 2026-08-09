using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace oojjrs.oauth
{
    public abstract class Authenticator : MonoBehaviour
    {
        public interface CallbackInterface
        {
            ILogger Logger { get; }

            void OnAuthenticated(string account, string nickname);
            void OnError(MyAuthenticationException e);
            void OnError(OperationCanceledException e);
            void OnError(MyRequestFailedException e);
        }

        private bool _isQuitting = false;

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private async void Start()
        {
            var cancellationToken = destroyCancellationToken;

            try
            {
                await RunAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (_isQuitting == false)
                Destroy(gameObject);
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var callback = GetComponent<CallbackInterface>();
            await RunAsync(callback, callback as UnityEngine.Object, cancellationToken);
        }

        private async Task RunAsync(CallbackInterface callback, UnityEngine.Object callbackObject, CancellationToken cancellationToken)
        {
            var logger = (callbackObject != null) ? (callback.Logger ?? Debug.unityLogger) : Debug.unityLogger;
            if (callbackObject == null)
            {
                // 경고 로깅을 이상하게 해야되네 -.-
                logger.Log(LogType.Warning, $"{name}> DON'T HAVE CALLBACK FUNCTION.");
            }

            try
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                {
                    await UnityServices.InitializeAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (AuthenticationService.Instance.IsSignedIn == false)
                {
                    await SignInAsync();
                    cancellationToken.ThrowIfCancellationRequested();

                    logger.Log($"{name}> SIGN IN SUCCEEDED.");
                }

                var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                cancellationToken.ThrowIfCancellationRequested();

                if (callbackObject != null)
                    callback.OnAuthenticated(AuthenticationService.Instance.PlayerId, playerName);
            }
            catch (AuthenticationException e)
            {
                if (callbackObject != null)
                    callback.OnError(new MyAuthenticationException(e.ErrorCode, e.Message, e, e.Notifications));
            }
            catch (OperationCanceledException e) when (cancellationToken.IsCancellationRequested == false)
            {
                if (callbackObject != null)
                    callback.OnError(e);
            }
            catch (RequestFailedException e)
            {
                if (callbackObject != null)
                    callback.OnError(new MyRequestFailedException(e.ErrorCode, e.Message, e));
            }
        }

        protected abstract Task SignInAsync();
    }
}
