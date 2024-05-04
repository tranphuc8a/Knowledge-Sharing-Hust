using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Connection { get; set; }

        /// <summary>
        /// Lưu thay đổi của DbContext
        /// </summary>
        /// <returns> Số bản ghi ảnh hưởng </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        int SaveChanges();

        /// <summary>
        /// Map tới Entry của một Entity trong DbContext
        /// </summary>
        /// <typeparam name="TEntity"> Kiểu của entity </typeparam>
        /// <param name="entity"> Đối tượng </param>
        /// <returns></returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        public DatabaseFacade Database { get; }

        /// <summary>
        /// Lưu thay đổi của DbContext bất đồng bộ
        /// </summary>
        /// <returns> Số bản ghi ảnh hưởng </returns>
        /// Created: PhucTV (20/3/24)
        /// Modified: None
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Khởi động transation cho dbContext
        /// </summary>
        /// <returns> Đối tượng Transaction được tạo </returns>
        /// Created: PhucTV (24/3/24)
        /// Modified: None
        Task<IDbContextTransaction> BeginTransaction();

        

        //DbSet<Block> Blocks { get; set; }
        //DbSet<Follow> Follows {get; set;}
        //DbSet<Friend> Friends {get; set;}
        //DbSet<RequestFriend> RequestFriends {get; set;}

        // KnowledgeDbContext
        DbSet<Knowledge> Knowledges {get; set;}
        DbSet<Category> Categories {get; set;}
        DbSet<KnowledgeCategory> KnowledgeCategories {get; set;}
        DbSet<ViewKnowledgeCategory> ViewKnowledgeCategories {get; set;}
        DbSet<Comment> Comments {get; set;}
        DbSet<ViewComment> ViewComments {get; set;}
        DbSet<Mark> Marks {get; set;}
        DbSet<StudyProgress> StudyProgresses {get; set;}
        
        // CourseDbContext
        DbSet<Course> Courses {get; set;}
        DbSet<ViewCourse> ViewCourses {get; set;}
        DbSet<CourseLesson> CourseLessons {get; set;}
        DbSet<CoursePayment> CoursePayments {get; set;}
        DbSet<ViewCoursePayment> ViewCoursePayments {get; set;}
        DbSet<CourseRegister> CourseRegisters {get; set;}
        DbSet<ViewCourseRegister> ViewCourseRegisters {get; set;}
        DbSet<CourseRelation> CourseRelations {get; set;}
        
        // PostDbContext
        DbSet<Post> Posts {get; set;}
        DbSet<ViewPost> ViewPosts {get; set;}
        DbSet<PostEditHistory> PostEditHistories {get; set;}
        DbSet<Lesson> Lessons {get; set;}
        DbSet<ViewLesson> ViewLessons {get; set;}
        DbSet<Question> Questions {get; set;}
        DbSet<ViewQuestion> ViewQuestions {get; set;}
        
        // UserGeneralDbContext
        DbSet<User> Users {get; set;}
        DbSet<Session> Sessions {get; set;}
        DbSet<Profile> Profiles {get; set;}
        DbSet<ViewUser> ViewUsers { get; set;}
        DbSet<Notification> Notifications {get; set;}

        // UserRelationDbContext
        DbSet<ViewMessage> ViewMessages { get; set; }
        DbSet<ViewUserConversation> ViewUserConversations { get; set;}
        DbSet<UserConversation> UserConversations {get; set;}
        DbSet<Conversation> Conversations {get; set;}
        DbSet<Message> Messages {get; set;}
        DbSet<UserRelation> UserRelations {get; set;}
        DbSet<ViewUserRelation> ViewUserRelations {get; set;}
        
        // UserItemDbContext
        DbSet<UserItem> UserItems {get; set;}
        DbSet<Star> Stars {get; set;}
        DbSet<Image> Images { get; set; }

    }
}
