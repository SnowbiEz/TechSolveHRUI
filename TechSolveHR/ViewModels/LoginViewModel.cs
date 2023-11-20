using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;
using Wpf.Ui.Mvvm.Contracts;

namespace TechSolveHR.ViewModels;

public class LoginViewModel : Screen
{
    private readonly IEventAggregator _events;
    private readonly IContainer _ioc;
    private readonly ISnackbarService _snackbar;
    private readonly MainWindowViewModel _main;

    public LoginViewModel(IEventAggregator events, IContainer ioc, ISnackbarService snackbar, MainWindowViewModel main)
    {
        _events   = events;
        _ioc      = ioc;
        _snackbar = snackbar;
        _main     = main;
    }

    public bool LoginError { get; set; }

    public string Password { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public async Task Login()
    {
        LoginError = true;
        NotifyOfPropertyChange(() => LoginError);

        if (string.IsNullOrEmpty(Password)) return;
        if (string.IsNullOrEmpty(Username)) return;

        var db = _ioc.Get<DatabaseContext>();

        var employee = await db.Employees.FirstOrDefaultAsync(x => x.Username == Username);
        if (employee == null) return;

        if (Crypto.VerifyHashedPassword(employee.Password, Password))
        {
            LoginError = false;
            _main.Login(employee);
        }
    }

    protected override void OnActivate()
    {
        Username = string.Empty;
        Password = string.Empty;

        _main.Logout();
    }
}