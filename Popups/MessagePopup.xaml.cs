using MauiPopup;
using MauiPopup.Views;
using Walkie_Doggie.Helpers;
using System.ComponentModel;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Walkie_Doggie.Popups;

public partial class MessagePopup : BasePopupPage
{
    private MessageViewModel ViewModel { get; set; }
    public MessageResult Result { get; private set; }

    public MessagePopup(
        string header, string msg, 
        ContextImage image = ContextImage.None, 
        ButtonSet buttons = ButtonSet.Ok, string? custom = null)
	{
		InitializeComponent();
        NavigationFlags.IsMessagePopedUp = true;

        string? imagePath = image == ContextImage.None ? null :
            "Context/" + image.ToString().ToLower() + ".png";

        if (buttons == ButtonSet.NoneAndForce)
            IsCloseOnBackgroundClick = false;

        ViewModel = new MessageViewModel(header, msg, imagePath, buttons, custom);
        BindingContext = ViewModel;
    }

    private void MessagePopup_Closing(object sender, EventArgs e)
    {
        NavigationFlags.IsMessagePopedUp = false;
    }

    private void MessagePopup_Opening(object sender, EventArgs e)
    {
        //NavigationFlags.IsMessagePopedUp = true;
    }
}

internal class MessageViewModel
{
    #region View Model Properties
    public string Header { get; set; }
    public string Message { get; set; }
    public ImageSource ImagePath { get; set; }
    public string CustomButton { get; set; } = string.Empty;
    #endregion View Model Properties

    #region Command Properties
    public ICommand YesCommand { get; }
    public ICommand NoCommand { get; }
    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand ConfirmCommand { get; }
    public ICommand CustomCommand { get; }
    #endregion Command Properties

    #region Visibility Properties
    public bool YesVisible { get; set; } = false;
    public bool NoVisible { get; set; } = false;
    public bool OkVisible { get; set; } = false;
    public bool CancelVisible { get; set; } = false;
    public bool CloseVisible { get; set; } = false;
    public bool ConfirmVisible { get; set; } = false;
    public bool CustomVisible { get; set; } = false;
    #endregion Visibility Properties

    public MessageViewModel(string header, string msg, string? path, ButtonSet buttons, string? custom)
    {
        Header = header;
        Message = msg;
        ImagePath = ImageSource.FromFile(path ?? string.Empty);
        CustomButton = custom ?? string.Empty;

        YesCommand = new Command(() => PopupAction.ClosePopup(MessageResult.Yes));
        NoCommand = new Command(() => PopupAction.ClosePopup(MessageResult.No));
        OkCommand = new Command(() => PopupAction.ClosePopup(MessageResult.OK));
        CancelCommand = new Command(() => PopupAction.ClosePopup(MessageResult.Cancel));
        CloseCommand = new Command(() => PopupAction.ClosePopup(MessageResult.Close));
        ConfirmCommand = new Command(() => PopupAction.ClosePopup(MessageResult.Confirm));
        CustomCommand = new Command(() => PopupAction.ClosePopup(MessageResult.Custom));

        SetButtonVisibility(buttons);
    }

    private void SetButtonVisibility(ButtonSet buttons)
    {
        // Show relevant buttons
        switch (buttons)
        {
            case ButtonSet.Ok:
                OkVisible = true;
                break;

            case ButtonSet.Close:
                CloseVisible = true;
                break;

            case ButtonSet.YesNo:
                YesVisible = true;
                NoVisible = true;
                break;

            case ButtonSet.YesNoCancel:
                YesVisible = true;
                NoVisible = true;
                CancelVisible = true;
                break;

            case ButtonSet.OkCancel:
                OkVisible = true;
                CancelVisible = true;
                break;

            case ButtonSet.ConfirmCancel:
                ConfirmVisible = true;
                CancelVisible = true;
                break;

            case ButtonSet.Custom:
                CustomVisible = true;
                break;

            case ButtonSet.CustomCancel:
                CustomVisible = true;
                CancelVisible = true;
                break;

            default:
                // No buttons to show
                break;
        }
    }
}

#region Message Enums

public enum ContextImage
{
    None,
    Info,
    Warning,
    Error,
    Blocked,
    Confirmation,
    Notification,
    NoInternet,
    NotFound,
    Question,
    Exclamation
}

public enum ButtonSet
{
    None,
    NoneAndForce,
    Ok,
    Close,
    YesNo,
    YesNoCancel,
    OkCancel,
    ConfirmCancel,
    Custom,
    CustomCancel
}

public enum MessageResult
{
    Yes,
    No,
    OK,
    Cancel,
    Close,
    Confirm,
    Custom
}
#endregion Message Enums