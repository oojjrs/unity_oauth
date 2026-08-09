using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;

namespace oojjrs.oauth
{
    public sealed class AnonymousAuthenticationSignIn : AuthenticationSignInInterface
    {
        async Task AuthenticationSignInInterface.SignInAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (AuthenticationService.Instance.IsSignedIn)
                return;

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
