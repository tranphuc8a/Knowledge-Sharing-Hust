using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.DbContexts
{
    public class MySqlDbContext : DbContext, IDbContext
    {
        //private readonly IConfiguration Configuration = configuration;
        //private readonly string MySqlVersion = "10.4.32-MariaDB";
        private readonly string ConnectionString;
        public IDbConnection Connection { get; set; }
        public MySqlDbContext(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:MariaDb"] ?? String.Empty;
            Connection = new MySqlConnection(ConnectionString);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override void Dispose()
        {
            Connection?.Dispose();
            base.Dispose();
        }

        public override int SaveChanges() => base.SaveChanges();

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await base.Database.BeginTransactionAsync();
        }

        //public DbSet<Block> Blocks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseLesson> CourseLessons { get; set; }
        public DbSet<CoursePayment> CoursePayments { get; set; }
        public DbSet<CourseRegister> CourseRegisters { get; set; }
        public DbSet<CourseRelation> CourseRelations { get; set; }
        //public DbSet<Follow> Follows {get; set;}
        //public DbSet<Friend> Friends {get; set;}
        public DbSet<Knowledge> Knowledges { get; set; }
        public DbSet<KnowledgeCategory> KnowledgeCategories { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostEditHistory> PostEditHistories { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Question> Questions { get; set; }
        //public DbSet<RequestFriend> RequestFriends {get; set;}
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Star> Stars { get; set; }
        public DbSet<StudyProgress> StudyProgresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserConversation> UserConversations { get; set; }
        public DbSet<UserItem> UserItems { get; set; }
        public DbSet<UserRelation> UserRelations { get; set; }
        public DbSet<ViewUserRelation> ViewUserRelations { get; set; }
        public DbSet<ViewUser> ViewUsers { get; set; }
        public DbSet<ViewMessage> ViewMessages { get; set; }
        public DbSet<ViewUserConversation> ViewUserConversations { get; set; }
        public DbSet<ViewComment> ViewComments { get; set; }
        public DbSet<ViewPost> ViewPosts { get; set; }
        public DbSet<ViewCourse> ViewCourses { get; set; }
        public DbSet<ViewLesson> ViewLessons { get; set; }
        public DbSet<ViewQuestion> ViewQuestions { get; set; }
        public DbSet<ViewCourseRegister> ViewCourseRegisters { get; set; }
        public DbSet<ViewKnowledgeCategory> ViewKnowledgeCategories { get; set; }
    }
}
