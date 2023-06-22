using MicroservicesExample.Order.AsyncDataServices;
using MicroservicesExample.Order.Data;
using MicroservicesExample.Order.EventProcessing;
using MicroservicesExample.Order.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
// Add services to the container.

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(configuration["ConnectionStrings:RedisConnection"])
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
         options.UseSqlite(@"Data Source=order.db"));

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
    {
        //x.Authority = configuration["Urls:IdentityUrl"];
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    });

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddHostedService<MessageBusSubscriber>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var userDbContext = scope.ServiceProvider.GetService<AppDbContext>();

    var isCreated = userDbContext?.Database.EnsureCreated();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
