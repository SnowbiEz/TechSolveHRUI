using System;
using System.Threading.Tasks;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;

namespace TechSolveHR.ViewModels;

public class PersonalInfoViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public PersonalInfoViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _main = main;
    }

    public DateTime DateOfBirth
    {
        get => Employee.Data.DateOfBirth?.DateTime ?? DateTime.Now;
        set => Employee.Data.DateOfBirth = value;
    }

    public Employee Employee => _main.LoggedInUser!;


    public string Age => $"{DateTime.Now.Year - Employee.Data.DateOfBirth?.Year}";

    public async Task Save()
    {
        var db = _ioc.Get<DatabaseContext>();
        db.Employees.Update(Employee);
        await db.SaveChangesAsync();
    }
}