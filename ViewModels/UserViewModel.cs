using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Walkie_Doggie.ViewModels;

public class UserViewModel : INotifyPropertyChanged
{
    public UserViewModel()
    {
        // Initialize default values for properties if needed
        SignedUser = null;
        SelectedUser = null;
    }

    #region ViewModel Properties

    private UserModel? _signedUser;
    public UserModel? SignedUser
    {
        get => _signedUser;
        set
        {
            if (_signedUser != value)
            {
                _signedUser = value;
                OnPropertyChanged();
            }
        }
    }

    private UserModel? _selectedUser;
    public UserModel? SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (_selectedUser != value)
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion ViewModel Properties

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
