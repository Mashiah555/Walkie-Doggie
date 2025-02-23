using Microsoft.Maui.Controls.Shapes;
using System.Collections;
using System.Collections.ObjectModel;

namespace Walkie_Doggie.PacksKit;

public class PickerPack : StackLayout
{
    #region Bindable Properties
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(PickerPack), new ObservableCollection<string>(), BindingMode.TwoWay);

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(PickerPack), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty PickerTitleProperty =
        BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(PickerPack), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty FloatingLabelProperty =
        BindableProperty.Create(nameof(FloatingLabel), typeof(string), typeof(PickerPack), string.Empty);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(PickerPack), default(ImageSource));

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(PickerPack), string.Empty);

    public static readonly BindableProperty ContentStateProperty =
    BindableProperty.Create(nameof(ContentState), typeof(ContentStates), typeof(PickerPack), ContentStates.Normal);

    public static readonly BindableProperty MandatoryProperty =
        BindableProperty.Create(nameof(Mandatory), typeof(bool), typeof(EntryPack), false);
    #endregion Bindable Properties

    #region Properties
    public IList ItemsSource
    {
        get => (IList)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public string SelectedItem
    {
        get => (string)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public string PickerTitle
    {
        get => (string)GetValue(PickerTitleProperty);
        set => SetValue(PickerTitleProperty, value);
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

    public PickerPack()
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
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(-5,-2)
        };
        image.SetBinding(
            Image.SourceProperty, new Binding(nameof(ImageSource), source: this));
        image.SetBinding(IsVisibleProperty, new Binding(
            nameof(ImageSource), source: this, converter: new ConvertToBool()));
        #endregion Image Initialization

        #region Picker Initialization
        var picker = new Picker
        {
            FontSize = 18,
            Margin = new Thickness(-4, 1, 3, 2),
            HorizontalTextAlignment = TextAlignment.Start,
            FlowDirection = FlowDirection.RightToLeft
        };
        picker.SetBinding(
            Picker.ItemsSourceProperty, new Binding(
                nameof(ItemsSource), source: this));
        picker.SetBinding(
            Picker.SelectedItemProperty, new Binding(
                nameof(SelectedItem), source: this, mode: BindingMode.TwoWay));
        picker.SetBinding(
            Picker.TitleProperty, new Binding(
                nameof(PickerTitle), source: this));
        picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
        picker.Unfocused += Picker_Unfocused;
        #endregion Picker Initialization

        #region Grid & Border Initialization
        var grid = new Grid() { FlowDirection = FlowDirection.RightToLeft };
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        grid.Children.Add(image);
        grid.Children.Add(picker);
        grid.SetColumn(image, 0);
        grid.SetColumn(picker, 1);

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

        //Initialize the PickerPack's StackLayout with components:
        Children.Add(floatingLabel);
        Children.Add(border);
        Children.Add(descriptionLabel);
        FlowDirection = FlowDirection.RightToLeft;
    }

    private void Picker_Unfocused(object? sender, FocusEventArgs e)
    {
        if (sender is Picker picker && Mandatory)
        {
            ContentState = picker.SelectedItem == null
                ? ContentStates.Error : ContentStates.Normal;
        }
    }
    private void Picker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (sender is Picker picker && Mandatory)
        {
            ContentState = picker.SelectedItem == null
                ? ContentStates.Error : ContentStates.Normal;
        }
    }
}
