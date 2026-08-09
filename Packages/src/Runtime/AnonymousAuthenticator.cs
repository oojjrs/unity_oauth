using System.Threading.Tasks;
using Unity.Services.Authentication;

namespace oojjrs.oauth
{
    public sealed class AnonymousAuthenticator : Authenticator
    {
        protected override async Task SignInAsync()
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}
