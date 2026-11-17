namespace CafeBar.Modules.StaffManagement.Logic;

//Какие разделы доступны каждой роли
public static class AccessPolicy
{
    private static readonly Dictionary<string, string[]> Sections = new()
    {
        [EmployeeRoles.Administrator] = new[] { "Заказы", "Меню", "Склад", "Техкарты", "Отчёты", "Персонал", "Администрирование" },
        [EmployeeRoles.WaiterBartender] = new[] { "Заказы", "Меню" },
        [EmployeeRoles.Storekeeper] = new[] { "Склад", "Техкарты" }
    };

    public static bool CanOpen(string? role, string section)
    {
        if (role == null || !Sections.TryGetValue(role, out var allowed)) return false;
        return allowed.Contains(section);
    }

    //Первый раздел, который можно открыть после входа.
    public static string FirstSection(string? role)
    {
        if (role != null && Sections.TryGetValue(role, out var allowed)) return allowed[0];
        return "Заказы";
    }
}