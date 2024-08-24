namespace BluDay.FluentNoiseRemover.Windows;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private DisplayArea _displayArea;

    private readonly AppWindow _appWindow;

    private readonly OverlappedPresenter _overlappedPresenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        _appWindow = AppWindow;

        _displayArea = DisplayArea.GetFromWindowId(_appWindow.Id, DisplayAreaFallback.Primary);

        _overlappedPresenter = (OverlappedPresenter)_appWindow.Presenter;

        InitializeComponent();

        Setup();

        AppTitleBar.SetWindow(this);
    }

    private void Setup()
    {
        _overlappedPresenter.IsMaximizable = false;
        _overlappedPresenter.IsMinimizable = false;
        _overlappedPresenter.IsResizable   = false;

        _overlappedPresenter.SetBorderAndTitleBar(true, false);

        _appWindow.ResizeClient(new SizeInt32(380, 380));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new SettingsWindow().Activate();
    }
}