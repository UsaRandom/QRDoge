using System;

namespace QRDoge
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();
		}


		private async void OnScanButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ScanQRPage());
		}

		private async void OnNodeSettingsButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new NodeSettingsPage());

		}
	}

}
