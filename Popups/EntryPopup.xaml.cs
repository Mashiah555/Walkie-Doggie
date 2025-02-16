using CommunityToolkit.Maui.Views;

namespace Walkie_Doggie.Popups;

public partial class EntryPopup : Popup
{
    public EntryPopup(string msg, string placeholder, Keyboard keyboard, string saveButton, string cancelButton)
	{
		InitializeComponent();

        MessageContent.Text = msg;
        ValueEntry.Placeholder = placeholder;
        ValueEntry.Keyboard = keyboard;
        ButtonSave.Text = saveButton;
        ButtonCancel.Text = cancelButton;
    }

    private void ButtonSave_Clicked(object sender, EventArgs e)
    {
        Close(ValueEntry.Text);
    }

    private void ButtonCancel_Clicked(object sender, EventArgs e)
    {
        Close(null);
    }
}