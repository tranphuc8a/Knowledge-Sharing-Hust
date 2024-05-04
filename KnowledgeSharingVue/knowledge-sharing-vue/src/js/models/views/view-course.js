
import Course from '../entities/course';

class ViewCourse extends Course {
    constructor() {
        super();
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
    }
}

export default ViewCourse;
