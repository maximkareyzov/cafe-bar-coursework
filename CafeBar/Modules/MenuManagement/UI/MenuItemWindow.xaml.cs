using System;
using System.Globalization;
using System.Windows;
using CafeBar.Modules.MenuManagement.Data;
using CafeBar.Modules.MenuManagement.Logic;

namespace CafeBar.Modules.MenuManagement.UI;

public partial class MenuItemWindow : Window
{
    private readonly MenuService _service = new();
    private readonly int? _itemId;

    public MenuItemWindow(MenuItems? item)
    {
        InitializeComponent();
        CategoryBox.ItemsSource = _service.GetCategories();

        if (item == null)
        {
            HeaderText.Text = "Новая позиция";
            PriceBox.Text = "0";
            AvailableBox.IsChecked = true;
        }
        else
        {
            HeaderText.Text = "Позиция меню";
            _itemId = item.MenuItemID;
            CategoryBox.SelectedValue = item.CategoryID;
            NameBox.Text = item.Name;
            PriceBox.Text = item.Price.ToString("0.##", CultureInfo.CurrentCulture);
            UnitBox.Text = item.Unit;
            DescriptionBox.Text = item.Description;
            AvailableBox.IsChecked = item.IsAvailable;
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (CategoryBox.SelectedValue is not int categoryId)
        {
            ShowError("Выберите категорию. Если категорий нет, сначала создайте её на вкладке «Категории».");
            return;
        }

        string priceText = PriceBox.Text.Trim().Replace(',', '.');
        if (!decimal.TryParse(priceText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price))
        {
            ShowError("Цену укажите числом, например 250 или 99,5.");
            return;
        }

        bool saved = _service.TrySaveMenuItem(_itemId, categoryId, NameBox.Text, price, UnitBox.Text,
            DescriptionBox.Text, AvailableBox.IsChecked == true, out string error);
        if (saved) DialogResult = true;
        else ShowError(error);
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.Visibility = Visibility.Visible;
    }
}
