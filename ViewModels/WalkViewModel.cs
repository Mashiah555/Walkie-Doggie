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
    #endregion View Model Properties

    #region View Model Commands
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    #endregion View Model Commands

    public WalkViewModel(int? walkId)
    {
        _db = new FirebaseService();

        walkerName = string.Empty;
        InitializeAsync(walkId);

        SaveCommand = new Command(SaveWalk);
        CancelCommand = new Command(CloseView);
    }
    private async void InitializeAsync(int? id)
    {
        if (id is not null)
        {
            WalkModel? walk = await _db.GetWalkAsync(id.Value);
            if (walk is not null)
            {
                walkerName = walk.WalkerName;
                walkDate = walk.WalkTime.ToDateTime().Date;
                walkTime = walk.WalkTime.ToDateTime().ToLocalTime();
                inDebtName = walk.InDebtName;
                isPayback = walk.IsPayback;
                isPooped = walk.IsPooped;
                notes = walk.Notes ?? string.Empty;

                return;
            }
        }

        walkerName = string.Empty;
        walkDate = DateTime.Today;
        walkTime = DateTime.Now;
        inDebtName = null;
        isPayback = null;
        isPooped = true;
        notes = string.Empty;
    }

    public async void SaveWalk()
    {
        CloseView();
    }

    public async void CloseView()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}