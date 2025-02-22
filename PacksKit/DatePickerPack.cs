using Microsoft.Maui.Controls.Shapes;
namespace Walkie_Doggie.PacksKit;

public class DatePickerPack : StackLayout
{
    #region Bindable Properties
    public static readonly BindableProperty DateProperty =
        BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(DatePickerPack), DateTime.Today, BindingMode.TwoWay);

    public static readonly BindableProperty FloatingLabelProperty =
        BindableProperty.Create(nameof(FloatingLabel), typeof(string), typeof(DatePickerPack), string.Empty);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(DatePickerPack), default(ImageSource));

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(DatePickerPack), string.Empty);

    public static readonly BindableProperty ContentStateProperty =
    BindableProperty.Create(nameof(ContentState), typeof(ContentStates), typeof(DatePickerPack), ContentStates.Normal);
    #endregion Bindable Properties

    #region Properties
    public DateTime Date
    {
        get => (DateTime)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
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
    #endregion Properties

    public DatePickerPack()
    {
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

        #region DatePicker Initialization
        var datePicker = new DatePicker
        {
            FontSize = 18,
            Margin = new Thickness(-4, 1, 3, 2),
            FlowDirection = FlowDirection.RightToLeft
        };
        datePicker.SetBinding(
            DatePicker.DateProperty, new Binding(
                nameof(Date), source: this, mode: BindingMode.TwoWay));
        #endregion DatePicker Initialization

        #region Grid & Border Initialization
        var grid = new Grid() { FlowDirection = FlowDirection.RightToLeft };
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        grid.Children.Add(image);
        grid.Children.Add(datePicker);
        grid.SetColumn(image, 0);
        grid.SetColumn(datePicker, 1);

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
        var descriptionLabel = new Label
        {
            Style = (Style)Application.Current!.Resources["Context"],
            FontSize = 12,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(5, -2, 5, 5)
        };
        descriptionLabel.SetBinding(
            Label.TextProperty, new Binding(nameof(Description), source: this));
        descriptionLabel.SetBinding(
            Label.TextColorProperty, new Binding(
            nameof(ContentState), source: this, converter: new ConvertToColor()));
        #endregion Description Label Initialization

        //Initialize the DatePickerPack's StackLayout with components:
        Children.Add(floatingLabel);
        Children.Add(border);
        Children.Add(descriptionLabel);
        FlowDirection = FlowDirection.RightToLeft;
    }
}
