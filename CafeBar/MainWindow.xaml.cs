using System.Windows;
using CafeBar.ViewModels;

namespace CafeBar;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    /// <summary>Нажата кнопка «Выйти»: окно нужно закрыть и снова показать окно входа.</summary>
    public event EventHandler? SignOutRequested;

    private void SignOut_Click(object sender, RoutedEventArgs e)
    {
        SignOutRequested?.Invoke(this, EventArgs.Empty);
    }
}
