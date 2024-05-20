// Framework and Technology Stack: C# Entity Framework

import CoursePayment from '../entities/course-payment';

class ViewCoursePayment extends CoursePayment {
    constructor() {
        super();
        this.Username = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
        this.Title = null;
        this.Thumbnail = null;
    }

    init() {
        return new ViewCoursePayment();
    }
}

export default ViewCoursePayment;

