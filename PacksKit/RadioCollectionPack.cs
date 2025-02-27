using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Walkie_Doggie.PacksKit;

public class RadioCollectionPack : StackLayout
{
    #region Bindable Properties
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(List<string>), typeof(RadioCollectionPack), new List<string>(), propertyChanged: OnItemsSourceChanged);

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(RadioCollectionPack), null, BindingMode.TwoWay);

    public static readonly BindableProperty SelectionModeProperty =
        BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(RadioCollectionPack), SelectionMode.Single);

    public static readonly BindableProperty GroupNameProperty =
        BindableProperty.Create(nameof(GroupName), typeof(string), typeof(RadioCollectionPack), default(string));

    public static readonly BindableProperty DefaultItemProperty =
        BindableProperty.Create(nameof(DefaultItem), typeof(string), typeof(RadioCollectionPack), default(string), propertyChanged: OnDefaultItemChanged);

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
        var collectionView = new CollectionView
        {
            SelectionMode = SelectionMode,
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
            ItemsSource = ItemsSource,
            ItemTemplate = new DataTemplate(() =>
            {
                var frame = new Frame { Padding = 5, Margin = new Thickness(5) };

                var stack = new StackLayout { Orientation = StackOrientation.Horizontal };

                var radioButton = new RadioButton { GroupName = GroupName };
                radioButton.CheckedChanged += (s, e) =>
                {
                    if (e.Value)
                        SelectedItem = (string)((RadioButton)s).BindingContext;
                };

                var label = new Label();
                label.SetBinding(Label.TextProperty, ".");

                stack.Children.Add(radioButton);
                stack.Children.Add(label);

                frame.Content = stack;
                frame.BindingContextChanged += (s, e) =>
                {
                    if (frame.BindingContext is string item && item == DefaultItem)
                    {
                        radioButton.IsChecked = true;
                    }
                };
                frame.SetBinding(BindableObject.BindingContextProperty, new Binding("."));

                return frame;
            })
        };
        #endregion CollectionView Initialization
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RadioCollectionPack)bindable;
        //control.UpdateItems();
    }

    private static void OnDefaultItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RadioCollectionPack)bindable;
        control.SelectDefaultItem();
    }

    private void UpdateItems()
    {
        Children.Clear();
        if (ItemsSource == null)
            return;

        var collectionView = new CollectionView
        {
            SelectionMode = SelectionMode,
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical),
            ItemsSource = ItemsSource,
            ItemTemplate = new DataTemplate(() =>
            {
                var frame = new Frame { Padding = 5, Margin = new Thickness(5) };

                var stack = new StackLayout { Orientation = StackOrientation.Horizontal };

                var radioButton = new RadioButton { GroupName = GroupName };
                radioButton.CheckedChanged += (s, e) => 
                {
                    if (e.Value) 
                        SelectedItem = (string)((RadioButton)s).BindingContext; 
                };

                var label = new Label();
                label.SetBinding(Label.TextProperty, ".");

                stack.Children.Add(radioButton);
                stack.Children.Add(label);

                frame.Content = stack;
                frame.BindingContextChanged += (s, e) =>
                {
                    if (frame.BindingContext is string item && item == DefaultItem)
                    {
                        radioButton.IsChecked = true;
                    }
                };
                frame.SetBinding(BindableObject.BindingContextProperty, new Binding("."));

                return frame;
            })
        };

        collectionView.SetBinding(SelectableItemsView.SelectedItemProperty, new Binding(nameof(SelectedItem), source: this, mode: BindingMode.TwoWay));
        Children.Add(collectionView);
    }

    private void SelectDefaultItem()
    {
        if (ItemsSource == null || string.IsNullOrEmpty(DefaultItem))
            return;
        SelectedItem = ItemsSource.Contains(DefaultItem) ? DefaultItem : null;
    }
}
