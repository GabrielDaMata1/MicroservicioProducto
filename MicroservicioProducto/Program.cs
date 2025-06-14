using Domain.Interfaces;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Infrastructure.Persistance;
using Infrastructure.Repositories.PostgreSQL;
using System.Reflection;
using Application.Command;
using Application.Services;
using Infrastructure.Consumers;
using Infrastructure.Models.MongoDB;
using Infrastructure.Repositories.MongoDB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mi API",
        Version = "v1",
        Description = "Documentaci�n de mi API usando Swagger"
    });
});


builder.Services.AddDbContext<SubastaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"),
        b => b.MigrationsAssembly("Infrastructure")));


// Configuraci�n de MongoDB
var mongoClient = new MongoClient("mongodb://localhost:27017");
builder.Services.AddSingleton<IMongoClient>(mongoClient);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddHttpClient<UsuarioService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/api/usuarios/");
});


builder.Services.AddScoped<IProductoRepositoryPostgreSQL, ProductoPostgreSQLRepository>();
builder.Services.AddScoped<IProductoMongoRepository, ProductoMongoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegistrarProductoCommand).Assembly));


// Configuraci�n de RabbitMQ con MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductoRegistadoConsumer>();
    x.AddConsumer<ModificarProductoConsumer>();
    x.AddConsumer<EliminarProductoConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("producto-registrado-queue", e =>
        {
            e.ConfigureConsumer<ProductoRegistadoConsumer>(context);
        });

        cfg.ReceiveEndpoint("producto-modificado-queue", e =>
        {
            e.ConfigureConsumer<ModificarProductoConsumer>(context);
        });

        cfg.ReceiveEndpoint("producto-eliminado-queue", e =>
        {
            e.ConfigureConsumer<EliminarProductoConsumer>(context);
        });
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
    });
}
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();