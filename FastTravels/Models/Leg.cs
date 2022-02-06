using System;
namespace FastTravels.Models
{
    public class Leg
    {
        public Detail distance { get; set; }
        public Detail duration { get; set; }
        public string end_address { get; set; }
        public Location end_location { get; set; }
        public string start_address { get; set; }
        public Location start_location { get; set; }
        public Step[] steps { get; set; }
    }
}
