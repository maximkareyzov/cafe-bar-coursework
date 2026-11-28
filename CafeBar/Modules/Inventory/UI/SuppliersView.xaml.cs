using System.Windows;
using System.Windows.Controls;
using CafeBar.Modules.Inventory.Data;

namespace CafeBar.Modules.Inventory.UI;

public partial class SuppliersView : UserControl
{
    public SuppliersView()
    {
        InitializeComponent();
    }

    private SuppliersViewModel ViewModel => (SuppliersViewModel)DataContext;

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        OpenWindow(null);
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.Selected != null) OpenWindow(ViewModel.Selected);
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        var supplier = ViewModel.Selected;
        if (supplier == null) return;

        var answer = MessageBox.Show($"Удалить поставщика «{supplier.Name}»?", "Склад",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer != MessageBoxResult.Yes) return;

        if (!ViewModel.TryDeleteSelected(out string error))
        {
            MessageBox.Show(error, "Склад", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void OpenWindow(Suppliers? supplier)
    {
        var window = new SupplierWindow(supplier) { Owner = Window.GetWindow(this), ShowInTaskbar = false };
        if (window.ShowDialog() == true) ViewModel.Reload();
    }
}