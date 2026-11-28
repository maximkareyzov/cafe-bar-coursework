using System.Windows;
using CafeBar.Modules.Inventory.Data;
using CafeBar.Modules.Inventory.Logic;

namespace CafeBar.Modules.Inventory.UI;

public partial class SupplierWindow : Window
{
    private readonly SupplierService _service = new();
    private readonly int? _supplierId;

    public SupplierWindow(Suppliers? supplier)
    {
        InitializeComponent();
        if (supplier == null)
        {
            HeaderText.Text = "Новый поставщик";
            return;
        }

        HeaderText.Text = "Поставщик";
        _supplierId = supplier.SupplierID;
        NameBox.Text = supplier.Name;
        InnBox.Text = supplier.INN;
        ContactBox.Text = supplier.ContactInfo;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (_service.TrySave(_supplierId, NameBox.Text, InnBox.Text, ContactBox.Text, out string error))
        {
            DialogResult = true;
            return;
        }

        ErrorText.Text = error;
        ErrorText.Visibility = Visibility.Visible;
    }
}