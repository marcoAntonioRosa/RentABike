using RentABike.Application;
using RentABike.Application.Configurations;
using RentABike.Domain.Entities;
using RentABike.Handlers;
using RentABike.Infrastructure;
using RentABike.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.RegisterMapsterConfiguration();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentity<User, Role>(opt =>
    {
        opt.Password.RequiredLength = 6;
        opt.Password.RequireDigit = true;
        opt.Password.RequireLowercase = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequireNonAlphanumeric = true;
    })
    .AddEntityFrameworkStores<PostgreSqlDbContext>();
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStatusCodePages();
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
