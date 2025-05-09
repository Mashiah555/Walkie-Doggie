using CommunityToolkit.Maui.Alerts;
using System.Text.Json;

namespace Walkie_Doggie.Services;

public static class AppService
{
    #region Version Diagnostics
    public static ReleaseVersion CurrentVersion
    {
        get
        {
            // Convert Major.Minor.Build.Revision to Major.Minor.Build
            return new ReleaseVersion() { Version = AppInfo.VersionString};
        }
    }

    public static async Task CheckForUpdatesAsync()
    {
        try
        {
            await NetworkService.NetworkCheck();

            if (string.IsNullOrEmpty(AppInfo.BuildString))
                throw new Exception();

            using HttpClient client = new();
            string json = await client.GetStringAsync(
                "https://raw.githubusercontent.com/Mashiah555/Walkie-Doggie/master/Services/version.json");
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            VersionInfo? remote = JsonSerializer.Deserialize<VersionInfo>(json, options);
            if (remote == null || string.IsNullOrEmpty(remote.LatestVersion))
                throw new Exception("Failed to deserialize the json's url");

            ReleaseVersion latestVersion = new() { Version = remote.LatestVersion };
            if (latestVersion <= CurrentVersion)
                return;

            bool update = false;
            if (!remote.IsMandatory)
            {
                await Snackbar.Make(
                    "עדכון חדש זמין",
                    () => { update = true; },
                    "עדכן",
                    TimeSpan.FromSeconds(6)).Show();
                await Task.Delay(6000);
                if (!update)
                    return;
            }

        UpdatePopup:
            update = await Shell.Current.DisplayAlert("עדכון זמין", remote.Message, "עדכון", "מה חדש");
            await Launcher.OpenAsync(update ? remote.DownloadUrl : remote.AboutUrl);

            if (remote.IsMandatory && !update)
                goto UpdatePopup;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("שגיאה", ex.Message, "סגור");
        }
    }

    public class ReleaseVersion
    {
        private string _version;
        public required string Version
        {
            get => _version;
            set
            {
                _version = value;
                ParseVersion(value);
            }
        }
        public int Major { get; private set; } = 0;
        public int Minor { get; private set; } = 0;
        public int Build { get; private set; } = 0;

        private void ParseVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                Major = 0;
                Minor = 0;
                Build = 0;
                return;
            }

            string[] parts = version.Split('.');

            // Extract available parts
            if (parts.Length > 0 && int.TryParse(parts[0], out int major))
                Major = major;

            if (parts.Length > 1 && int.TryParse(parts[1], out int minor))
                Minor = minor;

            if (parts.Length > 2 && int.TryParse(parts[2], out int build))
                Build = build;
        }

        #region ReleaseVersion Comparison Operators
        public static bool operator ==(ReleaseVersion left, ReleaseVersion right)
        {
            if (left is null)
                return right is null;

            if (right is null)
                return false;

            return left.Major == right.Major &&
                   left.Minor == right.Minor &&
                   left.Build == right.Build;
        }

        public static bool operator !=(ReleaseVersion left, ReleaseVersion right)
        {
            return !(left == right);
        }

        public static bool operator <(ReleaseVersion left, ReleaseVersion right)
        {
            if (left is null)
                return !(right is null);

            if (right is null)
                return false;

            if (left.Major != right.Major)
                return left.Major < right.Major;

            if (left.Minor != right.Minor)
                return left.Minor < right.Minor;

            return left.Build < right.Build;
        }

        public static bool operator >(ReleaseVersion left, ReleaseVersion right)
        {
            return right < left;
        }

        public static bool operator <=(ReleaseVersion left, ReleaseVersion right)
        {
            return left < right || left == right;
        }

        public static bool operator >=(ReleaseVersion left, ReleaseVersion right)
        {
            return left > right || left == right;
        }
        #endregion ReleaseVersion Comparison Operators

        #region ReleaseVersion Implicit Operators
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ReleaseVersion other = (ReleaseVersion)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            return Major.GetHashCode() ^ Minor.GetHashCode() ^ Build.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}";
        }
        #endregion ReleaseVersion Implicit Operators
    }
    private class VersionInfo
    {
        /* version.json Structural Properties.
           Must match the JSON properties to deserialize correctly. */
        public string LatestVersion { get; set; } = string.Empty;
        public bool IsMandatory { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public string AboutUrl { get; set; } = string.Empty;
    }
    #endregion Version Diagnostics

    #region App Configuration
    public static void QuitApp()
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#else
       // For unsupported platforms:
       Application.Current!.Quit();
       Environment.Exit(0);
#endif
    }
    public static void ReloadApp()
    {
        //WIP (Work In Progress)
    }
    #endregion App Configuration
}
