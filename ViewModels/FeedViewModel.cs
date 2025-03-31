using System.ComponentModel;
using System.Windows.Input;

namespace Walkie_Doggie.ViewModels;

public class FeedViewModel : INotifyPropertyChanged
{
    private readonly FirebaseService _db;

    #region View Model Properties
    string signedUser;
    public string SignedUser
    {
        get => signedUser;
    }

    string feederName;
    public string FeederName
    {
        get => feederName;
        set
        {
            feederName = value;
            OnPropertyChanged(nameof(FeederName));
        }
    }

    DateTime feedDate;
    public DateTime FeedDate
    {
        get => feedDate;
        set
        {
            feedDate = value;
            OnPropertyChanged(nameof(FeedDate));
        }
    }

    DateTime feedTime;
    public DateTime FeedTime
    {
        get => feedTime;
        set
        {
            feedTime = value;
            OnPropertyChanged(nameof(FeedTime));
        }
    }

    int feedAmount;
    public int FeedAmount
    {
        get => feedAmount;
        set
        {
            feedAmount = value;
            OnPropertyChanged(nameof(FeedAmount));
        }
    }

    string notes;
    public string Notes
    {
        get => notes;
        set
        {
            notes = value;
            OnPropertyChanged(nameof(Notes));
        }
    }

    List<string> users;
    public List<string> Users
    {
        get => users;
        set
        {
            users = value;
            OnPropertyChanged(nameof(Users));
        }
    }
    #endregion View Model Properties

    #region View Model Commands
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    #endregion View Model Commands

    public FeedViewModel()
    {
        _db = new FirebaseService();

        signedUser = LocalService.GetUsername() ?? string.Empty;
        feederName = string.Empty;
        feedDate = DateTime.Today;
        feedTime = DateTime.Now;
        feedAmount = 0;
        notes = string.Empty;
        users = new List<string> { signedUser };

        InitializeAsync();

        SaveCommand = new Command(SaveFeed);
        CancelCommand = new Command(CloseView);
    }
    private async void InitializeAsync()
    {
        Users = await _db.GetAllUsernamesAsync();

        DogModel dog = await _db.GetDogAsync();
        FeedAmount = dog.DefaultFeedAmount;
    }

    public async void SaveFeed()
    {
        await _db.AddFeedAsync(FeederName, FeedTime, FeedAmount, Notes);

        await Shell.Current.GoToAsync("..", true);
    }

    public async void CloseView()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
