using System.Collections.ObjectModel;
using CafeBar.Modules.Inventory.Logic;
using CafeBar.Modules.StaffManagement.Logic;
using CafeBar.ViewModels;

namespace CafeBar.Modules.Inventory.UI;

public class SuppliesViewModel : ViewModelBase
{
    private readonly SupplyService _service = new();
    private SupplyRow? _selected;

    public SuppliesViewModel()
    {
        Reload();
    }

    // сообщаем склад, что остатки поменялись
    public event Action? Posted;

    public ObservableCollection<SupplyRow> Documents { get; } = new();

    public ObservableCollection<SupplyLineRow> Lines { get; } = new();

    public bool CanEdit
    {
        get
        {
            var role = Session.Current.Employee?.Role;
            return role == EmployeeRoles.Administrator || role == EmployeeRoles.Storekeeper;
        }
    }

    public SupplyRow? Selected
    {
        get => _selected;
        set
        {
            if (!Set(ref _selected, value)) return;
            LoadLines();
        }
    }

    public void Reload()
    {
        int? selectedId = Selected?.SupplyDocumentID;
        Documents.Clear();
        foreach (var document in _service.GetDocuments()) Documents.Add(document);
        Selected = Documents.FirstOrDefault(d => d.SupplyDocumentID == selectedId);
    }

    public void OnPosted()
    {
        Reload();
        Posted?.Invoke();
    }

    private void LoadLines()
    {
        Lines.Clear();
        if (Selected == null) return;
        foreach (var line in _service.GetLines(Selected.SupplyDocumentID)) Lines.Add(line);
    }
}