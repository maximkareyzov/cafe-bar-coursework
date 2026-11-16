using System.Windows;
using CafeBar.Modules.StaffManagement.Logic;

namespace CafeBar.Modules.StaffManagement.UI;

/// <summary>Окно входа по логину и паролю.</summary>
public partial class LoginWindow : Window
{
    private readonly EmployeeService _employees = new();

    public LoginWindow()
    {
        InitializeComponent();
    }

    private void SignIn_Click(object sender, RoutedEventArgs e)
    {
        var employee = _employees.SignIn(LoginBox.Text, PasswordInput.Password);
        if (employee == null)
        {
            ErrorText.Text = "Неверный логин или пароль.";
            ErrorText.Visibility = Visibility.Visible;
            PasswordInput.Clear();
            PasswordInput.Focus();
            return;
        }

        Session.Current.SignIn(employee);
        DialogResult = true;
    }
}
