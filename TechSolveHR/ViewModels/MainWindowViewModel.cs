using System.Linq;
using ModernWpf;
using Stylet;
using StyletIoC;
using TechSolveHR.Views;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace TechSolveHR.ViewModels;

public class MainWindowViewModel : Conductor<IScreen>
{
    public static NavigationStore Navigation = null!;
    private readonly IContainer _ioc;
    private readonly IThemeService _theme;

    public MainWindowViewModel(IContainer ioc, IThemeService theme)
    {
        Title = $"Tech Solve HR System {SettingsPageViewModel.ProgramVersion}";

        _ioc   = ioc;
        _theme = theme;

        SettingsPage = new SettingsPageViewModel(ioc, this);
        LoginPage    = new LoginViewModel(ioc, this);
        HomePage     = new HomeViewModel(ioc, this);
    }

    public HomeViewModel HomePage { get; }

    public LoginViewModel LoginPage { get; }

    public Screen FirstPage => HomePage;

    public SettingsPageViewModel SettingsPage { get; }

    public string Title { get; set; }

    public void Navigate(INavigation sender, RoutedNavigationEventArgs args)
    {
        if (args.CurrentPage is NavigationItem { Tag: IScreen viewModel })
            ActivateItem(viewModel);
    }

    public void NavigateToSettings() => ActivateItem(SettingsPage);

    public void ToggleTheme()
    {
        ThemeManager.Current.ApplicationTheme = _theme.GetTheme() switch
        {
            ThemeType.Unknown      => ApplicationTheme.Dark,
            ThemeType.Dark         => ApplicationTheme.Light,
            ThemeType.Light        => ApplicationTheme.Dark,
            ThemeType.HighContrast => ApplicationTheme.Dark,
            _                      => ApplicationTheme.Dark
        };

        SettingsPage.OnThemeChanged();
    }

    protected override async void OnViewLoaded()
    {
        Navigation = ((MainWindowView) View).RootNavigation;
        SettingsPage.OnThemeChanged();

        var navigationItem = Navigation.Items.First(x => x is NavigationItem item && item.Tag == FirstPage);
        Navigation.SelectedPageIndex = Navigation.Items.IndexOf(navigationItem);
        ActivateItem(FirstPage);
    }
}