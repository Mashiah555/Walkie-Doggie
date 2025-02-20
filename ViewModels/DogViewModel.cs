using Google.Cloud.Firestore;
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

    float dogWeight;
    public float DogWeight
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
    #endregion View Model Properties

    public DogViewModel()
    {
        _db = new FirebaseService();
        dogName = string.Empty;
        dogBirthdate = DateTime.Now;
        dogBreed = string.Empty;
        dogWeight = 0;
        defaultFeedAmount = 0;
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
