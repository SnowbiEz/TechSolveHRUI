using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ModernWpf;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;
using TechSolveHR.ModernWPF;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;
using Transition = TechSolveHR.ModernWPF.Animation.Transition;
using TransitionCollection = TechSolveHR.ModernWPF.Animation.Transitions.TransitionCollection;

namespace TechSolveHR.ViewModels;

public class SettingsPageViewModel : Screen
{
    private readonly IEventAggregator _events;
    private readonly IContainer _ioc;
    private readonly IThemeService _theme;
    private readonly MainWindowViewModel _main;

    public SettingsPageViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc    = ioc;
        _events = ioc.Get<IEventAggregator>();
        _theme  = ioc.Get<IThemeService>();
        _main   = main;
    }

    public bool CanChangeCredentials => _main.IsLoggedIn;

    public static CaptionedObject<Transition>? Transition { get; set; } =
        TransitionCollection.Transitions[0];

    public string Password { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public static Version ProgramVersion => Assembly.GetExecutingAssembly().GetName().Version!;

    public async Task Save()
    {
        if (_main.LoggedInUser is null) return;

        await using var db = _ioc.Get<DatabaseContext>();
        var snackBar = _ioc.Get<ISnackbarService>();

        if (string.IsNullOrWhiteSpace(Username))
        {
            await snackBar.ShowAsync("Error!", "Please provide a username.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
        {
            await snackBar.ShowAsync("Error!", "Password is too weak.");
            return;
        }

        if (db.Employees.Any(x => x.Username == Username))
        {
            await snackBar.ShowAsync("Error!", "Username already exists");
            return;
        }

        _main.LoggedInUser.Username = Username;
        _main.LoggedInUser.Password = Password;

        db.Update(_main.LoggedInUser);
        await db.SaveChangesAsync();

        await snackBar.ShowAsync("Success", "Successfully saved changes to the database.",
            SymbolRegular.PeopleCheckmark20, ControlAppearance.Success);
    }

    public void Clear()
    {
        Username = string.Empty;
        Password = string.Empty;
    }

    public void OnThemeChanged() => _theme.SetTheme(ThemeManager.Current.ApplicationTheme switch
    {
        ApplicationTheme.Light => ThemeType.Light,
        ApplicationTheme.Dark  => ThemeType.Dark,
        _                      => _theme.GetSystemTheme()
    });

    protected override void OnActivate() => Username = _main.LoggedInUser?.Username ?? string.Empty;
}