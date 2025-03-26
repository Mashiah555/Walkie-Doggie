using Microsoft.Maui.Controls.Shapes;

namespace Walkie_Doggie.PacksKit;

public class EntryPack : StackLayout
{
    #region Bindable Properties
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryPack), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EntryPack), string.Empty);

    public static readonly BindableProperty FloatingLabelProperty =
        BindableProperty.Create(nameof(FloatingLabel), typeof(string), typeof(EntryPack), string.Empty);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(EntryPack), default(ImageSource));

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(EntryPack), string.Empty);

    public static readonly BindableProperty ContentStateProperty =
    BindableProperty.Create(nameof(ContentState), typeof(ContentStates), typeof(EntryPack), ContentStates.Normal);

    public static readonly BindableProperty MandatoryProperty =
        BindableProperty.Create(nameof(Mandatory), typeof(bool), typeof(EntryPack), false);
    #endregion Bindable Properties

    #region Properties
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string FloatingLabel
    {
        get => (string)GetValue(FloatingLabelProperty);
        set => SetValue(FloatingLabelProperty, value);
    }

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public ContentStates ContentState
    {
        get => (ContentStates)GetValue(ContentStateProperty);
        set => SetValue(ContentStateProperty, value);
    }

    public bool Mandatory
    {
        get => (bool)GetValue(MandatoryProperty);
        set => SetValue(MandatoryProperty, value);
    }
    #endregion Properties

    public EntryPack()
    {
        if (Mandatory && string.IsNullOrWhiteSpace(Description))
            Description = "שדה חובה";

        #region Floating Label Initialization
        var floatingLabel = new Label
        {
            Style = (Style)Application.Current!.Resources["SubHeader"],
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(9, 5, 5, -2),
            FlowDirection = FlowDirection.RightToLeft
        };
        floatingLabel.SetBinding(
            Label.TextProperty, new Binding(nameof(FloatingLabel), source: this));
        #endregion Floating Label Initialization

        #region Image Initialization
        var image = new Image 
        { 
            WidthRequest = 38, 
            HeightRequest = 38, 
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(-2)
        };
        image.SetBinding(
            Image.SourceProperty, new Binding(nameof(ImageSource), source: this));
        image.SetBinding(IsVisibleProperty, new Binding(
            nameof(ImageSource), source: this, converter: new ConvertToBool()));
        #endregion Image Initialization

        #region Entry Initialization
        var entry = new Entry 
        { 
            FontSize = 18, 
            Margin = new Thickness(-4, 1, 3, 2),
            FlowDirection = FlowDirection.RightToLeft
        };
        entry.SetBinding(
            Entry.TextProperty, new Binding(
                nameof(Text), source: this, mode: BindingMode.TwoWay));
        entry.SetBinding(
            Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
        entry.TextChanged += Entry_TextChanged;
        entry.Unfocused += Entry_Unfocused;
        #endregion Entry Initialization

        #region Grid & Border Initialization
        var grid = new Grid() { FlowDirection = FlowDirection.RightToLeft };
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        grid.Children.Add(image);
        grid.Children.Add(entry);
        grid.SetColumn(image, 0);
        grid.SetColumn(entry, 1);

        var border = new Border
        {
            Content = grid,
            BackgroundColor = Colors.Transparent,
            Margin = new Thickness(5, 2),
            Padding = new Thickness(5, -3),
            FlowDirection = FlowDirection.RightToLeft,
            Stroke = Application.Current!.UserAppTheme == AppTheme.Light ?
                (Color)Application.Current!.Resources["Gray200"] :
                (Color)Application.Current!.Resources["Gray900"],
            StrokeThickness = 0.5,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(8)  // You can adjust this value to make corners more or less rounded
            }
        };
        border.SetBinding(Border.StrokeProperty, new Binding(
            nameof(ContentState), source: this, converter: new ConvertToColor()));
        #endregion Grid & Border Initialization

        #region Description Label Initialization
        Label descriptionLabel = new Label 
        {
            Style = (Style)Application.Current!.Resources["Context"],
            FontSize = 12, 
            HorizontalOptions = LayoutOptions.End,
            Margin = new Thickness(5, -2, 5, 5) 
        };

        descriptionLabel.SetBinding(
            Label.TextProperty, new Binding(nameof(Description), source: this));
        descriptionLabel.SetBinding(
            Label.TextColorProperty, new Binding(
            nameof(ContentState), source: this, converter: new ConvertToColor()));
        #endregion Description Label Initialization

        //Initialize the EntryPack's StackLayout with components:
        Children.Add(floatingLabel);
        Children.Add(border);
        Children.Add(descriptionLabel);
        FlowDirection = FlowDirection.RightToLeft;
    }

    private void Entry_Unfocused(object? sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.IsFocused && Mandatory)
        {
            ContentState = string.IsNullOrWhiteSpace(entry.Text)
                ? ContentStates.Error : ContentStates.Normal;
        }
    }

    private void Entry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.IsFocused && Mandatory)
        {
            ContentState = string.IsNullOrWhiteSpace(e.NewTextValue)
                ? ContentStates.Error : ContentStates.Normal;
        }
    }
}
