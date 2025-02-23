﻿using Google.Cloud.Firestore;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Walkie_Doggie.ViewModels;

public class DogViewModel : INotifyPropertyChanged
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

    DateTime dogBirthdate;
    public DateTime DogBirthdate
    {
        get => dogBirthdate;
        set
        {
            dogBirthdate = value;
            OnPropertyChanged(nameof(DogBirthdate));
        }
    }

    string dogBreed;
    public string DogBreed
    {
        get => dogBreed;
        set
        {
            dogBreed = value;
            OnPropertyChanged(nameof(DogBreed));
        }
    }

    double dogWeight;
    public double DogWeight
    {
        get => dogWeight;
        set
        {
            dogWeight = value;
            OnPropertyChanged(nameof(DogWeight));
        }
    }

    int defaultFeedAmount;
    public int DefaultFeedAmount
    {
        get => defaultFeedAmount;
        set
        {
            defaultFeedAmount = value;
            OnPropertyChanged(nameof(DefaultFeedAmount));
        }
    }

    ObservableCollection<string> dogBreeds;
    public ObservableCollection<string> DogBreeds
    {
        get => (ObservableCollection<string>)dogBreeds.Order();
    }

    bool hasDog;
    public bool HasDog
    {
        get => hasDog;
    }

    public ICommand SaveCommand { get; }
    #endregion View Model Properties

    public DogViewModel()
    {
        _db = new FirebaseService();

        dogName = string.Empty;
        dogBirthdate = DateTime.Now;
        dogBreed = string.Empty;
        dogWeight = 8;
        defaultFeedAmount = 75;
        dogBreeds = new ObservableCollection<string>
        {
            "איילירלנד", "אלסקן מלמוט", "בולדוג צרפתי", "ביגל", "בורדר קולי", "בוקסר",
            "בישון פריזה", "דוברמן", "דלמטי", "האסקי סיבירי", "הרועה הגרמני", "וויפט",
            "טרייר סקוטי", "לברדור רטריבר", "מלטז", "פודל", "פקינז", "פומרניאן", "פיטבול",
            "פינצ'ר", "צ'יוואווה", "קוקר ספניאל", "רועה בלגי", "רועה שווייצרי",
            "רוטוויילר", "רידג'בק רודזי", "שיצו", "שנאוצר"
        };
        InitializeAsync();

        SaveCommand = new Command(SaveClick);
    }
    public async void InitializeAsync() => hasDog = await _db.HasDog();

    public async void SaveClick()
    {
        if (string.IsNullOrWhiteSpace(dogName) || string.IsNullOrWhiteSpace(dogBreed))
        {
            await DisplayAlert("שמירה נכשלה", 
                "אחד או יותר מהשדות ריקים. חובה למלא את כל השדות לפני ביצוע שמירה!", "סגירה");
            return;
        }

        if (await _db.HasDog())
            await _db.UpdateDog(dogBirthdate, dogBreed, dogWeight, defaultFeedAmount);
        else
            await _db.AddDog(dogName, dogBirthdate, dogBreed, dogWeight, defaultFeedAmount);
    }

    private Task DisplayAlert(string title, string message, string cancel)
    {
        // Implement your alert display logic here
        // For example, if using Xamarin.Forms:
        // return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        return Task.CompletedTask;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName) => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
