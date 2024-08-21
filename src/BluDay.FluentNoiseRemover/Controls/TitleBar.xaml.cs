namespace BluDay.FluentNoiseRemover.Controls;

/// <summary>
/// Interaction logic for TitleBar.xaml.
/// </summary>
public sealed partial class TitleBar : UserControl
{
    private InputNonClientPointerSource _nonClientPointerSource;

    private Action _closeAction;

    /// <summary>
    /// Initializes a new instance of the <see cref="TitleBar"/> class.
    /// </summary>
    public TitleBar()
    {
        _nonClientPointerSource = null!;

        _closeAction = null!;

        InitializeComponent();
    }

    private void UpdateDragRegionRectangles()
    {
        _nonClientPointerSource.SetRegionRects(
            NonClientRegionKind.Caption,
            [GetControlBounds(this)]
        );
    }

    private void UpdateInteractiveRegionRectangles()
    {
        Collection<RectInt32> passthroughBounds = [GetControlBounds(CloseButton)];

        if (BackButton.Visibility is Visibility.Visible)
        {
            passthroughBounds.Add(GetControlBounds(BackButton));
        }

        _nonClientPointerSource.SetRegionRects(
            NonClientRegionKind.Passthrough,
            passthroughBounds.ToArray()
        );
    }

    private static RectInt32 GetControlBounds(FrameworkElement element)
    {
        /* Setting the `visual` parameter to `null` will automatically use the root element of `XamlRoot`.
         * 
         * Visit https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.uielement.transformtovisual for
         * more information.
         */
        Rect bounds = element
            .TransformToVisual(null)
            .TransformBounds(LayoutInformation.GetLayoutSlot(element));

        double scale = element.XamlRoot.RasterizationScale;

        return new RectInt32(
            (int)bounds.X,
            (int)bounds.Y,
            (int)(bounds.Width  * scale),
            (int)(bounds.Height * scale)
        );
    }

    /// <summary>
    /// Sets the targeted window.
    /// </summary>
    /// <param name="window">
    /// The targeted window instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="window"/> is null.
    /// </exception>
    public void SetWindow(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        _nonClientPointerSource = InputNonClientPointerSource.GetForWindowId(window.AppWindow.Id);

        _closeAction = window.Close;

        window.ExtendsContentIntoTitleBar = true;

        window.SetTitleBar(this);
    }
}

/// <summary>
/// Contains all XAML-defined event handlers.
/// </summary>
public sealed partial class TitleBar
{
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        _closeAction();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        _nonClientPointerSource!.ClearAllRegionRects();

        UpdateDragRegionRectangles();
        UpdateInteractiveRegionRectangles();
    }

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (!IsLoaded) return;

        _nonClientPointerSource!.ClearAllRegionRects();

        UpdateDragRegionRectangles();
        UpdateInteractiveRegionRectangles();
    }
}

/// <summary>
/// Contains all dependency properties for the control.
/// </summary>
public sealed partial class TitleBar
{
    
}