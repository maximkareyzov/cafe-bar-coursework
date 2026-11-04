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
}
