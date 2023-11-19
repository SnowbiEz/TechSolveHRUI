using Stylet;
using StyletIoC;

namespace TechSolveHR.ViewModels;

public class HomeViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public HomeViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _main = main;
    }


}