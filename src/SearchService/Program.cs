using SearchService.Data;
using SearchService.MassTransit;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddSingleton<IItemRepository, MongoItemRepository>(_ =>
        new MongoItemRepository(builder.Configuration.GetConnectionString("DefaultMongoConnection")!));
    builder.Services.AddMassTransitWithConfigurations();
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