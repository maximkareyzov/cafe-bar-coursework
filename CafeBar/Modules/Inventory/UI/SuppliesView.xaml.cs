using System.Windows;
using System.Windows.Controls;

namespace CafeBar.Modules.Inventory.UI;

public partial class SuppliesView : UserControl
{
    public SuppliesView()
    {
        InitializeComponent();
    }

    private SuppliesViewModel ViewModel => (SuppliesViewModel)DataContext;

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        var window = new SupplyWindow { Owner = Window.GetWindow(this), ShowInTaskbar = false };
        if (window.ShowDialog() == true) ViewModel.OnPosted();
    }
}