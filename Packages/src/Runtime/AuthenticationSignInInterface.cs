using System.Threading;
using System.Threading.Tasks;

namespace oojjrs.oauth
{
    public interface AuthenticationSignInInterface
    {
        Task SignInAsync(CancellationToken cancellationToken);
    }
}
