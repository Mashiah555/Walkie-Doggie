using Google.Cloud.Firestore;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
            "איילירלנד", "אלסקן מלמוט", "בולדוג צרפתי", "ביגל", "בורדר קולי", "בוקסר", "בישון פריזה",
            "דוברמן", "דלמטי", "האסקי סיבירי", "הרועה הגרמני", "וויפט", "טרייר סקוטי",
            "לברדור רטריבר", "מלטז", "פודל", "פקינז", "פומרניאן", "פיטבול", "פינצ'ר",
            "צ'יוואווה", "קוקר ספניאל", "רועה בלגי", "רועה שווייצרי", "רוטוויילר", "רידג'בק רודזי", "שיצו",
            "שנאוצר", "שיצו"
        };
    }

    public async void SaveDog()
    {
        bool isDogSaved = await _db.AddDog(
            DogName, DogBirthdate, DogBreed, DogWeight, DefaultFeedAmount);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName) => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
