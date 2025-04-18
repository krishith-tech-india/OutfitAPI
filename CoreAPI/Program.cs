using Core;
using Core.CustomExceptionFilter;
using Mapper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Repo;
using Service;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Outfit API", Version = "v1" });
});

IConfigurationSection appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

var emailConfigurationSection = builder.Configuration.GetSection("EmailConfigurations");
builder.Services.Configure<EmailConfigurations>(emailConfigurationSection);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(appSettings!.ClientList)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

builder.Services.InjectDBContextDependencies(builder.Configuration.GetConnectionString("Dev")!);
builder.Services.InjectRepoDependencies();
builder.Services.InjectServiceDependencies();
builder.Services.InjectMapperDependnecies();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Version V1");
    c.RoutePrefix = "swagger";
});

//}

app.UseCors(MyAllowSpecificOrigins);

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
