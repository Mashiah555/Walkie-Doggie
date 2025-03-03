using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using The49.Maui.BottomSheet;

namespace Walkie_Doggie
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBottomSheet()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if ANDROID
            SetAndroidHandlers();
#endif

#if WINDOWS
            SetWindowsHandlers();
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

#if ANDROID
        public static void SetAndroidHandlers()
        {
            // Removes Entry underline on Android:
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                handler.PlatformView.Background = null;
            });
        }
#endif

#if WINDOWS
        public static void SetWindowsHandlers()
        {
            // Removes Switch state-label on Windows:
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("NoSwitchLabel", (handler, view) =>
            {
                handler.PlatformView.OffContent = string.Empty;
                handler.PlatformView.OnContent = string.Empty;

                handler.PlatformView.MinWidth = 0;
            });
        }
#endif

    }
}
