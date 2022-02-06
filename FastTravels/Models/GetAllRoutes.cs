using System;
namespace FastTravels.Models
{
    public class GetAllRoutes : JsonPackage
    {
        public string start { get; set; }
        public string destination { get; set; }
    }
}
