

using FastTravel.Common;
using FastTravel.Models;
using Rg.Plugins.Popup.Services;

namespace FastTravel
{
    public partial class EditJourneyPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public EditJourneyPopup(Journey journey)
        {
            InitializeComponent();

            BindingContext = this;
        }

        void Save_Clicked(System.Object sender, System.EventArgs e)
        {
            ClosePopup();
        }

        void Cancel_Clicked(object sender, System.EventArgs e)
        {
            ClosePopup();
        }

        void ClosePopup()
        {
            PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}
