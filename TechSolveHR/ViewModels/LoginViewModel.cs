using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;

namespace TechSolveHR.ViewModels;

public class LoginViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public LoginViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _main = main;
    }

    public Employee? LoggedInUser { get; set; }

    public bool IsLoggedIn => true;

    public string Password { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public void Login()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            return;
        }

        if (VerifyPasswordAsync(Username, Password).Result)
        {
            LoggedInUser = _ioc.Get<DatabaseContext>().Users.FirstOrDefaultAsync(x => x.Username == Username).Result;
            _main.NavigateToSettings();
        }
    }

    public async Task<bool> VerifyPasswordAsync(string username, string password)
    {
        var user = await _ioc.Get<DatabaseContext>().Users.FirstOrDefaultAsync(x => x.Username == username);
        return user is not null && Crypto.VerifyHashedPassword(user.Password, password);
    }
}