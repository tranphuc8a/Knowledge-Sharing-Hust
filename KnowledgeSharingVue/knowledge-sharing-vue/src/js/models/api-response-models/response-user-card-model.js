
import Entity from '../entity.js';

class ResponseUserCardModel extends Entity {
    constructor() {
        super();
        this.UserId = null;
        this.Username = null;
        this.Email = null;
        this.PhoneNumber = null;
        this.Avatar = null;
        this.Cover = null;
        this.FullName = null;
        this.Role = null;
        this.Bio = null;
        this.UserRelationType = null;
        this.UserRelationId = null;
        this.CourseRoleType = null;
        this.CourseRelationId = null;
    }

    init() {
        return new ResponseUserCardModel();
    }
}

export default ResponseUserCardModel;
