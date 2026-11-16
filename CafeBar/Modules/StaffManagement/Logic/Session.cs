using System.ComponentModel;
using CafeBar.Modules.StaffManagement.Data;

namespace CafeBar.Modules.StaffManagement.Logic;

/// <summary>Текущий сеанс работы: сотрудник, под которым выполнен вход.</summary>
public sealed class Session : INotifyPropertyChanged
{
    private Employees? _employee;

    private Session()
    {
    }

    public static Session Current { get; } = new();

    /// <summary>Вошедший сотрудник; null, пока вход не выполнен.</summary>
    public Employees? Employee
    {
        get => _employee;
        private set
        {
            _employee = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Employee)));
        }
    }

    public void SignIn(Employees employee) => Employee = employee;

    public void SignOut() => Employee = null;

    public event PropertyChangedEventHandler? PropertyChanged;
}
