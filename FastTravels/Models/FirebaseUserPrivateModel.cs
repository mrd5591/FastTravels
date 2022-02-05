using System;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace FastTravel.Models
{
    public class FirebaseUserPrivateModel
    {
        [ServerTimestamp(CanReplace = false)]
        public Timestamp CREATED { get; set; }
    }
}
