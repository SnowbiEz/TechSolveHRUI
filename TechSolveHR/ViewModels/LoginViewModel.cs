using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;

namespace TechSolveHR.ViewModels;

public class LoginViewModel : Screen
{
    private readonly IEventAggregator _events;
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public LoginViewModel(IContainer ioc, IEventAggregator events, MainWindowViewModel main)
    {
        _ioc    = ioc;
        _events = events;
        _main   = main;
    }

    public bool LoginError { get; set; }

    public string Password { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public async Task Login()
    {
        LoginError = true;

        var db = _ioc.Get<DatabaseContext>();
        _events.Publish(new LoggedInEvent(db.Employees.First()));

        return;
        if (string.IsNullOrWhiteSpace(Username)) return;
        if (string.IsNullOrWhiteSpace(Password)) return;

        var employee = await db.Employees.FirstOrDefaultAsync(x => x.Username == Username);
        if (employee == null) return;

        if (Crypto.VerifyHashedPassword(employee.Password, Password))
        {
            LoginError = false;
            _events.Publish(new LoggedInEvent(employee));
        }
    }
}