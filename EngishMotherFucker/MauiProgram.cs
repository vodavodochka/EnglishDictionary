using EngishMotherFucker.Shared;
using Microsoft.Extensions.Logging;

namespace EngishMotherFucker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<SqliteConnectionFactory>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
