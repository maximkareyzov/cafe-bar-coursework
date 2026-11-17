using System.Windows;
using CafeBar.ViewModels;
using System.Windows.Controls;
using CafeBar.Modules.StaffManagement.Logic;

namespace CafeBar;

public partial class MainWindow : Window
{
    private readonly ShiftService _shifts = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        ApplyAccess();
        RefreshShift();
    }

    //>Нажата кнопка «Выйти»: окно нужно закрыть и снова показать окно входа
    public event EventHandler? SignOutRequested;

    private void SignOut_Click(object sender, RoutedEventArgs e)
    {
        SignOutRequested?.Invoke(this, EventArgs.Empty);
    }

    //Скрывает кнопки разделов, которые недоступны роли вошедшего сотрудника
    private void ApplyAccess()
    {
        var role = Session.Current.Employee?.Role;
        foreach (var button in NavigationPanel.Children.OfType<Button>())
        {
            string section = button.CommandParameter as string ?? "";
            button.Visibility = AccessPolicy.CanOpen(role, section) ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private void RefreshShift()
    {
        var employee = Session.Current.Employee;
        if (employee == null) return;

        var shift = _shifts.GetOpenShift(employee.EmployeeID);
        if (shift == null)
        {
            ShiftText.Text = "Смена не начата";
            ShiftButton.Content = "Начать смену";
        }
        else
        {
            ShiftText.Text = "На смене с " + shift.ClockIn.ToString("dd.MM HH:mm");
            ShiftButton.Content = "Завершить смену";
        }
    }

    private void Shift_Click(object sender, RoutedEventArgs e)
    {
        var employee = Session.Current.Employee;
        if (employee == null) return;

        string error;
        bool done = _shifts.GetOpenShift(employee.EmployeeID) == null
            ? _shifts.TryStart(employee.EmployeeID, out error)
            : _shifts.TryEnd(employee.EmployeeID, out error);
        if (!done) MessageBox.Show(error, "Смена", MessageBoxButton.OK, MessageBoxImage.Warning);
        RefreshShift();
    }
}