using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CafeBar.Modules.MenuManagement.Data;

namespace CafeBar.Modules.MenuManagement.UI;

public partial class MenuView : UserControl
{
    public MenuView()
    {
        InitializeComponent();
    }

    private MenuViewModel ViewModel => (MenuViewModel)DataContext;

    private void AddItem_Click(object sender, RoutedEventArgs e)
    {
        OpenItem(null);
    }

    private void EditItem_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedItem != null) OpenItem(ViewModel.SelectedItem);
    }

    private void Items_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (!ViewModel.CanEdit) return;
        // клик по заголовку столбца не считаем
        bool onRow = e.OriginalSource is DependencyObject source
            && ItemsControl.ContainerFromElement((DataGrid)sender, source) is DataGridRow;
        if (onRow && ViewModel.SelectedItem != null) OpenItem(ViewModel.SelectedItem);
    }

    private void DeleteItem_Click(object sender, RoutedEventArgs e)
    {
        var item = ViewModel.SelectedItem;
        if (item == null) return;

        var answer = MessageBox.Show($"Удалить позицию «{item.Name}»?", "Меню",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer != MessageBoxResult.Yes) return;

        if (!ViewModel.TryDeleteSelectedItem(out string error))
        {
            MessageBox.Show(error, "Меню", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void ToggleAvailability_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ToggleSelectedAvailability();
    }

    private void AddCategory_Click(object sender, RoutedEventArgs e)
    {
        OpenCategory(null);
    }

    private void EditCategory_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedCategory != null) OpenCategory(ViewModel.SelectedCategory);
    }

    private void DeleteCategory_Click(object sender, RoutedEventArgs e)
    {
        var category = ViewModel.SelectedCategory;
        if (category == null) return;

        var answer = MessageBox.Show($"Удалить категорию «{category.Name}»?", "Меню",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer != MessageBoxResult.Yes) return;

        if (!ViewModel.TryDeleteSelectedCategory(out string error))
        {
            MessageBox.Show(error, "Меню", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    // null - создаём новую позицию
    private void OpenItem(MenuItems? item)
    {
        var window = new MenuItemWindow(item) { Owner = Window.GetWindow(this), ShowInTaskbar = false };
        if (window.ShowDialog() == true) ViewModel.Reload();
    }

    private void OpenCategory(Categories? category)
    {
        var window = new CategoryWindow(category) { Owner = Window.GetWindow(this), ShowInTaskbar = false };
        if (window.ShowDialog() == true) ViewModel.Reload();
    }

private void AddModifier_Click(object sender, RoutedEventArgs e)
    {
        OpenModifier(null);
    }

    private void EditModifier_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedModifier != null) OpenModifier(ViewModel.SelectedModifier);
    }

    private void DeleteModifier_Click(object sender, RoutedEventArgs e)
    {
        var modifier = ViewModel.SelectedModifier;
        if (modifier == null) return;

        var answer = MessageBox.Show($"Удалить модификатор «{modifier.Name}»?", "Меню",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer != MessageBoxResult.Yes) return;

        if (!ViewModel.TryDeleteSelectedModifier(out string error))
        {
            MessageBox.Show(error, "Меню", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void OpenModifier(Modifiers? modifier)
    {
        var window = new ModifierWindow(modifier) { Owner = Window.GetWindow(this), ShowInTaskbar = false };
        if (window.ShowDialog() == true) ViewModel.Reload();
    }
}