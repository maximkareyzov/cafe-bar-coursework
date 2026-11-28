using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CafeBar.Modules.Inventory.Logic;
using CafeBar.Modules.StaffManagement.Logic;

namespace CafeBar.Modules.Inventory.UI;

public partial class SupplyWindow : Window
{
    private readonly SupplyService _service = new();
    private readonly ObservableCollection<SupplyLine> _lines = new();

    public SupplyWindow()
    {
        InitializeComponent();
        SupplierBox.ItemsSource = new SupplierService().GetAll();
        IngredientBox.ItemsSource = new IngredientService().GetRows(string.Empty, false);
        DateBox.SelectedDate = DateTime.Today;
        LinesGrid.ItemsSource = _lines;
        UpdateTotal();
    }

    // в подписи количества показываем единицу выбранного ингредиента
    private void IngredientBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var row = IngredientBox.SelectedItem as IngredientRow;
        QuantityLabel.Text = row == null ? "Количество" : $"Количество ({row.Unit})";
    }

    private void AddLine_Click(object sender, RoutedEventArgs e)
    {
        if (IngredientBox.SelectedItem is not IngredientRow ingredient)
        {
            ShowError("Выберите ингредиент.");
            return;
        }

        if (!TryParse(QuantityBox.Text, out decimal quantity) || quantity <= 0)
        {
            ShowError("Количество укажите числом больше нуля.");
            return;
        }

        if (!TryParse(PriceBox.Text, out decimal price) || price < 0)
        {
            ShowError("Цену укажите числом, например 85,5.");
            return;
        }

        if (ExpiryBox.SelectedDate is not DateTime expiry)
        {
            ShowError("Укажите срок годности.");
            return;
        }

        if (DateBox.SelectedDate is DateTime date && expiry.Date < date.Date)
        {
            ShowError("Срок годности раньше даты поступления.");
            return;
        }

        _lines.Add(new SupplyLine
        {
            IngredientID = ingredient.IngredientID,
            IngredientName = ingredient.Name,
            Unit = ingredient.Unit,
            Quantity = quantity,
            UnitPrice = price,
            ExpirationDate = expiry
        });

        QuantityBox.Clear();
        PriceBox.Clear();
        ErrorText.Visibility = Visibility.Collapsed;
        UpdateTotal();
    }

    private void RemoveLine_Click(object sender, RoutedEventArgs e)
    {
        if (LinesGrid.SelectedItem is SupplyLine line)
        {
            _lines.Remove(line);
            UpdateTotal();
        }
    }

    private void Post_Click(object sender, RoutedEventArgs e)
    {
        if (SupplierBox.SelectedValue is not int supplierId)
        {
            ShowError("Выберите поставщика. Если поставщиков нет, добавьте его на вкладке «Поставщики».");
            return;
        }

        if (DateBox.SelectedDate is not DateTime date)
        {
            ShowError("Укажите дату поступления.");
            return;
        }

        var employee = Session.Current.Employee;
        if (employee == null)
        {
            ShowError("Не определён сотрудник, выполните вход заново.");
            return;
        }

        if (_service.TryPost(supplierId, date, _lines.ToList(), employee.EmployeeID, out string error))
        {
            DialogResult = true;
            return;
        }

        ShowError(error);
    }

    private void UpdateTotal()
    {
        TotalText.Text = "Итого: " + _lines.Sum(l => l.Sum).ToString("0.00", CultureInfo.CurrentCulture);
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