
import ViewCourse from "../views/view-course";

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
