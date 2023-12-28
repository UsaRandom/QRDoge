namespace QRDoge;

public partial class NodeSettingsPage : ContentPage
{
	public NodeSettingsPage()
	{
		InitializeComponent();

		var url = Preferences.Get("RpcUrl", "http://192.168.1.69:22555/");
		var user = Preferences.Get("RpcUsername", string.Empty);
		var pass = Preferences.Get("RpcPassword", string.Empty);

		RpcUrlEntry.Text = url;
		RpcUsernameEntry.Text = user;
		RpcPasswordEntry.Text = pass;


	}
	private void OnSaveSettingsClicked(object sender, EventArgs e)
	{
		// Retrieve values from Entry controls
		string rpcUrl = RpcUrlEntry.Text;
		string rpcUsername = RpcUsernameEntry.Text;
		string rpcPassword = RpcPasswordEntry.Text;

		// Save values using Preferences or other persistent storage
		Preferences.Set("RpcUrl", rpcUrl);
		Preferences.Set("RpcUsername", rpcUsername);
		Preferences.Set("RpcPassword", rpcPassword);

		// Optionally, provide feedback to the user that settings are saved
		DisplayAlert("Settings Saved", "Node settings have been saved.", "OK");
	}
}