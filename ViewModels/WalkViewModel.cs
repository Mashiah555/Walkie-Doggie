﻿using System.ComponentModel;
using System.Windows.Input;
using Walkie_Doggie.Popups;
using Walkie_Doggie.Services;

namespace Walkie_Doggie.ViewModels;

public class WalkViewModel : INotifyPropertyChanged
{

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

    TimeSpan walkTime;
    public TimeSpan WalkTime
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
    public ICommand PaybackCommand { get; }
    public ICommand FavorCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    #endregion View Model Commands

    public WalkViewModel()
    {
        signedUser = LocalService.GetUsername() ?? string.Empty;
        walkerName = string.Empty;
        walkDate = DateTime.Today;
        walkTime = DateTime.Now.TimeOfDay;
        inDebtName = null;
        isPayback = null;
        isPooped = true;
        notes = string.Empty;
        users = new List<string> { signedUser };

        InitializeAsync();

        PaybackCommand = new Command((_) => OpenUsersPopup(true));
        FavorCommand = new Command((_) => OpenUsersPopup(false));
        SaveCommand = new Command(SaveWalk);
        CancelCommand = new Command(CloseView);
    }
    private async void InitializeAsync()
    {
        try
        {
            Users = await DbService.Users.GetAllUsernamesAsync();

            //if (id == null) return;

            //WalkModel? walk = await _db.GetWalkAsync(id);
            //if (walk == null) return;

            //WalkerName = walk.WalkerName;
            //WalkDate = walk.WalkTime.ToDateTime().Date;
            //WalkTime = walk.WalkTime.ToDateTime().ToLocalTime();
            //InDebtName = walk.InDebtName;
            //IsPayback = walk.IsPayback;
            //IsPooped = walk.IsPooped;
            //Notes = walk.Notes ?? string.Empty;
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
        string result = await MauiPopup.PopupAction.DisplayPopup(new UsersPopup());
        if (!string.IsNullOrEmpty(result))
        {
            InDebtName = result;
            IsPayback = payback;
        }
    }

    public async void SaveWalk()
    {
        DateTime fullTime = WalkDate.Date + WalkTime;
        await DbService.Walks.AddAsync(
            WalkerName, 
            fullTime, 
            isPooped, Notes, 
            inDebtName, IsPayback);

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
