using System;
namespace FastTravel.Models
{
    public class GetAllRoutes : JsonPackage
    {
        public string start { get; set; }
        public string destination { get; set; }
    }
}
