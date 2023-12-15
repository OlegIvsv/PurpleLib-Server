using CatalogService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddDbContext<CatalogDbContext>(opt =>
    {
        opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    builder.Services.AddMassTransit(opt =>
    {
        opt.AddEntityFrameworkOutbox<CatalogDbContext>(o =>
        {
            o.QueryDelay = TimeSpan.FromSeconds(15);
            o.UsePostgres();
            o.UseBusOutbox();
        });
        opt.UsingRabbitMq((context, config) =>  
        {
            config.ConfigureEndpoints(context);
        });
    });
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();
    app.MapControllers();
}

try
{
    using var seeder = new DataSeeder(app)
        .ApplyMigrations()
        .InsertCatalogItems();
}
catch (Exception e)
{
    Console.WriteLine(e);
}


app.Run();