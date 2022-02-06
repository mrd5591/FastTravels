using FastTravels.Common;
using FastTravels.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace FastTravels
{
	public partial class HelpPopup : Rg.Plugins.Popup.Pages.PopupPage
	{
		public HelpPopup (string helpText)
		{
			InitializeComponent();

			Stack.Children.Add(new Label 
			{ 
				Text = helpText,
			});

			BindingContext = this;
		}
	}
}