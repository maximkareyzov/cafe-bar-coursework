namespace CafeBar.Modules.Inventory.UI;

// объединяет вкладки раздела «Склад»
public class InventoryViewModel
{
    public InventoryViewModel()
    {
        Ingredients = new IngredientsViewModel();
        Suppliers = new SuppliersViewModel();
        Supplies = new SuppliesViewModel();
        // после проведения поступления остатки изменились
        Supplies.Posted += Ingredients.Reload;
    }

    public IngredientsViewModel Ingredients { get; }

    public SuppliersViewModel Suppliers { get; }

    public SuppliesViewModel Supplies { get; }
}