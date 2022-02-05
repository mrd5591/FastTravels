using System.Linq;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Text;
using FastTravel.Models;
using Flurl;
using Plugin.FirebaseAuth;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Plugin.CloudFirestore;
using FastTravel.Common;

namespace FastTravel
{
    public class Client
    {
        private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://us-central1-enc-fast-travel.cloudfunctions.net") };
        public static readonly Uri BaseUri = new Uri("https://us-central1-enc-fast-travel.cloudfunctions.net");
        public static readonly string getPlaceEndpoint = "getPlace";
        public static readonly string getAllRoutesEndpoint = "getAllRoutes";

        public static IFirestore db;

        public static string FirebaseID;

        public static async Task<string> Post(Uri url, JsonPackage json)
        {
            var jsonObject = JsonConvert.SerializeObject(json);
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

            string result = await client.PutAsync(url, content).Result.Content.ReadAsStringAsync();
            return result;
        }

        public static async Task<string> Get(Uri url, params (string, string)[] httpParams)
        {
            url = url.SetQueryParams(httpParams).ToUri();
            string jsonResult = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
            return jsonResult;
        }

        public static void SetHttpAuthToken(string authToken)
        {
            var authHeader = new AuthenticationHeaderValue("Bearer", authToken);
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.DefaultRequestHeaders.Add("X-Accel-Buffering", "no");
        }

        public async static void SignInAnonymously()
        {
            var user = await CrossFirebaseAuth.Current.Instance.SignInAnonymouslyAsync();
            FirebaseID = user.User.Uid;
            SetHttpAuthToken(await user.User.GetIdTokenAsync(false));
            FirebaseFunctions.UpdateCreated();
            FirebaseFunctions.UpdateTime();
        }

        public async static void Login()
        {
            var user = CrossFirebaseAuth.Current.Instance.CurrentUser;
            FirebaseID = user.Uid;
            SetHttpAuthToken(await user.GetIdTokenAsync(false));
            FirebaseFunctions.UpdateTime();
        }
    }
}
