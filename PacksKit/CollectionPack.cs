using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Xml;

namespace Walkie_Doggie.PacksKit;

public class CollectionPack : StackLayout
{
    #region Bindable Properties
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(List<ItemPack>), typeof(CollectionPack), new List<ItemPack>());

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(ItemPack), typeof(CollectionPack), null, BindingMode.TwoWay);

    public static readonly BindableProperty SelectionModeProperty =
        BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(CollectionPack), SelectionMode.Single);

    public static readonly BindableProperty ColumnsProperty =
        BindableProperty.Create(nameof(Columns), typeof(int), typeof(CollectionPack), 2,
            propertyChanged: OnColumnsChanged);

    public static readonly BindableProperty EmptyContentProperty =
        BindableProperty.Create(nameof(EmptyContent), typeof(string), typeof(RadioCollectionPack), string.Empty);

    public static readonly BindableProperty FloatingLabelProperty =
        BindableProperty.Create(nameof(FloatingLabel), typeof(string), typeof(CollectionPack), string.Empty);

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(CollectionPack), string.Empty);

    public static readonly BindableProperty ContentStateProperty =
    BindableProperty.Create(nameof(ContentState), typeof(ContentStates), typeof(CollectionPack), ContentStates.Normal);

    #endregion Bindable Properties

    #region Properties
    public List<ItemPack> ItemsSource
    {
        get => (List<ItemPack>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public ItemPack? SelectedItem
    {
        get => (ItemPack?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public SelectionMode SelectionMode
    {
        get => (SelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }

    public int Columns
    {
        get => (int)GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public string EmptyContent
    {
        get => (string)GetValue(EmptyContentProperty);
        set => SetValue(EmptyContentProperty, value);
    }

    public string FloatingLabel
    {
        get => (string)GetValue(FloatingLabelProperty);
        set => SetValue(FloatingLabelProperty, value);
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

    private CollectionView collectionView;
    private Dictionary<string, RadioPack> radioPackLookup = new Dictionary<string, RadioPack>();

    public CollectionPack()
    {
        #region Floating Label Initialization
        Label floatingLabel = new Label
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

        #region CollectionView Initialization
        collectionView = new CollectionView
        {
            ItemsLayout = new GridItemsLayout(Columns, ItemsLayoutOrientation.Vertical),
            Margin = new Thickness(8),
            ItemTemplate = new DataTemplate(() =>
            {
                #region Image Initialization
                var image = new Image
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(1, 3, -8, 3)
                };
                image.SetBinding(Image.SourceProperty, nameof(ImageSource));
                image.SetBinding(Image.IsVisibleProperty, nameof(ImageSource), converter: new ConvertToBool());
                #endregion Image Initialization

                #region Main Label Initialization
                // Create the MainText Label
                var mainLabel = new Label
                {
                    Style = (Style)Application.Current.Resources["SubHeader"],
                    VerticalOptions = LayoutOptions.End
                };
                mainLabel.SetBinding(Label.TextProperty, "MainText");
                #endregion Main Label Initialization

                #region Secondary Label Initialization
                var secondaryLabel = new Label
                {
                    Style = (Style)Application.Current.Resources["Context"]
                };
                secondaryLabel.SetBinding(Label.TextProperty, "SecondaryText");
                #endregion Secondary Label Initialization

                #region Context Label Initialization
                var contextLabel = new Label
                {
                    Style = (Style)Application.Current.Resources["Context"],
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(-9, 0, 0, 0)
                };
                contextLabel.SetBinding(Label.TextProperty, "ContextText");
                #endregion Context Label Initialization

                #region Grid Initialization
                var grid = new Grid
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                    ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(60) },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                }
                };

                // Add all elements to the Grid
                grid.Add(image);
                grid.Add(mainLabel);
                grid.Add(secondaryLabel);
                grid.Add(contextLabel);

                Grid.SetRow(image, 0);
                Grid.SetColumn(image, 0);
                Grid.SetRowSpan(image, 2);

                Grid.SetRow(mainLabel, 0);
                Grid.SetColumn(mainLabel, 1);
                Grid.SetColumnSpan(mainLabel, 2);

                Grid.SetRow(secondaryLabel, 1);
                Grid.SetColumn(secondaryLabel, 1);

                Grid.SetRow(contextLabel, 1);
                Grid.SetColumn(contextLabel, 2);
                #endregion Grid Initialization

                return new Frame
                {
                    Content = grid,
                    Style = (Style)Application.Current.Resources["SelectableFrame"],
                    BorderColor = Colors.Transparent,
                    InputTransparent = true
                };
            })
        };

        collectionView.SetBinding(
            CollectionView.ItemsSourceProperty, new Binding(nameof(ItemsSource), source: this));
        collectionView.SetBinding(
            CollectionView.SelectedItemProperty, new Binding(nameof(SelectedItem), source: this));
        collectionView.SetBinding(
            CollectionView.SelectionModeProperty, new Binding(nameof(SelectionMode), source: this));
        collectionView.SetBinding(
            CollectionView.EmptyViewProperty, new Binding(nameof(EmptyContent), source: this));

        // Handle when CollectionView selection changes
        collectionView.SelectionChanged += (sender, e) =>
        {
            if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is ItemPack selectedItem)
            {
                SelectedItem = selectedItem;
            }
        };
        #endregion CollectionView Initialization

        #region Border Initialization
        Border border = new Border
        {
            Content = collectionView,
            BackgroundColor = Colors.Transparent,
            Margin = new Thickness(5, 2),
            Padding = new Thickness(2),
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

        if (string.IsNullOrEmpty(FloatingLabel))
            border.StrokeThickness = 0;
        #endregion Border Initialization

        #region Description Label Initialization
        var descriptionLabel = new Label
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

        //Initialize the CollectionPack's StackLayout with components:
        Children.Add(floatingLabel);
        Children.Add(border);
        Children.Add(descriptionLabel);
        FlowDirection = FlowDirection.RightToLeft;
    }

    // Handle when the ColumnCount property changes
    private static void OnColumnsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CollectionPack pack &&
            pack.collectionView != null &&
            newValue is int columns)
        {
            // Update the ItemsLayout with the new column count
            pack.collectionView.ItemsLayout = new GridItemsLayout(columns, ItemsLayoutOrientation.Vertical);
        }
    }
}

public class ItemPack
{
    public required string MainText { get; set; }
    public string? SecondaryText { get; set; }
    public string? ContextText { get; set; }
    public ImageSource? ImageSource { get; set; }
}