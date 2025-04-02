using System.ComponentModel;
using System.Windows.Input;
using Walkie_Doggie.Popups;

namespace Walkie_Doggie.ViewModels;

public class WalkViewModel : INotifyPropertyChanged
{
    private readonly FirebaseService _db;

    #region View Model Properties

    string signedUser;
    public string SignedUser
    {
        get => signedUser;
    }

    string walkerName;
    public string WalkerName
    {
        get => walkerName;
        set
        {
            walkerName = value;
            OnPropertyChanged(nameof(WalkerName));
        }
    }

    DateTime walkDate;
    public DateTime WalkDate
    {
        get => walkDate;
        set
        {
            walkDate = value;
            OnPropertyChanged(nameof(WalkDate));
        }
    }

    DateTime walkTime;
    public DateTime WalkTime
    {
        get => walkTime;
        set
        {
            walkTime = value;
            OnPropertyChanged(nameof(WalkTime));
        }
    }

    string? inDebtName;
    public string? InDebtName
    {
        get => inDebtName;
        set
        {
            inDebtName = value;
            OnPropertyChanged(nameof(InDebtName));
        }
    }

    bool? isPayback;
    public bool? IsPayback
    {
        get => isPayback;
        set
        {
            isPayback = value;
            OnPropertyChanged(nameof(IsPayback));
        }
    }

    bool isPooped;
    public bool IsPooped
    {
        get => isPooped;
        set
        {
            isPooped = value;
            OnPropertyChanged(nameof(IsPooped));
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

    int? walkId;
    public int? WalkId
    {
        get => walkId;
        private set
        {
            walkId = value;
            OnPropertyChanged(nameof(WalkId));
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
    public ICommand PaybackCommand { get; }
    public ICommand FavorCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    #endregion View Model Commands

    public WalkViewModel()
    {
        _db = new FirebaseService();

        walkId = -1;
        walkerName = string.Empty;
        walkDate = DateTime.Today;
        walkTime = DateTime.Now;
        inDebtName = null;
        isPayback = null;
        isPooped = true;
        notes = string.Empty;

        try
        {
            signedUser = LocalService.GetUsername() ?? string.Empty;
            WalkId = LocalService.GetWalk();
            users = new List<string> { signedUser };

            InitializeAsync(WalkId);

            PaybackCommand = new Command((_) => OpenUsersPopup(true));
            FavorCommand = new Command((_) => OpenUsersPopup(false));
            SaveCommand = new Command(SaveWalk);
            CancelCommand = new Command(CloseView);
        }
        catch (Exception ex)
        {
            Application.Current!.MainPage!.DisplayAlert(
                "WalkViewModel Error", "WalkViewModel failed to initialize: \n\n" +
                ex.Message, "סגירה", FlowDirection.RightToLeft);
        }
    }

    private async void InitializeAsync(int? id)
    {
        try
        {
            Users = await _db.GetAllUsernamesAsync();

            if (id == null) return;

            WalkModel? walk = await _db.GetWalkAsync(id);
            if (walk == null) return;

            WalkerName = walk.WalkerName;
            WalkDate = walk.WalkTime.ToDateTime().Date;
            WalkTime = walk.WalkTime.ToDateTime().ToLocalTime();
            InDebtName = walk.InDebtName;
            IsPayback = walk.IsPayback;
            IsPooped = walk.IsPooped;
            Notes = walk.Notes ?? string.Empty;
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "WalkViewModel Error", "InitializeAsync failed: \n\n" +
                ex.Message, "סגירה", FlowDirection.RightToLeft);
        }
    }

    private async void OpenUsersPopup(bool payback)
    {
        //UsersSheet sheet = new((result) =>
        //{
        //    this.InDebtName = result;
        //});
        //sheet.Dismissed += (s, e) =>
        //{
        //    OnPropertyChanged(nameof(InDebtName));
        //};
        //await sheet.ShowAsync();
        //await sheet.DismissAsync();

        string result = await MauiPopup.PopupAction.DisplayPopup(new UsersPopup());
        if (!string.IsNullOrEmpty(result))
        {
            InDebtName = result;
            IsPayback = payback;
        }
    }

    public async void SaveWalk()
    {
        await _db.AddWalkAsync(WalkerName, WalkTime, IsPooped, Notes, InDebtName, IsPayback);

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
