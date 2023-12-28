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


								if (responseString.StartsWith("ERROR"))
								{
									await DisplayAlert("Error Registering with Node", "Select 'Cancel' in DogecoinTerminal.Error:\n" + responseString, "Ok, I selected Cancel");
									await Navigation.PopAsync();
								}
								else
								{
									try
									{
										var response = JsonNode.Parse(responseString);

										if (response["error"] == null)
										{
											await DisplayAlert("Node is Watching for UTXOs", "Press 'Next' in DogecoinTerminal", "OK, I selected 'Next'");
											await Navigation.PopAsync();
										}
									}
									catch (Exception ex)
									{

										await DisplayAlert("Error Registering with Node", "Select 'Cancel' in DogecoinTerminal.Error:\n" + responseString, "Ok, I selected Cancel");
										await Navigation.PopAsync();
									}
								}


							});

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
										await DisplayAlert("No Balance Info", "No Balance Info, sorry. :(", "OK");
										await Navigation.PopAsync();
									}
									else
									{
										await DisplayAlert("We have Balance Info", "Press 'Next' on DogecoinTerminal, then 'OK' on this message, and then you should get the picture.", "OK");
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

								if(responseString.StartsWith("ERROR"))
								{
									await DisplayAlert("Error Sending Transaction", "Select 'Cancel' in DogecoinTerminal.Error:\n" + responseString, "Ok, I selected Cancel");
									await Navigation.PopAsync();
								}
								else
								{
									try
									{
										var response = JsonNode.Parse(responseString);

										if (response["error"] == null)
										{
											await DisplayAlert("Transaction Broadcasted", "Press 'Next' in DogecoinTerminal", "OK, I selected 'Next'");
											await Navigation.PopAsync();
										}
									}
									catch(Exception ex)
									{

										await DisplayAlert("Error Sending Transaction", "Select 'Cancel' in DogecoinTerminal.Error:\n" + responseString, "Ok, I selected Cancel");
										await Navigation.PopAsync();
									}
								}
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