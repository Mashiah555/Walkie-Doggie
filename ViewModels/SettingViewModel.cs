﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Walkie_Doggie.Pages;
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

    bool hasChanges;
    public bool HasChanges
    {
        get => hasChanges;
        set
        {
            hasChanges = value;
            OnPropertyChanged(nameof(HasChanges));
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
                ApplyTheme(value); // Apply theme update
                OnPropertyChanged(nameof(Theme)); // Notify UI about change
            }
        }
    }

    ObservableCollection<string> themes;
    public ObservableCollection<string> Themes
    {
        get => themes;
    }
    #endregion View Model Properties

    #region View Model Commands
    public ICommand SyncChanges { get; }
    public ICommand NavigateToDog { get; }
    public ICommand NavigateToLogin { get; }
    #endregion View Model Commands

    public SettingViewModel()
    {
        _db = new FirebaseService();

        themes = new ObservableCollection<string> { "ברירת המחדל של המערכת", "בהיר", "כהה" };
        name = LocalService.GetUsername() ?? string.Empty;
        theme = LocalService.GetTheme();

        SyncChanges = new Command(SyncToFirebase);
        NavigateToDog = new Command(GoToDogView);
        NavigateToLogin = new Command(GoToLoginPage);
    }

    private async void GoToDogView()
    {
        await Shell.Current.GoToAsync(nameof(DogView), true);
    }

    private async void GoToLoginPage()
    {
        bool result = await Application.Current!.MainPage!.DisplayAlert(
            "התנתקות", "האם את/ה בטוח/ה שברצונך להתנתק מהמערכת?",
                "אישור", "ביטול", FlowDirection.RightToLeft);

        if (result)
        {
            LocalService.RemoveUsername();
            await Toast.Make("התנתקת מהמערכת", ToastDuration.Short).Show();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }

    private async void SyncToFirebase()
    {
        await _db.UpdateUserAsync(Name, Theme);
    }

    private void ApplyTheme(AppTheme newTheme)
    {
        switch (newTheme)
        {
            case AppTheme.Light:
                Application.Current!.UserAppTheme = AppTheme.Light;
                break;

            case AppTheme.Dark:
                Application.Current!.UserAppTheme = AppTheme.Dark;
                break;

            default:
                Application.Current!.UserAppTheme = AppTheme.Unspecified;
                break;
        }

        App.ApplyTheme(); // Apply colors immediately
        LocalService.SetTheme(newTheme); // Save theme locally

    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName)
    {
        if (propertyName != nameof(HasChanges))
            HasChanges = true;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
