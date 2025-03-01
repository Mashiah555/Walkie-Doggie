using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace Walkie_Doggie.PacksKit;

public class RadioCollectionPack : StackLayout
{
    #region Bindable Properties
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(List<string>), typeof(RadioCollectionPack), new List<string>());

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(RadioCollectionPack), null, BindingMode.TwoWay,
            propertyChanged: OnSelectedItemChanged);

    public static readonly BindableProperty SelectionModeProperty =
        BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(RadioCollectionPack), SelectionMode.Single);

    public static readonly BindableProperty GroupNameProperty =
        BindableProperty.Create(nameof(GroupName), typeof(string), typeof(RadioCollectionPack), default(string));

    public static readonly BindableProperty DefaultItemProperty =
        BindableProperty.Create(nameof(DefaultItem), typeof(string), typeof(RadioCollectionPack), default(string));

    public static readonly BindableProperty FloatingLabelProperty =
        BindableProperty.Create(nameof(FloatingLabel), typeof(string), typeof(EntryPack), string.Empty);

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(EntryPack), string.Empty);

    public static readonly BindableProperty ContentStateProperty =
    BindableProperty.Create(nameof(ContentState), typeof(ContentStates), typeof(EntryPack), ContentStates.Normal);

    #endregion Bindable Properties

    #region Properties
    public List<string> ItemsSource
    {
        get => (List<string>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public string? SelectedItem
    {
        get => (string?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public SelectionMode SelectionMode
    {
        get => (SelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }

    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    public string DefaultItem
    {
        get => (string)GetValue(DefaultItemProperty);
        set => SetValue(DefaultItemProperty, value);
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

    public RadioCollectionPack()
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

        #region CollectionView Initialization
        collectionView = new CollectionView
        {
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
            EmptyView = "עדיין לא קיים במערכת אף משתמש",
            Margin = new Thickness(8),
            ItemTemplate = new DataTemplate(() =>
            {
                RadioPack radioPack = new RadioPack
                {
                    GroupName = GroupName,
                    Margin = new Thickness(3)
                };
                radioPack.SetBinding(RadioPack.TextProperty, ".");

                // Store a reference to each RadioPack for later use
                radioPack.BindingContextChanged += (sender, e) =>
                {
                    if (sender is RadioPack pack && pack.BindingContext is string text)
                    {
                        if (!radioPackLookup.ContainsKey(text))
                            radioPackLookup[text] = pack;

                        // Set IsChecked based on whether this item is the SelectedItem
                        pack.IsChecked = text == SelectedItem;
                    }
                };

                // Handle IsChecked changes to update SelectedItem
                radioPack.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(RadioPack.IsChecked) && sender is RadioPack pack)
                    {
                        if (pack.IsChecked && pack.BindingContext is string text)
                        {
                            SelectedItem = text;
                        }
                    }
                };

                return radioPack;
            })
        };

        collectionView.SetBinding(
            CollectionView.ItemsSourceProperty, new Binding(nameof(ItemsSource), source: this));
        collectionView.SetBinding(
            CollectionView.SelectedItemProperty, new Binding(nameof(SelectedItem), source: this));
        collectionView.SetBinding(
            CollectionView.SelectionModeProperty, new Binding(nameof(SelectionMode), source: this));

        // Handle when CollectionView selection changes
        collectionView.SelectionChanged += (sender, e) =>
        {
            if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is string selectedItem)
            {
                SelectedItem = selectedItem;
            }
        };
        #endregion CollectionView Initialization

        #region Border Initialization
        var border = new Border
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

        //Initialize the EntryPack's StackLayout with components:
        Children.Add(floatingLabel);
        Children.Add(border);
        Children.Add(descriptionLabel);
        FlowDirection = FlowDirection.RightToLeft;

        // Set the DefaultItem if specified
        if (!string.IsNullOrEmpty(DefaultItem))
        {
            SelectedItem = DefaultItem;
        }
    }

    // Handle when the SelectedItem property changes
    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RadioCollectionPack radioCollection)
        {
            radioCollection.UpdateRadioPackSelection();
        }
    }

    // Update all RadioPack controls to reflect the current selection
    private void UpdateRadioPackSelection()
    {
        foreach (var entry in radioPackLookup)
        {
            string itemText = entry.Key;
            RadioPack radioPack = entry.Value;

            radioPack.IsChecked = itemText == SelectedItem;
        }
    }
}