using GtpApi.DataModel.Entities;
using Microsoft.EntityFrameworkCore;
namespace GtpApi.DataModel.DataAccess;

public class DataContext : DbContext
{
    public DbSet<ChatInfo> ChatInfos { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected DataContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        //optionsBuilder.UseLoggerFactory(LoggerFactory.Create(x => x.AddConsole()));
#endif
        base.OnConfiguring(optionsBuilder);
    }
}
