using CommunityToolkit.Maui.Views;

namespace Walkie_Doggie.Popups;

public partial class MessagePopup : Popup
{
	public MessagePopup(string msg, 
        string? btn1 = null, string? btn2 = null, string? btn3 = null, string? btn4 = null)
	{
		InitializeComponent();

        MessageContent.Text = msg;
        InitializeButton(Button1, btn1);
        InitializeButton(Button2, btn2);
        InitializeButton(Button3, btn3);
        InitializeButton(Button4, btn4);
    }

    private void InitializeButton(Button btn, string? str)
    {
		if (str is null || str == string.Empty)
			Button1.IsVisible = false;
		else
			btn.Text = str;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Close(btn.Text);
    }
}