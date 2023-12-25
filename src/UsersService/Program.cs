using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using UsersService.Services;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.Authority = builder.Configuration["IdentityServerUrl"];
            opt.RequireHttpsMetadata = false;
            opt.TokenValidationParameters.ValidateAudience = false;
            opt.TokenValidationParameters.NameClaimType = "sub";
        });
    builder.Services.AddAuthorization(opt => opt.AddPolicy("SellerOnly", policyBuilder => { policyBuilder.RequireRole("seller"); }));
    builder.Services.AddSingleton<ISellerRepository, SellerMongoRepository>(_ =>
        new SellerMongoRepository(builder.Configuration.GetConnectionString("DefaultMongoConnection")!));
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}
var app = builder.Build();
{
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true) // allow any origin
        .AllowCredentials());
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}
app.Run();