using System;
using System.Linq;
using System.Threading;
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
            CancellationToken CancellationToken { get; }
            ILogger Logger { get; }

            void OnAuthenticated(string account, string nickname);
            void OnError(AuthenticationFlowException e);
            void OnError(AuthenticationRequestFailedException e);
            void OnError(AuthenticationServiceException e);
            void OnError(OperationCanceledException e);
        }

        private bool _isQuitting;

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private async void Start()
        {
            await RunAsync();

            if ((this != null) && (_isQuitting == false))
                Destroy(this);
        }

        private Task RunAsync()
        {
            var callback = GetComponent<CallbackInterface>();
            return RunAsync(callback, callback as UnityEngine.Object);
        }

        private async Task RunAsync(CallbackInterface callback, UnityEngine.Object callbackObject)
        {
            var logger = callbackObject != null ? callback.Logger ?? Debug.unityLogger : Debug.unityLogger;
            if (callbackObject == null)
            {
                // 경고 로깅을 이상하게 해야되네 -.-
                logger.Log(LogType.Warning, $"{name}> DON'T HAVE CALLBACK FUNCTION.");
                return;
            }

            try
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                    await UnityServices.InitializeAsync();

                if (IsAlive() == false)
                    return;

                var signIn = GetComponent<AuthenticationSignInInterface>();
                if ((signIn as UnityEngine.Object) == null)
                    signIn = new AnonymousAuthenticationSignIn();

                await signIn.SignInAsync(callback.CancellationToken);
                if (IsAlive() == false)
                    return;

                var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                if (IsAlive() == false)
                    return;

                callback.OnAuthenticated(AuthenticationService.Instance.PlayerId, playerName);
            }
            catch (AuthenticationException e)
            {
                var error = new AuthenticationServiceException(e.ErrorCode, e.Message, e,
                    e.Notifications?.Select(notification => new AuthenticationNotification(notification.CaseId,
                        notification.CreatedAt, notification.Id, notification.Message, notification.PlayerId,
                        notification.ProjectId, notification.Type)).ToArray() ?? Array.Empty<AuthenticationNotification>());
                if (IsCallbackAlive())
                    callback.OnError(error);
                else
                    logger.LogException(error);
            }
            catch (OperationCanceledException e)
            {
                if (IsCallbackAlive())
                    callback.OnError(e);
            }
            catch (RequestFailedException e)
            {
                var error = new AuthenticationRequestFailedException(e.ErrorCode, e.Message, e);
                if (IsCallbackAlive())
                    callback.OnError(error);
                else
                    logger.LogException(error);
            }
            catch (Exception e)
            {
                var error = e as AuthenticationFlowException ?? new AuthenticationFlowException(e.Message, e);
                if (IsCallbackAlive())
                    callback.OnError(error);
                else
                    logger.LogException(error);
            }

            bool IsAlive()
            {
                return IsCallbackAlive() && (callback.CancellationToken.IsCancellationRequested == false);
            }

            bool IsCallbackAlive()
            {
                return (this != null) && (callbackObject != null);
            }
        }
    }
}
