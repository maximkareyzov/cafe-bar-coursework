using System.Collections.ObjectModel;
using CafeBar.Modules.Inventory.Data;
using CafeBar.Modules.Inventory.Logic;
using CafeBar.Modules.StaffManagement.Logic;
using CafeBar.ViewModels;

namespace CafeBar.Modules.Inventory.UI;

public class SuppliersViewModel : ViewModelBase
{
    private readonly SupplierService _service = new();
    private Suppliers? _selected;

    public SuppliersViewModel()
    {
        Reload();
    }

    public ObservableCollection<Suppliers> Rows { get; } = new();

    // менять могут администратор и кладовщик
    public bool CanEdit
    {
        get
        {
            var role = Session.Current.Employee?.Role;
            return role == EmployeeRoles.Administrator || role == EmployeeRoles.Storekeeper;
        }
    }

    public Suppliers? Selected
    {
        get => _selected;
        set
        {
            if (Set(ref _selected, value)) OnPropertyChanged(nameof(IsSelected));
        }
    }

    public bool IsSelected => _selected != null;

    public void Reload()
    {
        int? selectedId = Selected?.SupplierID;
        Rows.Clear();
        foreach (var supplier in _service.GetAll()) Rows.Add(supplier);
        Selected = Rows.FirstOrDefault(s => s.SupplierID == selectedId);
    }

    public bool TryDeleteSelected(out string error)
    {
        error = string.Empty;
        if (Selected == null) return true;

        bool deleted = _service.TryDelete(Selected.SupplierID, out error);
        if (deleted) Reload();
        return deleted;
    }
}