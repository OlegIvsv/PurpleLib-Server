using MassTransit;
using SearchService.Consumers;
using SearchService.Data;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddSingleton<IItemRepository, MongoItemRepository>(_ =>
        new MongoItemRepository(builder.Configuration.GetConnectionString("DefaultMongoConnection")!));
    builder.Services.AddMassTransit(opt =>
    {
        opt.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
        opt.AddConsumer<CatalogItemCreatedConsumer>();
        
        opt.UsingRabbitMq((context, config) =>
        {
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