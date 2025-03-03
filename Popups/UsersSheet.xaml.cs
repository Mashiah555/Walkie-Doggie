using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using The49.Maui.BottomSheet;
using Walkie_Doggie.PacksKit;

namespace Walkie_Doggie.Popups;

public partial class UsersSheet : BottomSheet
{
    public UsersSheetViewModel ViewModel { get; set; }
    public UsersSheet(Action<string> callback)
	{
		InitializeComponent();
        ViewModel = new UsersSheetViewModel(callback);
        BindingContext = ViewModel;
    }
}

public class UsersSheetViewModel : INotifyPropertyChanged
{
    private readonly FirebaseService _db;
    private Action<string> _callback;

    #region View Model Properties

    List<ItemPack>? usersList;
    public List<ItemPack>? UsersList
    {
        get => usersList;
        set
        {
            if (usersList != value)
            {
                usersList = value;
                OnPropertyChanged(nameof(UsersList));
            }
        }
    }

    ItemPack? selectedItem;
    public ItemPack? SelectedItem
    {
        get => selectedItem;
        set
        {
            if (selectedItem != value)
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
    #endregion View Model Properties

    public UsersSheetViewModel(Action<string> callback)
    {
        _db = new FirebaseService();
        _callback = callback;

        selectedItem = null;
        InitializeAsync();
        _callback = callback;
    }
    private async void InitializeAsync()
    {
        List <ItemPack> usersConverter= new();
        List<UserModel> usersAsync = await _db.GetAllUsersAsync();

        foreach (var user in usersAsync)
        {
            usersConverter.Add(new ItemPack
            {
                MainText = user.Name,
                SecondaryText = "0",
                ContextText = "?????",
                ImageSource = Application.Current!.UserAppTheme == AppTheme.Light ?
                    "user_light" : "user_dark"
            });
        }

        UsersList = usersConverter.ToList();
    }

    public void CompleteWithResult(ItemPack result)
    {
        _callback?.Invoke(result.MainText);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (propertyName == nameof(SelectedItem) && SelectedItem != null)
            CompleteWithResult(SelectedItem);
    }
}
