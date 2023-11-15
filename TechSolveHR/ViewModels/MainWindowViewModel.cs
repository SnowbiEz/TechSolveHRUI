using JetBrains.Annotations;
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

[UsedImplicitly]
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

        ActiveItem = SettingsView = new SettingsPageViewModel(ioc, this);
    }

    public SettingsPageViewModel SettingsView { get; }

    public string Title { get; set; }

    public void Navigate(INavigation sender, RoutedNavigationEventArgs args)
    {
        if ((args.CurrentPage as NavigationItem)?.Tag is IScreen viewModel)
            ActivateItem(viewModel);
    }

    public void NavigateToSettings() => ActivateItem(SettingsView);

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

        SettingsView.OnThemeChanged();
    }

    protected override async void OnViewLoaded()
    {
        Navigation = ((MainWindowView) View).RootNavigation;
        SettingsView.OnThemeChanged();
    }
}