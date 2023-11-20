using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
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

        var db = _ioc.Get<DatabaseContext>();

        if (!db.Employees.Any())
        {
            var employee1 = new Employee
            {
                AccessType = "Admin",
                Username  = "charlie",
                Password  = Crypto.HashPassword("password"),
                CompanyId = "1",
                Status    = "Active",
                Department = new Department
                {
                    Name = "Human Resources"
                },
                Division = new Division
                {
                    Name = "Tech Solve"
                },
                Title = "HR Manager",
                Data = new PersonalInformation
                {
                    FirstName     = "Charlotte",
                    MiddleName    = "Rose",
                    LastName      = "Doyle",
                    PreferredName = "Charlie",
                    Gender        = "Female",
                    DateOfBirth   = new DateTime(1999, 1, 1),
                    DateOfHire    = new DateTime(2021, 1, 1),
                    Address = new Address
                    {
                        Street  = "1234 Main St",
                        City    = "Columbus",
                        State   = "OH",
                        ZipCode = "43215"
                    },
                    MaritalStatus   = "Single",
                    PhilHealth      = "123456789012",
                    Sss             = "12345678901234567",
                    PhoneNumber     = "1234567890",
                    TelephoneNumber = "1234567890",
                    Tin             = "1234567890"
                }
            };

            var employee2 = new Employee
            {
                Username  = "manager",
                Password  = Crypto.HashPassword("password"),
                Manager   = employee1,
                CompanyId = "2",
                Status    = "Active",
                Department = new Department
                {
                    Name = "Human Resources"
                },
                Division = new Division
                {
                    Name = "Tech Solve"
                },
                Title = "Manager",
                Data = new PersonalInformation
                {
                    FirstName     = "John",
                    MiddleName    = "Doe",
                    LastName      = "Smith",
                    PreferredName = "John",
                    Gender        = "Male",
                    DateOfBirth   = new DateTime(1985, 5, 10),
                    DateOfHire    = DateTime.Now,
                    Address = new Address
                    {
                        Street  = "5678 Elm St",
                        City    = "Columbus",
                        State   = "OH",
                        ZipCode = "43215"
                    },
                    MaritalStatus   = "Single",
                    PhilHealth      = "987654321098",
                    Sss             = "98765432109876543",
                    PhoneNumber     = "0987654321",
                    TelephoneNumber = "0987654321",
                    Tin             = "0987654321"
                }
            };

            db.Employees.Add(employee1);
            db.Employees.Add(employee2);
            db.SaveChanges();
        }

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

    public LoginViewModel LoginPage { get; }

    public PerformanceViewModel PerformancePage { get; }

    public PersonalInfoViewModel PersonalInfoPage { get; }

    public RegistrationViewModel RegistrationPage { get; }

    public Screen FirstPage => LoginPage;

    public SettingsPageViewModel SettingsPage { get; }

    public string LogText => IsLoggedIn ? "Logout" : "Login";

    public string Title { get; set; }

    public SymbolRegular LogIcon => IsLoggedIn ? SymbolRegular.DoorArrowLeft20 : SymbolRegular.Person12;

    public void Login(Employee employee)
    {
        LoggedInUser = employee;

        employee.Attendances.Add(_attendance = new Attendance
        {
            TimeIn = DateTimeOffset.UtcNow
        });

        
        NavigateToItem(PersonalInfoPage);
    }

    public async Task Logout()
    {
        if (LoggedInUser is null || _attendance is null) return;

        _attendance.TimeOut = DateTimeOffset.UtcNow;

        var db = _ioc.Get<DatabaseContext>();
        db.Employees.Update(LoggedInUser);

        await db.SaveChangesAsync();

        LoggedInUser = null;
    }

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
        MainView   = (MainWindowView) View;
        Navigation = MainView.RootNavigation;
        SettingsPage.OnThemeChanged();

        NavigateToItem(FirstPage);

        MainView.RootSnackBar.Timeout = (int) TimeSpan.FromSeconds(2).TotalMilliseconds;
        _snackbar.SetSnackbarControl(MainView.RootSnackBar);
        _dialog.SetDialogControl(MainView.RootContentDialog);

        await _snackbar.ShowAsync("Welcome!", "Welcome to Tech Solve HR System",
            SymbolRegular.RibbonStar24, ControlAppearance.Primary);
    }

    private void NavigateToItem(IScreen viewModel)
    {
        var navigationItem
            = (NavigationItem) Navigation.Items.First(x => x is NavigationItem item && item.Tag == viewModel);
        Navigation.SelectedPageIndex = Navigation.Items.IndexOf(navigationItem);
        Navigation.Navigate(navigationItem.PageType);
    }
}