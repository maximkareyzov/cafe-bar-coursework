using System.Globalization;
using CafeBar.Modules.StaffManagement.Data;
using CafeBar.Modules.StaffManagement.Logic;
using CafeBar.ViewModels;

namespace CafeBar.Modules.StaffManagement.UI;

/// <summary>Карточка сотрудника: создание и изменение (ТЗ, п. 3.6).</summary>
public class EmployeeCardViewModel : ViewModelBase
{
    private readonly EmployeeService _service = new();
    private readonly int? _employeeId;
    private string _fullName = string.Empty;
    private string _login = string.Empty;
    private string _role = EmployeeRoles.WaiterBartender;
    private string _salesPercent = "0";
    private string _errorMessage = string.Empty;

    /// <param name="employee">Сотрудник, карточку которого изменяют; null — новая карточка.</param>
    /// <param name="firstAdministrator">Первый запуск: создаётся администратор, роль выбрать нельзя.</param>
    public EmployeeCardViewModel(Employees? employee, bool firstAdministrator = false)
    {
        if (firstAdministrator)
        {
            Title = "Первый запуск";
            Hint = "В базе ещё нет сотрудников. Создайте администратора — под ним вы войдёте в систему.";
            _role = EmployeeRoles.Administrator;
            CanChooseRole = false;
        }
        else if (employee == null)
        {
            Title = "Новый сотрудник";
            Hint = "Заполните карточку и задайте пароль для входа.";
            CanChooseRole = true;
        }
        else
        {
            Title = "Карточка сотрудника";
            Hint = "Чтобы оставить прежний пароль, не заполняйте поля пароля.";
            CanChooseRole = true;
            _employeeId = employee.EmployeeID;
            _fullName = employee.FullName;
            _login = employee.Login;
            _role = employee.Role;
            _salesPercent = employee.SalesPercent.ToString("0.##", CultureInfo.CurrentCulture);
        }
    }

    public string Title { get; }

    public string Hint { get; }

    public bool CanChooseRole { get; }

    public IReadOnlyList<string> Roles => EmployeeRoles.All;

    public string FullName
    {
        get => _fullName;
        set => Set(ref _fullName, value);
    }

    public string Login
    {
        get => _login;
        set => Set(ref _login, value);
    }

    public string Role
    {
        get => _role;
        set => Set(ref _role, value);
    }

    /// <summary>Процент от продаж в том виде, как его ввели: с запятой или с точкой.</summary>
    public string SalesPercent
    {
        get => _salesPercent;
        set => Set(ref _salesPercent, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            if (Set(ref _errorMessage, value)) OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => _errorMessage.Length > 0;

    /// <summary>
    /// Сохраняет карточку и возвращает true при успехе. Пароли передаются отдельно:
    /// поле пароля в WPF не поддерживает привязку.
    /// </summary>
    public bool Save(string password, string passwordRepeat)
    {
        if (password != passwordRepeat)
        {
            ErrorMessage = "Пароли не совпадают.";
            return false;
        }

        if (!TryParsePercent(SalesPercent, out decimal percent))
        {
            ErrorMessage = "Процент от продаж укажите числом, например 2,5.";
            return false;
        }

        string error;
        bool saved = _employeeId == null
            ? _service.TryCreate(FullName, Login, Role, percent, password, out error)
            : _service.TryUpdate(_employeeId.Value, FullName, Login, Role, percent, password, out error);
        ErrorMessage = error;
        return saved;
    }

    private static bool TryParsePercent(string text, out decimal value)
    {
        return decimal.TryParse(text.Trim().Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
    }
}
