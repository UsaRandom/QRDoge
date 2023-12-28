using System.Text;
using System.Text.Json.Nodes;
using ZXing.Net.Maui;

namespace QRDoge;

public partial class ScanQRPage : ContentPage
{
	bool ScanFound = false;

	public ScanQRPage()
	{
		InitializeComponent();
		cameraBarcodeReaderView.Options = new BarcodeReaderOptions
		{
			Formats = BarcodeFormats.TwoDimensional,
			AutoRotate = true,
			Multiple = false
		};
	}

	protected async void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
		if (ScanFound)
		{
			return;
		}

		foreach (var barcode in e.Results)
		{
			if (barcode.Value.StartsWith("qrdoge:"))
			{
				ScanFound = true;
				var parts = barcode.Value.Split(':');

				var url = Preferences.Get("RpcUrl", string.Empty);
				var user = Preferences.Get("RpcUsername", string.Empty);
				var pass = Preferences.Get("RpcPassword", string.Empty);


				switch (parts[1])
				{
					case "0-new":
						{
							
							this.Dispatcher.Dispatch(async() =>
							{
								var client = new DogecoinRpcClient(url, user, pass);
								var responseString = client.AddAddressToWatch(parts[2], "qrdoge");
								await DisplayAlert("Node Response", responseString, "OK");
								await Navigation.PopAsync();
							});

							//var client = new DogecoinRpcClient(url, user, pass);
							//var responseString = client.AddAddressToWatch(parts[2], "qrdoge");

							//// Show a popup with the responseString
							//await DisplayAlert("Response", responseString, "OK");
							break;
						}
					case "0-update":
						{


							this.Dispatcher.Dispatch(async () =>
							{
								try
								{

									var client = new DogecoinRpcClient(url, user, pass);
									var responseString = client.ListUnspent(parts[2]);

									// Parse the JSON response
									var unspentJson = JsonObject.Parse(responseString);

									// Extract relevant information from the JSON response
									var utxoJsonArray = unspentJson["result"].AsArray();

									var utxoString = new StringBuilder();
									foreach (var utxoJson in utxoJsonArray)
									{
										utxoString.Append($"{utxoJson["txid"].ToString()}|");
										utxoString.Append($"{utxoJson["vout"].ToString()}|");
										utxoString.AppendLine($"{utxoJson["amount"].ToString()}");
									}

									var resultString = utxoString.ToString();
									

									if(resultString == string.Empty)
									{
										await DisplayAlert("No UTXOs", "No UTXOs to provide.", "OK");
										await Navigation.PopAsync();
									}
									else
									{
										Navigation.InsertPageBefore(new DisplayQRPage(resultString), this);
										await Navigation.PopAsync();
									}
								}
								catch (Exception ex)
								{
									await DisplayAlert("Error", ex.StackTrace + " - " + ex.ToString() + " - " + ex.Message, "OK");
									await Navigation.PopAsync();
								}


							});

							break;
						}
					case "0-send":
						{
							this.Dispatcher.Dispatch(async () =>
							{
								var client = new DogecoinRpcClient(url, user, pass);
								var responseString = client.BroadcastRawTransaction(parts[2]);
								await DisplayAlert("Send Transaction Response", responseString, "OK");
								await Navigation.PopAsync();
							});
							break;
						}

					default:
						await Navigation.PopAsync();
						break;
				}
			}
		}
	}
}