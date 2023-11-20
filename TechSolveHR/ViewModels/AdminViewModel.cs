using Stylet;
using StyletIoC;
using TechSolveHR.Models;

namespace TechSolveHR.ViewModels;

public class AdminViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly DatabaseContext _database;
    private readonly MainWindowViewModel _main;

    public AdminViewModel(IContainer ioc, DatabaseContext database, MainWindowViewModel main)
    {
        _ioc      = ioc;
        _database = database;
        _main     = main;
    }
}