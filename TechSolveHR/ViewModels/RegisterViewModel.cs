using Stylet;
using StyletIoC;

namespace TechSolveHR.ViewModels;

internal class RegisterViewModel : Screen   
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public RegisterViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _main = main;
    }
}