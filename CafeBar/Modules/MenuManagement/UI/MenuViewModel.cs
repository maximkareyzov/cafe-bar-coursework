using System;
using System.Collections.ObjectModel;
using CafeBar.Modules.MenuManagement.Data;
using CafeBar.Modules.MenuManagement.Logic;
using CafeBar.Modules.StaffManagement.Logic;
using CafeBar.ViewModels;

namespace CafeBar.Modules.MenuManagement.UI;

public class MenuViewModel : ViewModelBase
{
    private readonly MenuService _service = new();
    private Categories? _selectedCategory;
    private MenuItems? _selectedItem;

    public MenuViewModel()
    {
        Reload();
    }

    public ObservableCollection<Categories> CategoryList { get; } = new();

    public ObservableCollection<MenuItems> ItemList { get; } = new();

    // кнопки управления видны только администратору
    public bool CanEdit => Session.Current.Employee?.Role == EmployeeRoles.Administrator;

    public Categories? SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (Set(ref _selectedCategory, value)) OnPropertyChanged(nameof(IsCategorySelected));
        }
    }

    public MenuItems? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (Set(ref _selectedItem, value)) OnPropertyChanged(nameof(IsItemSelected));
        }
    }

    public bool IsCategorySelected => _selectedCategory != null;

    public bool IsItemSelected => _selectedItem != null;

    public void Reload()
    {
        // запоминаем выделение, чтобы оно не слетало после обновления
        int? categoryId = SelectedCategory?.CategoryID;
        int? itemId = SelectedItem?.MenuItemID;
        int? modifierId = SelectedModifier?.ModifierID;

        CategoryList.Clear();
        foreach (var category in _service.GetCategories()) CategoryList.Add(category);
        ItemList.Clear();
        foreach (var item in _service.GetMenuItems()) ItemList.Add(item);

        SelectedCategory = CategoryList.FirstOrDefault(c => c.CategoryID == categoryId);
        SelectedItem = ItemList.FirstOrDefault(m => m.MenuItemID == itemId);
        ModifierList.Clear();
        foreach (var modifier in _service.GetModifiers()) ModifierList.Add(modifier);
        SelectedModifier = ModifierList.FirstOrDefault(m => m.ModifierID == modifierId);
    }

    public bool TryDeleteSelectedCategory(out string error)
    {
        error = string.Empty;
        if (SelectedCategory == null) return true;

        bool deleted = _service.TryDeleteCategory(SelectedCategory.CategoryID, out error);
        if (deleted) Reload();
        return deleted;
    }

    public bool TryDeleteSelectedItem(out string error)
    {
        error = string.Empty;
        if (SelectedItem == null) return true;

        bool deleted = _service.TryDeleteMenuItem(SelectedItem.MenuItemID, out error);
        if (deleted) Reload();
        return deleted;
    }

    public void ToggleSelectedAvailability()
    {
        if (SelectedItem == null) return;
        _service.SetAvailable(SelectedItem.MenuItemID, !SelectedItem.IsAvailable);
        Reload();
    }

private Modifiers? _selectedModifier;

    public ObservableCollection<Modifiers> ModifierList { get; } = new();

    public Modifiers? SelectedModifier
    {
        get => _selectedModifier;
        set
        {
            if (Set(ref _selectedModifier, value)) OnPropertyChanged(nameof(IsModifierSelected));
        }
    }

    public bool IsModifierSelected => _selectedModifier != null;

    public bool TryDeleteSelectedModifier(out string error)
    {
        error = string.Empty;
        if (SelectedModifier == null) return true;

        bool deleted = _service.TryDeleteModifier(SelectedModifier.ModifierID, out error);
        if (deleted) Reload();
        return deleted;
    }
}