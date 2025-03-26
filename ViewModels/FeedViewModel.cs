using System.ComponentModel;

namespace Walkie_Doggie.ViewModels;

public class FeedViewModel : INotifyPropertyChanged
{
    private readonly FirebaseService _db;

    #region View Model Properties
    #endregion View Model Properties

    public FeedViewModel()
    {
        _db = new FirebaseService();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
