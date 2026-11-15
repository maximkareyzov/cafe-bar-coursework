using Microsoft.EntityFrameworkCore.Design;

namespace CafeBar.Data;

/// <summary>Создаёт контекст для команд dotnet ef (добавление и применение миграций).</summary>
public class CafeBarDbContextFactory : IDesignTimeDbContextFactory<CafeBarDbContext>
{
    public CafeBarDbContext CreateDbContext(string[] args) => AppDatabase.CreateContext();
}
