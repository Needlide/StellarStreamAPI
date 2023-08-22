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
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

string AllowedOriginsPolicyName = "AllowedSpecificOrigins";
string AppConfigConnectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Services.AddScoped<IMongoDatabaseContext, DatabaseContext>();
builder.Services.AddTransient<IEncryptor, SymmetricEncryptor>();

builder.Services.AddControllers();

builder.Configuration.AddUserSecrets<Program>();
Aes aes = Aes.Create();
builder.Configuration.GetSection("Security")["EncoderKey"] = aes.Key.ToString();

builder.Configuration.AddAzureAppConfiguration(AppConfigConnectionString);
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("StellarStreamAppConfig"));
var appConfig = new AppConfig();
builder.Configuration.GetSection("StellarStreamAppConfig").Bind(appConfig);

builder.Services.AddCors(options =>
{
    //options.AddPolicy(name: AllowedOriginsPolicyName, policy => { policy.WithOrigins(appConfig.Cors.AllowedOrigins.ToArray()).AllowCredentials().AllowAnyHeader(); });
    options.AddPolicy("CorsDebugPolicy", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "StellarStreamAPI",
        ValidAudience = "AstroNews",
        IssuerSigningKey = new RsaSecurityKey(JWTKeyReader.ReadPublicKey("public_key.pem"))
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = "You are not authorized." });
            return context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthorizationDebugPolicy", policy => { policy.RequireAuthenticatedUser(); });
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

builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

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
