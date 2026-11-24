using System.Globalization;
using System.Windows;
using CafeBar.Modules.MenuManagement.Data;
using CafeBar.Modules.MenuManagement.Logic;

namespace CafeBar.Modules.MenuManagement.UI;

public partial class ModifierWindow : Window
{
    private readonly MenuService _service = new();
    private readonly int? _modifierId;

    public ModifierWindow(Modifiers? modifier)
    {
        InitializeComponent();
        ItemBox.ItemsSource = _service.GetMenuItems();

        if (modifier == null)
        {
            HeaderText.Text = "Новый модификатор";
            PriceBox.Text = "0";
        }
        else
        {
            HeaderText.Text = "Модификатор";
            _modifierId = modifier.ModifierID;
            ItemBox.SelectedValue = modifier.MenuItemID;
            GroupBox.Text = modifier.GroupName;
            NameBox.Text = modifier.Name;
            PriceBox.Text = modifier.PriceExtra.ToString("0.##", CultureInfo.CurrentCulture);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (ItemBox.SelectedValue is not int menuItemId)
        {
            ShowError("Выберите позицию меню.");
            return;
        }

        // надбавку принимаем и с запятой, и с точкой
        string priceText = PriceBox.Text.Trim().Replace(',', '.');
        if (!decimal.TryParse(priceText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price))
        {
            ShowError("Надбавку укажите числом, например 30 или 12,5.");
            return;
        }

        if (_service.TrySaveModifier(_modifierId, menuItemId, GroupBox.Text, NameBox.Text, price, out string error))
        {
            DialogResult = true;
            return;
        }

        ShowError(error);
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.Visibility = Visibility.Visible;
    }
}
