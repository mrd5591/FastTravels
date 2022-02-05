using System;
using System.Threading.Tasks;
using FastTravel.Common;
using FastTravel.Models;
using Firebase.Auth;
using Plugin.FirebaseAuth;

namespace FastTravel.iOS
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
