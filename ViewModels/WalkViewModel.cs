using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

    public WalkViewModel()
    {
        _db = new FirebaseService();

        signedUser = LocalService.GetUsername() ?? string.Empty;
        walkId = LocalService.GetWalk();
        walkerName = string.Empty;
        walkDate = DateTime.Today;
        walkTime = DateTime.Now;
        inDebtName = null;
        isPayback = null;
        isPooped = true;
        notes = string.Empty;

        InitializeAsync(WalkId);

        SaveCommand = new Command(SaveWalk);
        CancelCommand = new Command(CloseView);
    }
    private async void InitializeAsync(int? id)
    {
        Users = await _db.GetAllUsernamesAsync();

        if (id == null) return;

        WalkModel? walk = await _db.GetWalkAsync(id ?? -1);
        if (walk == null) return;

        WalkerName = walk.WalkerName;
        WalkDate = walk.WalkTime.ToDateTime().Date;
        WalkTime = walk.WalkTime.ToDateTime().ToLocalTime();
        InDebtName = walk.InDebtName;
        IsPayback = walk.IsPayback;
        IsPooped = walk.IsPooped;
        Notes = walk.Notes ?? string.Empty;
    }

    public async void SaveWalk()
    {
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