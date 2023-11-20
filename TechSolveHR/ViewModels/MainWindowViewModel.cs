using System;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using ModernWpf;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;
using TechSolveHR.Views;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace TechSolveHR.ViewModels;

public class MainWindowViewModel : Conductor<IScreen>
{
    public static MainWindowView MainView = null!;
    public static NavigationStore Navigation = null!;
    private readonly IDialogService _dialog;
    private readonly IEventAggregator _events;
    private readonly IContainer _ioc;
    private readonly ISnackbarService _snackbar;
    private readonly IThemeService _theme;
    private Attendance? _attendance;

    public MainWindowViewModel(IStyletIoCBuilder builder)
    {
        Title = $"Tech Solve HR System {SettingsPageViewModel.ProgramVersion}";

        builder.Bind<MainWindowViewModel>().ToInstance(this);
        _ioc      = builder.BuildContainer();
        _events   = _ioc.Get<IEventAggregator>();
        _theme    = _ioc.Get<IThemeService>();
        _snackbar = _ioc.Get<ISnackbarService>();
        _dialog   = _ioc.Get<IDialogService>();

        HomePage         = _ioc.Get<HomeViewModel>();
        SettingsPage     = _ioc.Get<SettingsPageViewModel>();
        LoginPage        = _ioc.Get<LoginViewModel>();
        AdminPage        = _ioc.Get<AdminViewModel>();
        PersonalInfoPage = _ioc.Get<PersonalInfoViewModel>();
        EmployeeListPage = _ioc.Get<EmployeeListViewModel>();
        RegistrationPage = _ioc.Get<RegistrationViewModel>();
        AttendancePage   = _ioc.Get<AttendanceViewModel>();
        PerformancePage  = _ioc.Get<PerformanceViewModel>();
    }

    public AdminViewModel AdminPage { get; }

    public AttendanceViewModel AttendancePage { get; }

    public bool IsAdmin => LoggedInUser is { AccessType: "Admin" };

    public bool IsLoggedIn => LoggedInUser is not null;

    public bool IsLoggedOut => LoggedInUser is null;

    public Employee? LoggedInUser { get; set; }

    public EmployeeListViewModel EmployeeListPage { get; }

    public HomeViewModel HomePage { get; }

    public LoginViewModel LoginPage { get; }

    public PerformanceViewModel PerformancePage { get; }

    public PersonalInfoViewModel PersonalInfoPage { get; }

    public RegistrationViewModel RegistrationPage { get; }

    public Screen FirstPage => LoginPage;

    public SettingsPageViewModel SettingsPage { get; }

    public string LogText => IsLoggedIn ? "Logout" : "Login";

    public string Title { get; set; }

    public SymbolRegular LogIcon => IsLoggedIn ? SymbolRegular.DoorArrowLeft20 : SymbolRegular.Person12;

    public async Task Login(Employee employee)
    {
        LoggedInUser = employee;

        _attendance = new Attendance
        {
            TimeIn = DateTimeOffset.UtcNow
        };

        NavigateToItem(HomePage);
        await _snackbar.ShowAsync("Welcome!", $"You have successfully logged in as {LoggedInUser.Data.Name}.",
            SymbolRegular.WeatherSunny20, ControlAppearance.Primary);
    }

    public async Task Logout()
    {
        if (LoggedInUser is null || _attendance is null) return;

        _attendance.TimeOut = DateTimeOffset.UtcNow;

        await using var db = _ioc.Get<DatabaseContext>();
        var user = await db.Employees.FindAsync(LoggedInUser.Id);
        if (user is not null)
        {
            user.Attendances.Add(_attendance);
            await db.SaveChangesAsync();
        }

        await _snackbar.ShowAsync("Goodbye",
            $"You have logged out. Today you worked for {_attendance.Length.Humanize()}.",
            SymbolRegular.WeatherMoon20, ControlAppearance.Secondary);

        LoggedInUser = null;
    }

    public void Navigate(INavigation sender, RoutedNavigationEventArgs args)
    {
        if (args.CurrentPage is NavigationItem { Tag: IScreen viewModel })
            ActivateItem(viewModel);
    }

    public void NavigateToItem(IScreen view)
    {
        if (view == SettingsPage)
        {
            NavigateToSettings();
            return;
        }

        var navigationItem = Navigation.Items.OfType<NavigationItem>().First(x => x.Tag == view);
        Navigation.SelectedPageIndex = Navigation.Items.IndexOf(navigationItem);
        Navigation.Navigate(navigationItem.PageType);
    }

    public void NavigateToSettings()
    {
        ActivateItem(SettingsPage);
        Navigation.Navigate(typeof(SettingsPageView));
    }

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
        MainView   = (MainWindowView) View;
        Navigation = MainView.RootNavigation;
        SettingsPage.OnThemeChanged();

        NavigateToItem(FirstPage);

        MainView.RootSnackBar.Timeout = (int) TimeSpan.FromSeconds(3).TotalMilliseconds;

        _snackbar.SetSnackbarControl(MainView.RootSnackBar);
        _dialog.SetDialogControl(MainView.RootContentDialog);

        await _snackbar.ShowAsync("Welcome!", "Welcome to Tech Solve HR System",
            SymbolRegular.RibbonStar24, ControlAppearance.Primary);
    }
}