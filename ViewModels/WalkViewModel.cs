using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Walkie_Doggie.ViewModels;

public class WalkViewModel : INotifyPropertyChanged
{
    private readonly FirebaseService _db;

    #region View Model Properties
    string dogName;
    public string DogName
    {
        get => dogName;
        set
        {
            dogName = value;
            OnPropertyChanged(nameof(DogName));
        }
    }
    #endregion View Model Properties

    public ICommand SaveCommand { get; }

    public WalkViewModel()
    {
        _db = new FirebaseService();

        InitializeAsync();

        SaveCommand = new Command(SaveClick);
    }
    private async void InitializeAsync()
    {
        try
        {
            hasDog = await _db.HasDog();
            if (hasDog)
            {
                DogModel dog = await _db.GetDogAsync();

                dogName = dog.DogName;
                dogBirthdate = dog.DogBirthdate.ToDateTime();
                dogBreed = dog.DogBreed;
                dogWeight = dog.DogWeight;
                defaultFeedAmount = dog.DefaultFeedAmount;
            }
            else
            {
                dogName = string.Empty;
                dogBirthdate = DateTime.Today;
                dogBreed = string.Empty;
                dogWeight = 8;
                defaultFeedAmount = 75;
            }
        }
        catch { }
    }

    public async void SaveClick()
    {
        if (string.IsNullOrWhiteSpace(dogName) || string.IsNullOrWhiteSpace(dogBreed))
        {
            await Application.Current!.MainPage!.DisplayAlert("שמירה נכשלה",
                "אחד או יותר מהשדות ריקים. חובה למלא את כל השדות לפני ביצוע שמירה!", "סגירה");
            return;
        }

        if (await _db.HasDog())
        {
            await _db.UpdateDogAsync(dogBirthdate, dogBreed,
                Math.Floor(dogWeight * 2 + 0.5) / 2, defaultFeedAmount);

            await Toast.Make("השינויים נשמרו", ToastDuration.Short).Show();
            await Shell.Current.GoToAsync("..", true);
        }
        else
        {
            await _db.AddDogAsync(dogName, dogBirthdate, dogBreed,
                Math.Floor(dogWeight * 2 + 0.5) / 2, defaultFeedAmount);

            await Toast.Make("השינויים נשמרו", ToastDuration.Short).Show();
            await Shell.Current.GoToAsync($"//{nameof(AppShell)}");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
