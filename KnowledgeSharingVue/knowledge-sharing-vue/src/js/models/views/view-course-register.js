// Framework: .NET
// Technology stack: C#, Entity Framework
import CourseRegister from '../entities/course-register';

class ViewCourseRegister extends CourseRegister {
    constructor() {
        super();
        this.Username = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
        this.Thumbnail = null;
        this.Title = null;
        this.Abstract = null;
        this.CourseOwnerUserId = null;
        this.CourseOwnerUsername = null;
        this.CourseOwnerFullName = null;
        this.CourseOwnerAvatar = null;
        this.CourseOwnerCover = null;
    }

    init() {
        return new ViewCourseRegister();
    }

    getCourse(){
        return {
            UserItemId: this.CourseId,
            Title: this.Title,
            Abstract: this.Abstract,
            Thumbnail: this.Thumbnail
        }
    }

    getOwner(){
        return {
            UserId: this.CourseOwnerUserId,
            Username: this.CourseOwnerUsername,
            FullName: this.CourseOwnerFullName,
            Avatar: this.CourseOwnerAvatar,
            Cover: this.CourseOwnerCover
        }
    }
}

export default ViewCourseRegister;
