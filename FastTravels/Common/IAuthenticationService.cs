using System;
using System.Threading.Tasks;
using static FastTravel.Models.AuthenticationResult;

namespace FastTravel.Common
{
    public interface IAuthenticationService
    {
        bool IsSignedIn();
        bool Logout();
        //Task<AuthResult> LoginAnonymous();
        Task<AuthToken> RefreshToken();
    }
}
