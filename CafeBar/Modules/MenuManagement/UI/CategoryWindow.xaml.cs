using System;
using System.Windows;
using CafeBar.Modules.MenuManagement.Data;
using CafeBar.Modules.MenuManagement.Logic;

namespace CafeBar.Modules.MenuManagement.UI;

public partial class CategoryWindow : Window
{
    private readonly MenuService _service = new();
    private readonly int? _categoryId;

    public CategoryWindow(Categories? category)
    {
        InitializeComponent();
        if (category == null)
        {
            HeaderText.Text = "Новая категория";
        }
        else
        {
            HeaderText.Text = "Категория";
            _categoryId = category.CategoryID;
            NameBox.Text = category.Name;
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (_service.TrySaveCategory(_categoryId, NameBox.Text, out string error))
        {
            DialogResult = true;
            return;
        }

        ErrorText.Text = error;
        ErrorText.Visibility = Visibility.Visible;
    }
}