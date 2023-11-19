using Stylet;
using StyletIoC;

namespace TechSolveHR.ViewModels;

public class RegistrationViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public RegistrationViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc = ioc;
        _main = main;
    }
}