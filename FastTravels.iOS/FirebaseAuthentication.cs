using System;
using System.Threading.Tasks;
using FastTravels.Common;
using FastTravels.Models;
using Plugin.FirebaseAuth;

namespace FastTravels.iOS
{
    public class FirebaseAuthentication : IAuthenticationService
    {
        public bool IsSignedIn()
        {
            return CrossFirebaseAuth.Current.Instance?.CurrentUser != null;
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult.AuthToken> RefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
