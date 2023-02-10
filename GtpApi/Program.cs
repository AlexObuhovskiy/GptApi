using GtpApi.DataModel.DataAccess;
using GtpApi.Services;
using GtpApi.Setup;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var gptConfigSection = builder.Configuration.GetSection(nameof(OpenApiOptions));
_ = builder.Services.Configure<OpenApiOptions>(gptConfigSection);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create DB with migration
var dbBuilder = new DbContextOptionsBuilder<DataContext>();
var connectionString = builder.Configuration.GetConnectionString("GptDb");
dbBuilder.UseSqlServer(connectionString);
using var tenantMigrationContext = Activator.CreateInstance(typeof(DataContext), dbBuilder.Options) as DataContext;
tenantMigrationContext!.Database.SetCommandTimeout(300);
tenantMigrationContext!.Database.Migrate();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(connectionString);
});

builder.Services.AddTransient<IGptService, GptService>();
builder.Services.AddTransient<IGptHttpClient, GptHttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
