
import ViewCourse from '../view-models/view-course-model';

class ResponseCourseModel extends ViewCourse {
    constructor() {
        super();
        this.NumberComments = null;
        this.TopComments = null;
        this.IsMarked = null;
        this.AverageStars = null;
        this.MyStars = null;
        this.TotalStars = null;
        this.Role = null;
    }

    init() {
        return new ResponseCourseModel();
    }
}

export default ResponseCourseModel;
