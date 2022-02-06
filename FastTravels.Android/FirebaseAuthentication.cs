using FastTravels.Common;
using FastTravels.Models;
using Plugin.FirebaseAuth;
using System.Threading.Tasks;

namespace FastTravels.Droid
{
    public class FirebaseAuthentication : IAuthenticationService
    {
        public bool IsSignedIn()
        {
            return CrossFirebaseAuth.Current.Instance?.CurrentUser != null;
        }

        public bool Logout()
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthenticationResult.AuthToken> RefreshToken()
        {
            throw new System.NotImplementedException();
        }
    }
}