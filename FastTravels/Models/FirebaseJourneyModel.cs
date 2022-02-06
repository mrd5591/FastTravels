using System;
using System.Collections.Generic;
using Plugin.CloudFirestore.Attributes;

namespace FastTravels.Models
{
    public class FirebaseJourneyModel
    {
        [Id]
        public string Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public Dictionary<string,string> Routes { get; set; }
        public Dictionary<string,List<string>> UnknownRoutes { get; set; }
        public List<string> ActiveDays { get; set; }
        public string Time { get; set; }
        public bool IsActive { get; set; }
    }
}
