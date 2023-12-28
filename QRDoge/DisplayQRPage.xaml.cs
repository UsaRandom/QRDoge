namespace QRDoge;

public partial class DisplayQRPage : ContentPage
{
	public DisplayQRPage(string qrText)
	{
		InitializeComponent();
		QRView.Value = qrText;
	}
}