using System.Globalization;
using System.Windows;
using CafeBar.Modules.Inventory.Logic;

namespace CafeBar.Modules.Inventory.UI;

public partial class IngredientWindow : Window
{
    private readonly IngredientService _service = new();
    private readonly int? _ingredientId;

    public IngredientWindow(int? ingredientId)
    {
        InitializeComponent();
        UnitBox.ItemsSource = IngredientService.Units;
        _ingredientId = ingredientId;

        var ingredient = ingredientId == null ? null : _service.Get(ingredientId.Value);
        if (ingredient == null)
        {
            HeaderText.Text = "Новый ингредиент";
            UnitBox.SelectedIndex = 0;
            StockBox.Text = "0";
            ThresholdBox.Text = "0";
            return;
        }

        HeaderText.Text = "Ингредиент";
        NameBox.Text = ingredient.Name;
        UnitBox.SelectedItem = ingredient.Unit;
        StockBox.Text = IngredientService.ParseStock(ingredient.CurrentStock).ToString("0.###", CultureInfo.CurrentCulture);
        ThresholdBox.Text = ingredient.MinStockThreshold.ToString("0.###", CultureInfo.CurrentCulture);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (!TryParse(StockBox.Text, out decimal stock))
        {
            ShowError("Остаток укажите числом, например 12,5.");
            return;
        }

        if (!TryParse(ThresholdBox.Text, out decimal threshold))
        {
            ShowError("Порог укажите числом, например 2.");
            return;
        }

        string unit = UnitBox.SelectedItem as string ?? string.Empty;
        if (_service.TrySave(_ingredientId, NameBox.Text, unit, stock, threshold, out string error))
        {
            DialogResult = true;
            return;
        }

        ShowError(error);
    }

    // число принимаем и с запятой, и с точкой
    private static bool TryParse(string text, out decimal value)
    {
        return decimal.TryParse(text.Trim().Replace(',', '.'), NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out value);
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.Visibility = Visibility.Visible;
    }
}