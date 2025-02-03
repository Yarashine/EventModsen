using Microsoft.EntityFrameworkCore;

namespace EventModsen.Infrastructure.DB;

public static class MigrationExtention
{
    public static void ApplyMigration(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        using EventDBContext dBContext =
            scope.ServiceProvider.GetRequiredService<EventDBContext>();

        dBContext.Database.Migrate();

    }
}
