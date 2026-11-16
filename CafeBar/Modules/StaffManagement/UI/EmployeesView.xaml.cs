using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CafeBar.Modules.StaffManagement.Data;

namespace CafeBar.Modules.StaffManagement.UI;

/// <summary>Раздел «Персонал»: список сотрудников и работа с карточками.</summary>
public partial class EmployeesView : UserControl
{
    public EmployeesView()
    {
        InitializeComponent();
    }

    private EmployeesViewModel ViewModel => (EmployeesViewModel)DataContext;

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        OpenCard(null);
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedEmployee != null) OpenCard(ViewModel.SelectedEmployee);
    }

    private void Employees_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // Двойной щелчок по строке открывает карточку; щелчки по заголовкам столбцов не считаются.
        bool onRow = e.OriginalSource is DependencyObject source
            && ItemsControl.ContainerFromElement((DataGrid)sender, source) is DataGridRow;
        if (onRow && ViewModel.SelectedEmployee != null) OpenCard(ViewModel.SelectedEmployee);
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        var employee = ViewModel.SelectedEmployee;
        if (employee == null) return;

        var answer = MessageBox.Show(
            $"Удалить сотрудника «{employee.FullName}»?",
            "Персонал",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (answer != MessageBoxResult.Yes) return;

        if (!ViewModel.TryDeleteSelected(out string error))
        {
            MessageBox.Show(error, "Персонал", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void OpenCard(Employees? employee)
    {
        var card = new EmployeeCardWindow(new EmployeeCardViewModel(employee))
        {
            Owner = Window.GetWindow(this),
            ShowInTaskbar = false
        };
        if (card.ShowDialog() == true) ViewModel.Reload();
    }
}
