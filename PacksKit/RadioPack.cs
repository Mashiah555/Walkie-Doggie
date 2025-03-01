using Microsoft.Maui.Controls;

namespace Walkie_Doggie.PacksKit;

public class RadioPack : Frame
{
    #region Bindable Properties
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(RadioPack), string.Empty);
    
    public static readonly BindableProperty GroupNameProperty =
        BindableProperty.Create(nameof(GroupName), typeof(string), typeof(RadioPack), default(string));

    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(RadioPack), false, BindingMode.TwoWay);

    public static readonly BindableProperty ContentStateProperty =
    BindableProperty.Create(nameof(ContentState), typeof(ContentStates), typeof(RadioPack), ContentStates.Normal);

    #endregion Bindable Properties

    #region Properties
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public ContentStates ContentState
    {
        get => (ContentStates)GetValue(ContentStateProperty);
        set => SetValue(ContentStateProperty, value);
    }
    #endregion Properties

    public RadioPack()
    {
        #region RadioButton Initialization
        RadioButton radio = new RadioButton
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Start
        };
        radio.SetBinding(RadioButton.IsCheckedProperty,
            new Binding(nameof(IsChecked), source: this));
        radio.SetBinding(RadioButton.GroupNameProperty, 
            new Binding(nameof(GroupName), source: this));
        #endregion RadioButton Initialization

        #region Label Initialization
        var label = new Label
        {
            Style = (Style)Application.Current!.Resources["SubHeader"],
            FontSize = 18,
            Margin = new Thickness(-4, 1, 3, 2),
            FlowDirection = FlowDirection.RightToLeft
        };
        label.SetBinding(
            Label.TextProperty, new Binding(nameof(Text), source: this));
        #endregion Label Initialization

        #region StackLayout Initialization
        var stack = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            FlowDirection = FlowDirection.RightToLeft
        };

        stack.Children.Add(radio);
        stack.Children.Add(label);
        #endregion StackLayout Initialization

        #region Frame Initialization
        Content = stack;
        Style = (Style)Application.Current!.Resources["SelectableFrame"];
        Margin = new Thickness(4);
        Padding = new Thickness(5, 0);
        FlowDirection = FlowDirection.RightToLeft;
        InputTransparent = true;
        #endregion Frame Initialization
    }
}
