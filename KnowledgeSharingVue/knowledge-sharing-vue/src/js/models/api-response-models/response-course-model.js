
import ViewCourse from "../views/view-course";
import ResponseCommentModel from "./response-comment-model";
import Category from "../entities/category";

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
        if (this.Categories?.length > 0){
            this.Categories = this.Categories.map(function(cat){
                let cate = new Category();
                cate.copy(cat);
                return cate;
            });
        }
        return this;
    }
}

export default ResponseCourseModel;
