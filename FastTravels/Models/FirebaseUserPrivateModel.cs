using System;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace FastTravels.Models
{
    public class FirebaseUserPrivateModel
    {
        [ServerTimestamp(CanReplace = false)]
        public Timestamp CREATED { get; set; }
    }
}
