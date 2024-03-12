using KnowledgeSharingApi.Domains.Models.Entities;
using Microsoft.EntityFrameworkCore;
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
        public IDbConnection Connection { get; set; }

        //public DbSet<Block> Blocks { get; set; }
        public DbSet<Category> Categoríes {get; set;}
        public DbSet<Comment> Comments {get; set;}
        public DbSet<Conservation> Conservations {get; set;}
        public DbSet<Course> Courses {get; set;}
        public DbSet<CourseLesson> CourseLessons {get; set;}
        public DbSet<CoursePayment> CoursePayments {get; set;}
        public DbSet<CourseRegister> CourseRegisters {get; set;}
        public DbSet<CourseRelation> CourseRelations {get; set;}
        //public DbSet<Follow> Follows {get; set;}
        //public DbSet<Friend> Friends {get; set;}
        public DbSet<Knowledge> Knowledges {get; set;}
        public DbSet<KnowledgeCategory> KnowledgeCategories {get; set;}
        public DbSet<Lesson> Lessons {get; set;}
        public DbSet<Mark> Marks {get; set;}
        public DbSet<Message> Messages {get; set;}
        public DbSet<Notification> Notifications {get; set;}
        public DbSet<Post> Posts {get; set;}
        public DbSet<PostEditHistory> PostEditHistories {get; set;}
        public DbSet<Profile> Profiles {get; set;}
        public DbSet<Question> Questions {get; set;}
        //public DbSet<RequestFriend> RequestFriends {get; set;}
        public DbSet<Session> Sessions {get; set;}
        public DbSet<Star> Stars {get; set;}
        public DbSet<StudyProgress> StudyProgresses {get; set;}
        public DbSet<User> Users {get; set;}
        public DbSet<UserConservation> UserConservations {get; set;}
        public DbSet<UserItem> UserItems {get; set;}
        public DbSet<UserRelation> UserRelations {get; set;}

    }
}
