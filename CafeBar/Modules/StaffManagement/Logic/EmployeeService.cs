using CafeBar.Data;
using CafeBar.Modules.StaffManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Modules.StaffManagement.Logic;

/// <summary>Карточки сотрудников и вход в систему по логину и паролю (ТЗ, п. 3.6).</summary>
public class EmployeeService
{
    public bool HasEmployees()
    {
        using var db = AppDatabase.CreateContext();
        return db.Employees.Any();
    }

    public List<Employees> GetAll()
    {
        using var db = AppDatabase.CreateContext();
        return db.Employees.AsNoTracking().ToList()
            .OrderBy(e => e.FullName, StringComparer.CurrentCulture)
            .ToList();
    }

    /// <summary>Возвращает сотрудника, если логин и пароль верны, иначе null.</summary>
    public Employees? SignIn(string login, string password)
    {
        login = login.Trim();
        using var db = AppDatabase.CreateContext();
        // Логин сравнивается в коде: SQLite не учитывает регистр только у латинских букв.
        var employee = db.Employees.AsNoTracking().ToList().FirstOrDefault(e => SameLogin(e.Login, login));
        if (employee == null || !PasswordHasher.Verify(password, employee.PasswordHash)) return null;
        return employee;
    }

    public bool TryCreate(string fullName, string login, string role, decimal salesPercent, string password, out string error)
    {
        using var db = AppDatabase.CreateContext();
        if (!Validate(db, null, ref fullName, ref login, role, salesPercent, out error)) return false;
        if (password.Length == 0)
        {
            error = "Укажите пароль.";
            return false;
        }

        db.Employees.Add(new Employees
        {
            FullName = fullName,
            Login = login,
            Role = role,
            SalesPercent = salesPercent,
            PasswordHash = PasswordHasher.Hash(password)
        });
        db.SaveChanges();
        return true;
    }

    /// <summary>Изменяет карточку. Пароль меняется, только если newPassword задан.</summary>
    public bool TryUpdate(int employeeId, string fullName, string login, string role, decimal salesPercent, string? newPassword, out string error)
    {
        using var db = AppDatabase.CreateContext();
        var employee = db.Employees.SingleOrDefault(e => e.EmployeeID == employeeId);
        if (employee == null)
        {
            error = "Сотрудник не найден: возможно, его уже удалили.";
            return false;
        }

        if (!Validate(db, employeeId, ref fullName, ref login, role, salesPercent, out error)) return false;
        if (employee.Role == EmployeeRoles.Administrator && role != EmployeeRoles.Administrator && IsOnlyAdministrator(db, employeeId))
        {
            error = "Нельзя снять роль администратора с единственного администратора.";
            return false;
        }

        employee.FullName = fullName;
        employee.Login = login;
        employee.Role = role;
        employee.SalesPercent = salesPercent;
        if (!string.IsNullOrEmpty(newPassword)) employee.PasswordHash = PasswordHasher.Hash(newPassword);
        db.SaveChanges();
        return true;
    }

    /// <summary>Удаляет карточку. currentEmployeeId — сотрудник, под которым выполнен вход.</summary>
    public bool TryDelete(int employeeId, int? currentEmployeeId, out string error)
    {
        error = string.Empty;
        using var db = AppDatabase.CreateContext();
        var employee = db.Employees.SingleOrDefault(e => e.EmployeeID == employeeId);
        if (employee == null) return true;

        if (employeeId == currentEmployeeId)
        {
            error = "Нельзя удалить сотрудника, под которым выполнен вход.";
            return false;
        }

        if (employee.Role == EmployeeRoles.Administrator && IsOnlyAdministrator(db, employeeId))
        {
            error = "Нельзя удалить единственного администратора.";
            return false;
        }

        db.Employees.Remove(employee);
        try
        {
            db.SaveChanges();
        }
        catch (DbUpdateException)
        {
            // Внешние ключи не дают удалить сотрудника, на которого ссылаются другие таблицы.
            error = "Сотрудника нельзя удалить: с ним связаны заказы, смены, записи журнала или складские документы.";
            return false;
        }

        return true;
    }

    private static bool Validate(CafeBarDbContext db, int? employeeId, ref string fullName, ref string login, string role, decimal salesPercent, out string error)
    {
        fullName = fullName.Trim();
        login = login.Trim();
        error = string.Empty;

        if (fullName.Length == 0) error = "Укажите ФИО.";
        else if (login.Length == 0) error = "Укажите логин.";
        else if (!EmployeeRoles.All.Contains(role)) error = "Выберите должность (роль).";
        else if (salesPercent < 0 || salesPercent > 100) error = "Процент от продаж должен быть от 0 до 100.";
        else
        {
            // Уникальность логина проверяется здесь: на диаграммах такого ограничения в базе нет.
            string checkedLogin = login;
            bool taken = db.Employees.AsNoTracking()
                .Select(e => new { e.EmployeeID, e.Login })
                .ToList()
                .Any(e => e.EmployeeID != employeeId && SameLogin(e.Login, checkedLogin));
            if (taken) error = $"Логин «{login}» уже занят.";
        }

        return error.Length == 0;
    }

    private static bool IsOnlyAdministrator(CafeBarDbContext db, int employeeId)
    {
        return !db.Employees.Any(e => e.EmployeeID != employeeId && e.Role == EmployeeRoles.Administrator);
    }

    private static bool SameLogin(string first, string second)
    {
        return string.Equals(first, second, StringComparison.OrdinalIgnoreCase);
    }
}
