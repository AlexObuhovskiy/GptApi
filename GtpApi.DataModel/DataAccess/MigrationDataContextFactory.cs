using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GtpApi.DataModel.DataAccess;

public class MigrationDataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=GptDb;Integrated Security=true;Max Pool Size=1000;TrustServerCertificate=true");

        return new DataContext(optionsBuilder.Options);
    }
}
