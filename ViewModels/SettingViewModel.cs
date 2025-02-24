using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Walkie_Doggie.Views;

namespace Walkie_Doggie.ViewModels;

class SettingViewModel : INotifyPropertyChanged
{
    private readonly FirebaseService _db;

    #region View Model Properties
    string name;
    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    AppTheme theme;
    public AppTheme Theme
    {
        get => theme;
        set
        {
            if (theme != value)
            {
                theme = value;
                OnPropertyChanged(nameof(Theme)); // Notify UI about change
                ApplyTheme(value); // Apply theme update
            }
        }
    }

    ObservableCollection<string> themes;
    public ObservableCollection<string> Themes
    {
        get => themes;
    }
    #endregion View Model Properties

    public ICommand Navigate { get; }

    public SettingViewModel()
    {
        _db = new FirebaseService();

        themes = new ObservableCollection<string> { "ברירת המחדל של המערכת", "בהיר", "כהה" };
        InitializeAsync();

        Navigate = new Command(NavigateToDog);
    }
    private async void InitializeAsync()
    {
        try
        {
            UserModel? user = await _db.GetUserAsync(LocalService.GetUsername() ??
                throw new Exception("You must be signed in!"));

            if (user is null)
            {
                Name = string.Empty;
                Theme = AppTheme.Unspecified;
            }
            else
            {
                Name = user.Name;
                Theme = user.Theme;
            }
        }
        catch { }
    }

    private async void NavigateToDog()
    {
        await Shell.Current.GoToAsync(nameof(DogView), true);
    }

    private async void ApplyTheme(AppTheme theme)
    {
        switch (theme)
        {
            case AppTheme.Light:
                Application.Current!.UserAppTheme = AppTheme.Light;
                Preferences.Set("AppTheme", "Light");
                break;

            case AppTheme.Dark:
                Application.Current!.UserAppTheme = AppTheme.Dark;
                Preferences.Set("AppTheme", "Dark");
                break;

            default:
                Application.Current!.UserAppTheme = AppTheme.Unspecified;
                Preferences.Set("AppTheme", "System");
                break;
        }

        App.ApplyTheme(); // Apply colors immediately
        await _db.UpdateUserAsync(Name, theme);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
