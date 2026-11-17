using CafeBar.Data;
using CafeBar.Modules.StaffManagement.Data;

namespace CafeBar.Modules.StaffManagement.Logic;

//Регистрация начала и завершения смены
public class ShiftService
{
    //Незавершённая смена сотрудника; null, если смена не начата.
    public Shifts? GetOpenShift(int employeeId)
    {
        using var db = AppDatabase.CreateContext();
        return db.Shifts.FirstOrDefault(s => s.EmployeeID == employeeId && s.ClockOut == null);
    }

    public bool TryStart(int employeeId, out string error)
    {
        error = string.Empty;
        using var db = AppDatabase.CreateContext();
        if (db.Shifts.Any(s => s.EmployeeID == employeeId && s.ClockOut == null))
        {
            error = "Смена уже начата.";
            return false;
        }

        db.Shifts.Add(new Shifts { EmployeeID = employeeId, ClockIn = DateTime.Now });
        db.SaveChanges();
        return true;
    }

    public bool TryEnd(int employeeId, out string error)
    {
        error = string.Empty;
        using var db = AppDatabase.CreateContext();
        var shift = db.Shifts.FirstOrDefault(s => s.EmployeeID == employeeId && s.ClockOut == null);
        if (shift == null)
        {
            error = "Нет начатой смены.";
            return false;
        }

        shift.ClockOut = DateTime.Now;
        db.SaveChanges();
        return true;
    }
}