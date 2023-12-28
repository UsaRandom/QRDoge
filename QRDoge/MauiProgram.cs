using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace QRDoge
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>().UseBarcodeReader()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("ComicNeueu-Bold.ttf", "ComicNeueu");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
