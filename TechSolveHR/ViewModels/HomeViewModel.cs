using Stylet;

namespace TechSolveHR.ViewModels;

public class HomeViewModel : Screen
{
    public HomeViewModel(MainWindowViewModel main) { Main = main; }

    public MainWindowViewModel Main { get; }

    public void Navigate(Screen screen)
    {
        if (screen != Main.LoginPage
            && screen != Main.SettingsPage
            && screen != Main.EmployeeListPage
            && Main.IsLoggedOut)
            Main.NavigateToItem(Main.LoginPage);
        else
            Main.NavigateToItem(screen);
    }
}