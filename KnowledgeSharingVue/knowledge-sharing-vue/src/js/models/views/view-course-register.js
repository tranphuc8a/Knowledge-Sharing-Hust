// Framework: .NET
// Technology stack: C#, Entity Framework
import CourseRegister from '../entities/course-register';

class ViewCourseRegister extends CourseRegister {
    constructor() {
        super();
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
        this.Thumbnail = null;
        this.Title = null;
        this.Abstract = null;
        this.CourseOwnerUserId = null;
        this.CourseOwnerFullName = null;
        this.CourseOwnerAvatar = null;
        this.CourseOwnerCover = null;
    }

    init() {
        return new ViewCourseRegister();
    }
}

export default ViewCourseRegister;
