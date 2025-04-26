using MauiPopup;
using MauiPopup.Views;

namespace Walkie_Doggie.Popups;

public partial class EntryPopup : BasePopupPage
{
    public EntryPopup(string msg, string placeholder, Keyboard keyboard, string saveButton, string cancelButton)
	{
		InitializeComponent();

        MessageContent.Text = msg;
        ValueEntry.Placeholder = placeholder;
        ValueEntry.KeyboardStyle = keyboard;
        ButtonSave.Text = saveButton;
        ButtonCancel.Text = cancelButton;
    }

    private void ButtonSave_Clicked(object sender, EventArgs e)
    {
        PopupAction.ClosePopup(ValueEntry.Text);
    }

    private void ButtonCancel_Clicked(object sender, EventArgs e)
    {
        PopupAction.ClosePopup(string.Empty);
    }
}