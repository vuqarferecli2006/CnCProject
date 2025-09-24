using CnC.Application.Shared.Helpers.PermissionHelpers;
using CnC.Application.Features.User.Commands.Register;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CnC.Infrastructure.Services;
using CnC.Percistance.Contexts;
using Microsoft.OpenApi.Models;
using CnC.WepApi.MiddleWare;
using Hangfire.PostgreSql;
using CnC.Domain.Entities;
using CnC.Percistance;
using System.Text;
using Hangfire;
using MediatR;
using Nest;
using FluentValidation.AspNetCore;
using FluentValidation;
using CnC.Application.Validations.BasketValidations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });



builder.Services.AddValidatorsFromAssembly(typeof(AddBasketCommandRequestValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters(); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // JWT Authentication üçün Swagger konfiqurasiyas?
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token daxil edin Meselen: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new string[] {}
        }
    });
});


builder.Services.AddHttpClient<ICurrencyService, CurrencyService>();


builder.Services.AddDbContext<AppDbContext>(options=> 
        options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddHangfire(configuration =>
    configuration.UseStorage(
        new PostgreSqlStorage(
            builder.Configuration.GetConnectionString("Default"),
            new PostgreSqlStorageOptions
            {
                SchemaName = "hangfire",
                QueuePollInterval = TimeSpan.FromSeconds(15),
                InvisibilityTimeout = TimeSpan.FromMinutes(5)
                // Burada əlavə ayarlar da əlavə edə bilərsən
            })));

builder.Services.AddHangfireServer();

builder.Services.AddIdentity<AppUser, IdentityRole>(
options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(typeof(RegisterUserCommandRequest).Assembly);

builder.Services.Configure<JwtSetting>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.Configure<EmailSetting>(
    builder.Configuration.GetSection("EmailSetting"));

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMQ"));

builder.Services.Configure<GoogleSetting>(
    builder.Configuration.GetSection("Google"));

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.Configure<FaceBookSetting>(
    builder.Configuration.GetSection("Facebook"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSetting>();

builder.Services.AddAuthorization(options =>
{
    foreach (var permission in PermissionHelper.GetPermissionList())
    {
        options.AddPolicy(permission, policy =>
            policy.RequireClaim("Permission", permission));
    }
});

var elasticUri = builder.Configuration["ElasticsearchSettings:Uri"] ?? "http://localhost:9201";

builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var settings = new ConnectionSettings(new Uri(elasticUri))
        .DefaultIndex("products");

    return new ElasticClient(settings);
});
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
})
.AddGoogle("Google", googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Google:ClientSecret"];
})
.AddFacebook("Facebook", options =>
{
    options.AppId = builder.Configuration["Facebook:AppId"];
    options.AppSecret = builder.Configuration["Facebook:AppSecret"];
});

builder.Services.RegisterServices();

//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

var app = builder.Build();

//app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard("/hangfire");
}

RecurringJob.AddOrUpdate<CurrencyUpdateJob>(
    "currency-update-job", 
    job => job.UpdateRatesAsync(), 
    "0 6 * * *", 
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    }
);
app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseMiddleware<BlacklistTokenMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
