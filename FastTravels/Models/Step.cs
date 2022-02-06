using FastTravels.Common;
using HtmlAgilityPack;
namespace FastTravels.Models
{
    public class Step
    {
        public Detail distance { get; set; }
        public Detail duration { get; set; }
        public Location end_location { get; set; }
        private string _html_instructions;
        public string html_instructions
        {
            get { return HtmlUtilities.ConvertToPlainText(_html_instructions); }

            set { _html_instructions = value; }
        }
        public Location start_location { get; set; }
        public string travel_mode { get; set; }

        public string GetRawHtml()
        {
            return _html_instructions;
        }
    }
}
