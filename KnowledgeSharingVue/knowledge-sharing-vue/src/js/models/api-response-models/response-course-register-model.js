
import ViewCourseRegister from "../views/view-course-register";

class ResponseCourseRegisterModel extends ViewCourseRegister{
    constructor() {
        super();
        this.UserRelationType = null;
        this.UserRelationId = null;
    }

    init() {
        return new ResponseCourseRegisterModel();
    }
}

export default ResponseCourseRegisterModel;
