using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CafeBar.Data;

/// <summary>Расположение локальной базы данных и создание контекста для работы с ней.</summary>
public static class AppDatabase
{
    // DoNotVerify: без этой опции GetFolderPath возвращает пустую строку, если каталога нет,
    // и база молча создалась бы в текущем рабочем каталоге.
    public static string DirectoryPath { get; } = Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData,
            Environment.SpecialFolderOption.DoNotVerify),
        "CafeBar");

    public static string FilePath { get; } = Path.Combine(DirectoryPath, "cafebar.db");

    public static CafeBarDbContext CreateContext()
    {
        // SQLite сам создаёт файл базы, но не каталог для него.
        Directory.CreateDirectory(DirectoryPath);

        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = FilePath,
            ForeignKeys = true
        }.ToString();

        var options = new DbContextOptionsBuilder<CafeBarDbContext>()
            .UseSqlite(connectionString)
            .Options;

        return new CafeBarDbContext(options);
    }

    /// <summary>Создаёт базу при первом запуске и применяет ещё не применённые миграции.</summary>
    public static void Initialize()
    {
        using var context = CreateContext();
        context.Database.Migrate();
    }
}
