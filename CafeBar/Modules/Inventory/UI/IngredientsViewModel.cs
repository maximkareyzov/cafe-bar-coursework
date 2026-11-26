using System.Collections.ObjectModel;
using CafeBar.Modules.Inventory.Logic;
using CafeBar.Modules.StaffManagement.Logic;
using CafeBar.ViewModels;

namespace CafeBar.Modules.Inventory.UI;

public class IngredientsViewModel : ViewModelBase
{
    private readonly IngredientService _service = new();
    private string _searchText = string.Empty;
    private bool _onlyLow;
    private IngredientRow? _selectedRow;
    private string _summary = string.Empty;

    public IngredientsViewModel()
    {
        Reload();
    }

    public ObservableCollection<IngredientRow> Rows { get; } = new();

    // менять склад могут администратор и кладовщик
    public bool CanEdit
    {
        get
        {
            var role = Session.Current.Employee?.Role;
            return role == EmployeeRoles.Administrator || role == EmployeeRoles.Storekeeper;
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (Set(ref _searchText, value)) Reload();
        }
    }

    public bool OnlyLow
    {
        get => _onlyLow;
        set
        {
            if (Set(ref _onlyLow, value)) Reload();
        }
    }

    public IngredientRow? SelectedRow
    {
        get => _selectedRow;
        set
        {
            if (Set(ref _selectedRow, value)) OnPropertyChanged(nameof(IsRowSelected));
        }
    }

    public bool IsRowSelected => _selectedRow != null;

    public string Summary
    {
        get => _summary;
        private set => Set(ref _summary, value);
    }

    public void Reload()
    {
        // запоминаем выделение, чтобы оно не слетало при поиске
        int? selectedId = SelectedRow?.IngredientID;
        Rows.Clear();
        foreach (var row in _service.GetRows(SearchText, OnlyLow)) Rows.Add(row);
        SelectedRow = Rows.FirstOrDefault(r => r.IngredientID == selectedId);
        Summary = $"Показано: {Rows.Count}, ниже порога: {Rows.Count(r => r.IsLow)}";
    }

    public bool TryDeleteSelected(out string error)
    {
        error = string.Empty;
        if (SelectedRow == null) return true;

        bool deleted = _service.TryDelete(SelectedRow.IngredientID, out error);
        if (deleted) Reload();
        return deleted;
    }
}