using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using KnowledgeSharingApi.Infrastructures.Caches;
using KnowledgeSharingApi.Infrastructures.Captcha;
using KnowledgeSharingApi.Infrastructures.DbContexts;
using KnowledgeSharingApi.Infrastructures.Emails;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Caches;
using KnowledgeSharingApi.Infrastructures.Interfaces.Captcha;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Emails;
using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories;
using KnowledgeSharingApi.Infrastructures.Interfaces.Storages;
using KnowledgeSharingApi.Infrastructures.Interfaces.UnitOfWorks;
using KnowledgeSharingApi.Infrastructures.Interfaces.WebSockets;
using KnowledgeSharingApi.Infrastructures.Repositories.DecorationRepositories;
using KnowledgeSharingApi.Infrastructures.Repositories.MySqlRepositories;
using KnowledgeSharingApi.Infrastructures.Storages;
using KnowledgeSharingApi.Infrastructures.UnitOfWorks;
using KnowledgeSharingApi.Infrastructures.WebSockets;
using KnowledgeSharingApi.Options;
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


// Sử dụng memory Cache:
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<MySqlDbContext>();

builder.Services.ConfigureOptions<ConfigSwaggerOptions>();
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

//
// Injection Infrastructures
builder.Services.AddSingleton<ICache, MemoryCache>();
builder.Services.AddSingleton<IEmail, Email>();
builder.Services.AddSingleton<IStorage, FirebaseStorage>();
builder.Services.AddSingleton<IEncrypt, KSEncrypt>();
builder.Services.AddSingleton<ICaptcha, KSCaptcha>();
builder.Services.AddScoped<IDbContext, MySqlDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddSingleton<ISocketSessionManager, SocketSessionManager>();
builder.Services.AddSingleton<INotificationSocketSessionManager, NotificationSocketSessionManager>();
builder.Services.AddSingleton<ILiveChatSocketSessionManager, LiveChatSocketSessionManager>();
builder.Services.AddSingleton<INewMessageNotificationSocketSessionManager, NewMessageNotificationSocketSessionManager>();

//
//
// Injection Repositories
builder.Services.AddScoped<IUserRepository, UserMySqlRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileMySqlRepository>();
builder.Services.AddScoped<ISessionRepository, SessionMySqlRepository>();
builder.Services.AddScoped<IUserRelationRepository, UserRelationMySqlRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationMySqlRepository>();
builder.Services.AddScoped<IKnowledgeRepository, KnowledgeMySqlRepository>();
builder.Services.AddScoped<IPostRepository, PostMySqlRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionMySqlRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationMySqlRepository>();
builder.Services.AddScoped<IMessageRepository, MessageMySqlRepository>();
builder.Services.AddScoped<IUserConversationRepository, UserConversationMySqlRepository>();
builder.Services.AddScoped<ICourseRepository, CourseMySqlRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryMySqlRepository>();
builder.Services.AddScoped<IKnowledgeRepository, KnowledgeMySqlRepository>();
builder.Services.AddScoped<IMarkRepository, MarkMySqlRepository>();
builder.Services.AddScoped<ICommentRepository, CommentMySqlRepository>();
builder.Services.AddScoped<IStarRepository, StarMySqlRepository>();
builder.Services.AddScoped<IUserItemRepository, UserItemMySqlRepository>();
builder.Services.AddScoped<ILessonRepository, LessonMySqlRepository>();
builder.Services.AddScoped<ICourseRegisterRepository, CourseRegisterMySqlRepository>();
builder.Services.AddScoped<ICourseRelationRepository, CourseRelationMySqlRepository>();
builder.Services.AddScoped<ICoursePaymentRepository, CoursePaymentMySqlRepository>();
builder.Services.AddScoped<IDecorationRepository, DecorationRepository>();



//
//
// Injection Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
builder.Services.AddScoped<IRegisterNewUserService, RegisterNewUserService>();
//builder.Services.AddScoped<ISocketService, SocketService>();
builder.Services.AddScoped<INotificationSocketService, NotificationSocketService>();
builder.Services.AddScoped<ILiveChatSocketService, LiveChatSocketService>();
builder.Services.AddScoped<INewMessageNotificationSocketService, NewMessageNotificationSocketService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserRelationService, UserRelationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>(); 
builder.Services.AddScoped<IKnowledgeService, KnowledgeService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IStarService, StarService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICoursePaymentService, CoursePaymentService>();
builder.Services.AddScoped<ICourseLessonService, CourseLessonService>();



//
//
// Injection Domains
builder.Services.AddSingleton<IResourceFactory, ViResourceFactory>();



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

// Config WebSocket
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.UseCors("AllowAnyOrigin");
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); 
#endregion
