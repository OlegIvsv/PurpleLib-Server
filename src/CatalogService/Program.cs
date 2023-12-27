using CatalogService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

// TODO: move large service configs to it's own extension method

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
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {   
            opt.Authority = builder.Configuration["IdentityServerUrl"];
            opt.RequireHttpsMetadata = false;
            opt.TokenValidationParameters.ValidateAudience = false;
            opt.TokenValidationParameters.NameClaimType = "username";
        });
}

var app = builder.Build();
{
    app.UseCors(config => config.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseAuthentication();
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