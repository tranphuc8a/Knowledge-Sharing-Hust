// Framework and Technology Stack: C# Entity Framework

import CoursePayment from '../entities/course-payment';

class ViewCoursePayment extends CoursePayment {
    constructor() {
        super();
        this.fullName = null;
        this.avatar = null;
        this.cover = null;
        this.title = null;
        this.thumbnail = null;
    }

    init() {
        return new ViewCoursePayment();
    }
}

export default ViewCoursePayment;

