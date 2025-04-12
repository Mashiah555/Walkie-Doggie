using MauiPopup.Views;

namespace Walkie_Doggie.Popups;

public partial class MessagePopup : BasePopupPage
{
	public MessagePopup(
        string header, string msg, 
        ContextImage image = ContextImage.None, 
        ButtonSet buttons = ButtonSet.Ok, string? special = null)
	{
		InitializeComponent();
	}
}

public enum ContextImage
{
    None,
    Info,
    Warning,
    Error,
    Confirmation,
    Notification,
    NoInternet
}

public enum ButtonSet
{
    Ok,
    Close,
    YesNo,
    YesNoCancel,
    OkCancel,
    ConfirmCancel,
    Custom,
    CustomCancel
}