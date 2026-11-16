using System.Collections.ObjectModel;
using CafeBar.Modules.StaffManagement.Data;
using CafeBar.Modules.StaffManagement.Logic;
using CafeBar.ViewModels;

namespace CafeBar.Modules.StaffManagement.UI;

/// <summary>Список сотрудников в разделе «Персонал».</summary>
public class EmployeesViewModel : ViewModelBase
{
    private readonly EmployeeService _service = new();
    private Employees? _selectedEmployee;

    public EmployeesViewModel()
    {
        Reload();
    }

    public ObservableCollection<Employees> Employees { get; } = new();

    public Employees? SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            if (Set(ref _selectedEmployee, value)) OnPropertyChanged(nameof(IsEmployeeSelected));
        }
    }

    public bool IsEmployeeSelected => _selectedEmployee != null;

    /// <summary>Перечитывает список из базы и сохраняет выделение.</summary>
    public void Reload()
    {
        int? selectedId = SelectedEmployee?.EmployeeID;
        Employees.Clear();
        foreach (var employee in _service.GetAll()) Employees.Add(employee);
        SelectedEmployee = Employees.FirstOrDefault(e => e.EmployeeID == selectedId);

        // Если изменили карточку вошедшего сотрудника, в сеансе должны быть новые данные.
        int? currentId = Session.Current.Employee?.EmployeeID;
        var current = Employees.FirstOrDefault(e => e.EmployeeID == currentId);
        if (current != null) Session.Current.SignIn(current);
    }

    public bool TryDeleteSelected(out string error)
    {
        error = string.Empty;
        if (SelectedEmployee == null) return true;

        bool deleted = _service.TryDelete(SelectedEmployee.EmployeeID, Session.Current.Employee?.EmployeeID, out error);
        if (deleted) Reload();
        return deleted;
    }
}
