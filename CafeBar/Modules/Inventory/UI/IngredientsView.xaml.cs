using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CafeBar.Modules.Inventory.UI;

public partial class IngredientsView : UserControl
{
    public IngredientsView()
    {
        InitializeComponent();
    }

    private IngredientsViewModel ViewModel => (IngredientsViewModel)DataContext;

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        OpenWindow(null);
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedRow != null) OpenWindow(ViewModel.SelectedRow.IngredientID);
    }

    private void Rows_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (!ViewModel.CanEdit) return;
        // клик по заголовку столбца не считаем
        bool onRow = e.OriginalSource is DependencyObject source
            && ItemsControl.ContainerFromElement((DataGrid)sender, source) is DataGridRow;
        if (onRow && ViewModel.SelectedRow != null) OpenWindow(ViewModel.SelectedRow.IngredientID);
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        var row = ViewModel.SelectedRow;
        if (row == null) return;

        var answer = MessageBox.Show($"Удалить ингредиент «{row.Name}»?", "Склад",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer != MessageBoxResult.Yes) return;

        if (!ViewModel.TryDeleteSelected(out string error))
        {
            MessageBox.Show(error, "Склад", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    // null - создаём новый ингредиент
    private void OpenWindow(int? ingredientId)
    {
        var window = new IngredientWindow(ingredientId) { Owner = Window.GetWindow(this), ShowInTaskbar = false };
        if (window.ShowDialog() == true) ViewModel.Reload();
    }
}