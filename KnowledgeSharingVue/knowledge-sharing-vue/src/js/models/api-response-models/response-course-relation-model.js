// This is a C# code snippet

import CourseRelation from '../course-relation';
class ResponseCourseRelationModel extends CourseRelation {
    constructor() {
        super();
        this.User = null;
        this.Course = null;
    }

    init() {
        return new ResponseCourseRelationModel();
    }
}

export default ResponseCourseRelationModel;

