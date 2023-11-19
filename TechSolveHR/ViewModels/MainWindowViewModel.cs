using System;
using System.Linq;
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

public class MainWindowViewModel : Conductor<IScreen>,
    IHandle<LoggedInEvent>, IHandle<LoggedOutEvent>
{
    public static NavigationStore Navigation = null!;
    private readonly IEventAggregator _events;
    private readonly IContainer _ioc;
    private readonly IThemeService _theme;

    public MainWindowViewModel(IContainer ioc, IEventAggregator events, IThemeService theme)
    {
        Title = $"Tech Solve HR System {SettingsPageViewModel.ProgramVersion}";

        _ioc    = ioc;
        _events = events;
        _theme  = theme;

        events.Subscribe(this);

        var db = ioc.Get<DatabaseContext>();

        if (!db.Employees.Any())
        {
            var employee1 = new Employee
            {
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
                        StreetAddress = "1234 Main St",
                        City          = "Columbus",
                        State         = "OH",
                        ZipCode       = "43215"
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
                        StreetAddress = "5678 Elm St",
                        City          = "Columbus",
                        State         = "OH",
                        ZipCode       = "43215"
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

        SettingsPage     = new SettingsPageViewModel(ioc, this);
        LoginPage        = new LoginViewModel(ioc, events, this);
        HomePage         = new HomeViewModel(ioc, this);
        PersonalInfoPage = new PersonalInfoViewModel(ioc, this);
        EmployeeListPage = new EmployeeListViewModel(ioc, this);
        RegistrationPage = new RegistrationViewModel(ioc, this);
    }

    public bool IsLoggedIn => true || LoggedInUser is not null;

    public Employee? LoggedInUser { get; set; }

    public EmployeeListViewModel EmployeeListPage { get; }

    public HomeViewModel HomePage { get; }

    public LoginViewModel LoginPage { get; }

    public PersonalInfoViewModel PersonalInfoPage { get; }

    public RegistrationViewModel RegistrationPage { get; }

    public Screen FirstPage => LoginPage;

    public SettingsPageViewModel SettingsPage { get; }

    public string Title { get; set; }

    public void Handle(LoggedInEvent message)
    {
        LoggedInUser = message.Employee;
        NavigateToItem(HomePage);
    }

    public void Handle(LoggedOutEvent message) => throw new NotImplementedException();

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

        NavigateToItem(FirstPage);
    }

    private void NavigateToItem(IScreen viewModel)
    {
        var navigationItem
            = (NavigationItem) Navigation.Items.First(x => x is NavigationItem item && item.Tag == viewModel);
        Navigation.SelectedPageIndex = Navigation.Items.IndexOf(navigationItem);
        Navigation.Navigate(navigationItem.PageType);
    }
}

public record LoggedOutEvent;

public record LoggedInEvent(Employee Employee);