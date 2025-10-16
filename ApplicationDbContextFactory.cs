using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PersonalStylistIA.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // ✅ Corrigido para SQLite
        optionsBuilder.UseSqlite("Data Source=PersonalStylistDB.db");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
