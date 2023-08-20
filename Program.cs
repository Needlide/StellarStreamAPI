using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using StellarStreamAPI.Database;
using StellarStreamAPI.Interfaces;
using StellarStreamAPI.Security;
using StellarStreamAPI.Security.JWT;
using StellarStreamAPI.Security.Validators;

var builder = WebApplication.CreateBuilder(args);

string AllowedOriginsPolicyName = "AllowedAnyOrigins";
string AppConfigConnectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Configuration.AddAzureAppConfiguration(AppConfigConnectionString);
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("StellarStreamAppConfig"));
var appConfig = new AppConfig();
builder.Configuration.GetSection("StellarStreamAppConfig").Bind(appConfig);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOriginsPolicyName, policy => { policy.WithOrigins(appConfig.Cors.AllowedOrigins.ToArray()).AllowCredentials().AllowAnyHeader(); });
});

builder.Services.AddSingleton<IMongoDatabaseContext, DatabaseContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "StellarStreamAPI",
        ValidAudience = "AstroNews",
        IssuerSigningKey = new RsaSecurityKey(JWTKeyReader.ReadPrivateKey("private_key.pem"))
    };
});

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ApiKeyValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ApiKeyConsumerValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.RegisterMiddleware = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(AllowedOriginsPolicyName);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
