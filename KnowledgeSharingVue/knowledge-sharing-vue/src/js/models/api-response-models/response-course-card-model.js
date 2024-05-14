
import Entity from '../entity.js';

class ResponseCourseCardModel extends Entity{
    constructor() {
        super();
        this.UserItemId = null;
        this.Title = null;
        this.Abstract = null;
        this.Thumbnail = null;
        this.CourseRoleType = null;
        this.CourseRelationId = null;
        this.UserId = null;
        this.Username = null;
        this.FullName = null;
        this.Abstract = null;
    }

    init() {
        return new ResponseCourseCardModel();
    }
}

export default ResponseCourseCardModel;

