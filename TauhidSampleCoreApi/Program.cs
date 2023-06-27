using System.Reflection;
using TauhidSampleCoreApi.Infrastructure.Repositories;
using TauhidSampleCoreApi.Infrastructure;
using Zero.EFCoreSpecification;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSQLServerEFCoreSpecificationServices<ApplicationDbContext>
    (builder.Configuration.GetConnectionString("Dbconnection")!, typeof(EfRepository<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
