using System.ComponentModel;
using System.Windows.Input;
using Walkie_Doggie.Helpers;
using Walkie_Doggie.Services;

namespace Walkie_Doggie.ViewModels;

public class FeedViewModel : INotifyPropertyChanged
{
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

    IEnumerable<string> users;
    public IEnumerable<string> Users
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
        signedUser = LocalService.GetUsername() ?? string.Empty;
        users = Collections.Usernames;
        feederName = string.Empty;
        feedDate = DateTime.Today;
        feedTime = DateTime.Now;
        feedAmount = 0;
        notes = string.Empty;

        InitializeAsync();

        SaveCommand = new Command(SaveFeed);
        CancelCommand = new Command(CloseView);
    }
    private async void InitializeAsync()
    {
        DogModel? dog = await DbService.Dogs.GetAsync();
        FeedAmount = dog is null ? 0 : dog.DefaultFeedAmount;
    }

    public async void SaveFeed()
    {
        await DbService.Feeds.AddAsync(FeederName, FeedTime, FeedAmount, Notes);

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
