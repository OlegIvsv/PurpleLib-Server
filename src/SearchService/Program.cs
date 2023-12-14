using MassTransit;
using SearchService.Data;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<IItemRepository, MongoItemRepository>(_ =>
        new MongoItemRepository(builder.Configuration.GetConnectionString("DefaultMongoConnection")!));
    builder.Services.AddMassTransit(opt =>
    {
        opt.UsingRabbitMq((context, config) =>
        {
            config.Host("localhost",
                "/",
                h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            config.ConfigureEndpoints(context);
        });
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}

try
{
    new DataSeeder(app).SeedWithItems();
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();