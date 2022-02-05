using System.Collections.Generic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace FastTravel.Models
{
    public class FirebaseUserModel
    {
        [ServerTimestamp]
        public Timestamp LAST_ACCESS { get; set; }
        public Dictionary<string,FirebaseJourneyModel> journeys { get; set; }
    }
}
