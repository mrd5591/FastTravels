using System;
using System.Linq;
using System.Collections.Generic;
using FastTravel.Models;
using Plugin.CloudFirestore;
using System.Threading.Tasks;
using NodaTime;
using Plugin.FirebasePushNotification;
using Plugin.FirebaseAuth;

namespace FastTravel.Common
{
    public sealed class FirebaseFunctions
    {
        public static async void UpdateTime()
        {
            await CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).SetAsync(new { LAST_ACCESS = FieldValue.ServerTimestamp }, true);
        }

        public static async void UpdateCreated()
        {
            await CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("private").Document("_data").SetAsync(new { CREATED = FieldValue.ServerTimestamp }, true);
        }

        public static async void CreateJourneys(Journey journey)
        {
            if (string.IsNullOrEmpty(Client.FirebaseID))
                return;

            FirebaseJourneyModel journeyModel = new FirebaseJourneyModel { Id = journey.Id, Routes = new Dictionary<string, string>(), OriginName = journey.OriginName, DestinationName = journey.DestinationName, Origin = journey.Origin, Destination = journey.Destination, Time = journey.Time, ActiveDays = journey.ActiveDays, IsActive = journey.IsActive };

            foreach (Route route in journey.Routes)
            {
                journeyModel.Routes.Add(route.summary, route.Name);
            }

            await CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("journeys").Document(journeyModel.Id).SetAsync(journeyModel);
        }

        public static async void SetUnknownRoutes(List<UnknownRoute> unknownRoutes)
        {
            var query = await CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("journeys").GetAsync();

            var docs = query.ToObjects<FirebaseJourneyModel>();

            var batch = CrossCloudFirestore.Current.Instance.Batch();
            foreach(FirebaseJourneyModel model in docs)
            {
                var unknownRs = unknownRoutes.Where(x => x.JourneyId == model.Id).ToList();

                if (unknownRs == null || unknownRs.Count == 0)
                {
                    continue;
                }

                for (int i = unknownRs.Count - 1; i >= 0; i--)
                {
                    var unknownRoute = unknownRs[i];

                    if (!model.Routes.ContainsKey(unknownRoute.Summary))
                    {
                        model.Routes.Add(unknownRoute.Summary, unknownRoute.Name);
                        var journeyRef = CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("journeys").Document(unknownRoute.JourneyId);
                        model.UnknownRoutes = null;
                        batch.Update(journeyRef, model);
                        unknownRoutes.Remove(unknownRoute);
                    }
                }
            }

            await batch.CommitAsync();
            UpdateTime();
        }

        public static async Task<List<Journey>> GetJourneys()
        {
            var doc = await CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("journeys").GetAsync();

            var userModels = doc.ToObjects<FirebaseJourneyModel>();

            List<Journey> journeys = new List<Journey>();
            if(userModels == null)
            {
                return journeys;
            }

            foreach(FirebaseJourneyModel model in userModels)
            {
                journeys.Add(new Journey { Id = model.Id, Name = model.OriginName + " -> " + model.DestinationName, Destination = model.Destination, Origin = model.Origin, IsActive = model.IsActive, ActiveDays = model.ActiveDays, Time = model.Time });
            }

            return journeys;
        }

        public static async Task<List<UnknownRoute>> GetUnknownRoutes()
        {
            var query = await CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("journeys").GetAsync();

            var docs = query.ToObjects<FirebaseJourneyModel>();

            List<UnknownRoute> unknownRoutes = new List<UnknownRoute>();

            foreach (FirebaseJourneyModel journey in docs)
            {
                if (journey.UnknownRoutes == null)
                    continue;

                foreach(string key in journey.UnknownRoutes.Keys)
                {
                    unknownRoutes.Add(new UnknownRoute { JourneyId = journey.Id, JourneyName = journey.OriginName + " -> " + journey.DestinationName, Summary = key, Directions = journey.UnknownRoutes[key] });
                }
            }

            return unknownRoutes;
        }

        public static async void UpdateJourneyDateTime(Journey journey)
        {
            DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            string timezone = tz.ToString();

            var journeyRef = CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("journeys").Document(journey.Id);
            await CrossCloudFirestore.Current.Instance.Batch().Update(journeyRef, "ActiveDays", journey.ActiveDays).Update(journeyRef, "Time", journey.Time).Update(journeyRef, "TimeZone", timezone).Update(journeyRef, "IsActive", true).CommitAsync();
        }

        public static async void UpdateJourneyIsActive(Journey journey)
        {
            await CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("journeys").Document(journey.Id).UpdateAsync(new { IsActive = journey.IsActive });
        }

        public static async void DeleteJourney(Journey deletedJourney)
        {
            var privRef = CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("private").Document("_data").Collection("deletedJourneys").Document(deletedJourney.Id);
            var userRef = CrossCloudFirestore.Current.Instance.Collection("users").Document(Client.FirebaseID).Collection("journeys").Document(deletedJourney.Id);
            await CrossCloudFirestore.Current.Instance.Batch().Set(privRef, deletedJourney).Delete(userRef).CommitAsync();
        }

        public static async void UpdateFCMToken(string token)
        {
            await CrossCloudFirestore.Current.Instance.Collection("users").Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid).SetAsync(new { FCMToken = token }, true);
        }

        public static async void RemoveFCMTOken(string token)
        {
            await CrossCloudFirestore.Current.Instance.Collection("users").Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid).SetAsync(new { FCMToken = "" }, true);
        }
    }
}
