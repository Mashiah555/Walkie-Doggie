using MauiPopup;
using MauiPopup.Views;
using System.ComponentModel;
using Walkie_Doggie.PacksKit;

namespace Walkie_Doggie.Popups;

public partial class UsersPopup : BasePopupPage
{
    private UsersPopupViewModel ViewModel { get; set; }
    public UsersPopup()
	{
		InitializeComponent();
        ViewModel = new UsersPopupViewModel();
        BindingContext = ViewModel;
    }
}

internal class UsersPopupViewModel : INotifyPropertyChanged
{
    private readonly FirebaseService _db;

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

    public UsersPopupViewModel()
    {
        _db = new FirebaseService();

        selectedItem = null;
        InitializeAsync();
    }
    private async void InitializeAsync()
    {
        List<ItemPack> usersConverter = new();
        List<UserModel> usersAsync = await _db.GetAllUsersAsync();

        foreach (var user in usersAsync)
        {
            usersConverter.Add(new ItemPack
            {
                MainText = user.Name,
                SecondaryText = "0",
                ContextText = "TBD",
                ImageSource = Application.Current!.UserAppTheme == AppTheme.Light ?
                    "user_light" : "user_dark"
            });
        }

        UsersList = usersConverter.ToList();
    }

    public void CompleteWithResult(ItemPack result)
    {
        PopupAction.ClosePopup(result.MainText);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (propertyName == nameof(SelectedItem) && SelectedItem != null)
            CompleteWithResult(SelectedItem);
    }
}