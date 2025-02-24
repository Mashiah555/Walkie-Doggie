using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using Google.Cloud.Firestore;
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
        get => dogBreeds;
    }

    bool hasDog;
    public bool HasDog
    {
        get => hasDog;
    }
    #endregion View Model Properties

    public ICommand SaveCommand { get; }

    public DogViewModel()
    {
        _db = new FirebaseService();

        dogBreeds = new ObservableCollection<string>
        {
            "איילירלנד", "אלסקן מלמוט", "בולדוג צרפתי", "ביגל", "בורדר קולי", "בוקסר",
            "בישון פריזה", "דוברמן", "דלמטי", "האסקי סיבירי", "הרועה הגרמני", "וויפט",
            "טרייר סקוטי", "לברדור רטריבר", "מלטז", "פודל", "פקינז", "פומרניאן", "פיטבול",
            "פינצ'ר", "צ'יוואווה", "קוקר ספניאל", "רועה בלגי", "רועה שווייצרי",
            "רוטוויילר", "רידג'בק רודזי", "שיצו", "שנאוצר"
        };
        dogBreeds = dogBreeds.Order().ToObservableCollection();
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
            Application.Current!.MainPage = new AppShell();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName) => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
