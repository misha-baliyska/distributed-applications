using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TM.ApplicationServices.Implementations;
using TM.ApplicationServices.Interfaces;
using TM.Data.Contexts;
using TM.Data.Entities;
using TM.Repositories.Implementations;
using TM.Repositories.Interfaces;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<TravelMeDbContext>(options => options.UseSqlServer(connectionString,
            x => x.MigrationsAssembly("TM.WebAPI")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Start SERVICE DI
builder.Services.AddScoped<DbContext, TravelMeDbContext>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITripsRepository, TripsRepository>();
builder.Services.AddScoped<IReservationsRepository, ReservationsRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersManagementService, UsersManagementService>();
builder.Services.AddScoped<ITripsManagementService, TripsManagementService>();
builder.Services.AddScoped<IReservationsManagementService, ReservationsManagementService>();

// End SERVICE DI



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

SeedAdminUser(app);

app.Run();

void SeedAdminUser(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TravelMeDbContext>();

    // Check if any admin user exists in the database
    var adminExists = dbContext.Users.Any(u => u.IsAdmin);

    // If no admin user exists, add an admin user
    if (!adminExists)
    {
        var admin = new User
        {
            Username = "admin",
            Password = "123", // Note: You should hash passwords in a real-world scenario
            IsAdmin = true,
            CreatedBy = 1,
            CreatedOn = DateTime.Now,
            IsActivated = true,
            FirstName = "Admin",
            LastName = "Adminov",
            Email = "admin@"
        };
        dbContext.Users.Add(admin);
        dbContext.SaveChanges();
    }
}
