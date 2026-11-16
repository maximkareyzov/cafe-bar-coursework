namespace CafeBar.Modules.StaffManagement.Logic;

/// <summary>Роли сотрудников из ТЗ (п. 3.7). Название роли хранится в поле Employees.Role.</summary>
public static class EmployeeRoles
{
    public const string Administrator = "Администратор";
    public const string WaiterBartender = "Официант/бармен";
    public const string Storekeeper = "Кладовщик";

    public static IReadOnlyList<string> All { get; } = new[] { Administrator, WaiterBartender, Storekeeper };
}
