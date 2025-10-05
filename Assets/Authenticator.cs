using System;
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

            void OnAuthenticated(string accountName);
            void OnError(AuthenticationException e);
            void OnError(OperationCanceledException e);
            void OnError(RequestFailedException e);
        }

        public Task Run()
        {
            return Run(GetComponent<CallbackInterface>());
        }

        public async Task Run(CallbackInterface callback)
        {
            var logger = callback?.Logger ?? Debug.unityLogger;
            if (callback == default)
            {
                // 경고 로깅을 이상하게 해야되네 -.-
                logger.Log(LogType.Warning, $"{name}> DON'T HAVE CALLBACK FUNCTION.");
                return;
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

                callback.OnAuthenticated(playerName);
            }
            catch (AuthenticationException e)
            {
                callback.OnError(e);
            }
            catch (OperationCanceledException e)
            {
                callback.OnError(e);
            }
            catch (RequestFailedException e)
            {
                callback.OnError(e);
            }

            bool IsAlive()
            {
                return (this != default) && (callback.CancellationToken.IsCancellationRequested == false);
            }
        }
    }
}
