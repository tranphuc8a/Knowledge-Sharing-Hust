// This is a C# code snippet

import CourseRelation from "../entities/course-relation";
import ResponseUserCardModel from "./response-user-card-model";
import ResponseCourseCardModel from "./response-course-card-model";
import { myEnum } from "@/js/resources/enum";

class ResponseCourseRelationModel extends CourseRelation {
    constructor() {
        super();
        this.User = null;
        this.Course = null;
    }

    init() {
        return new ResponseCourseRelationModel();
    }

    copy(entity){
        super.copy(entity);
        if (this.User != null){
            this.User = new ResponseUserCardModel().copy(this.User);
            this.User.CourseRelationId = this.CourseRelationId;
            if (this.CourseRelationType == myEnum.ECourseRelationType.Request){
                this.User.CourseRoleType = myEnum.ECourseRoleType.Requesting;
            } else if (this.CourseRelationType == myEnum.ECourseRelationType.Invite){
                this.User.CourseRoleType = myEnum.ECourseRoleType.Invited;
            }
        }
        if (this.Course != null){
            this.Course = new ResponseCourseCardModel().copy(this.Course);
        }
        return this;
    }
}

export default ResponseCourseRelationModel;

