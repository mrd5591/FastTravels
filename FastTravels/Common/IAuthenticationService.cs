using System;
using System.Threading.Tasks;
using static FastTravels.Models.AuthenticationResult;

namespace FastTravels.Common
{
    public interface IAuthenticationService
    {
        bool IsSignedIn();
        bool Logout();
        //Task<AuthResult> LoginAnonymous();
        Task<AuthToken> RefreshToken();
    }
}
