using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Infrastructures.Caches;
using KnowledgeSharingApi.Infrastructures.DbContexts;
using KnowledgeSharingApi.Infrastructures.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories;
using KnowledgeSharingApi.Infrastructures.Storages;
using KnowledgeSharingApi.Services.Interfaces;
using KnowledgeSharingApi.Services.Middlewares;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;



#region Cấu hình builder
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

#region Cấu hình chung

// Add Controllers:
builder.Services.AddControllers();
// Cấu hình tắt response trả về thay đổi chữ hoa chữ thường
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession();


// Sử dụng memory cache:
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<MySqlDbContext>(); 
#endregion

#region Cấu hình Authentication Bearer Jwt cho Builder
// Thêm xác thực (authentication) bằng Bearer Jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? ""))
    };
});
#endregion

#region Cấu hình chính sách Cors
// add CORS Policy:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
#endregion


#endregion

#region Cấu hình Builder Dependency Injection

// Injection Infrastructures
builder.Services.AddScoped<ICache, MemoryCache>();
builder.Services.AddScoped<IDbContext, MySqlDbContext>();
builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddScoped<IStorage, FirebaseStorage>();


// Injection Repositories
builder.Services.AddScoped<IUserRepository, UserMySqlRepository>();


// Injection Services
builder.Services.AddScoped<IUserService, UserService>();


// Injection Domains
builder.Services.AddScoped<IResourceFactory, ViResourceFactory>();



#endregion


#region Cấu hình Application

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();


// Sử dụng middleware tùy chỉnh
// Custom Middle Ware
app.UseMiddleware<DefaultExceptionMiddleware>();
app.UseMiddleware<ResponseExceptionMiddleware>();
app.UseMiddleware<ValidatorExceptionMiddleware>();


app.UseCors("AllowAnyOrigin");
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); 
#endregion
