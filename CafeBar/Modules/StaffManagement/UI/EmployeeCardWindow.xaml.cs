using System.Windows;

namespace CafeBar.Modules.StaffManagement.UI;

/// <summary>Окно карточки сотрудника.</summary>
public partial class EmployeeCardWindow : Window
{
    private readonly EmployeeCardViewModel _viewModel;

    public EmployeeCardWindow(EmployeeCardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.Save(PasswordInput.Password, PasswordRepeat.Password)) DialogResult = true;
    }
}
