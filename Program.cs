using Azure.Storage.Blobs;
using MediPortal_AuthService.Data;
using MediPortal_AuthService.Extensions;
using MediPortal_AuthService.Models;
using MediPortal_AuthService.Services;
using MediPortal_AuthService.Services.IService;
using MediPortal_AuthService.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});

//register azure blob service
builder.Services.AddScoped( _ =>
{
    return new BlobServiceClient(builder.Configuration.GetConnectionString("blobstorage"));
});
//register identity framework
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IJwtInterface, JwtService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddCors(options => options.AddPolicy("policy", build =>
{
    build.AllowAnyOrigin();
    build.AllowAnyMethod();
    build.AllowAnyHeader();

}));
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    if (!app.Environment.IsDevelopment())
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AUTH API");
        c.RoutePrefix = string.Empty;
    }
});
app.UseMigration();
app.UseCors("policy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
