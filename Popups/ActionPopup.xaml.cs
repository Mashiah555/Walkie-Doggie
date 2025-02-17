using CommunityToolkit.Maui.Views;

namespace Walkie_Doggie.Popups;

public partial class ActionPopup : Popup
{
	public ActionPopup(string msg, 
        string? btn1 = null, string? btn2 = null, string? btn3 = null, string? btn4 = null)
	{
		InitializeComponent();

        MessageContent.Text = msg;
        InitializeButton(Button1, btn1);
        InitializeButton(Button2, btn2);
        InitializeButton(Button3, btn3);
        InitializeButton(Button4, btn4);
    }

    private void InitializeButton(Button btn, string? content)
    {
		if (content is null || content == string.Empty)
			btn.IsVisible = false;
		else
			btn.Text = content;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Close(btn.Text);
    }
}