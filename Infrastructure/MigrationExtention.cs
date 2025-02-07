using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;

public static class MigrationExtension
{
    public static void ApplyMigration(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<EventDBContext>();

        dbContext.Database.Migrate();
    }
}
