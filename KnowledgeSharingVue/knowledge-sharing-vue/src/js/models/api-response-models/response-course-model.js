
import ViewCourse from "../views/view-course";
import ResponseCommentModel from "./response-comment-model";

class ResponseCourseModel extends ViewCourse {
    constructor() {
        super();
        this.NumberComments = null;
        this.TopComments = null;
        this.IsMarked = null;
        this.AverageStar = null;
        this.MyStars = null;
        this.TotalStar = null;
        this.Role = null;
        this.Categories = [];
        this.CourseRoleType = null;
        this.CourseRelationId = null;
    }

    init() {
        return new ResponseCourseModel();
    }

    copy(entity) {
        super.copy(entity);
        if (this.TopComments?.length > 0){
            this.TopComments = this.TopComments.map(function(com){
                let comm = new ResponseCommentModel();
                comm.copy(com);
                return comm;
            });
        }
        return this;
    }
}

export default ResponseCourseModel;
