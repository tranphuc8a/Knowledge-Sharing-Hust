
import Entity from '../entity.js';

class ResponseUserCardModel extends Entity {
    constructor() {
        super();
        this.UserId = null;
        this.Username = null;
        this.Avatar = null;
        this.Cover = null;
        this.FullName = null;
        this.Role = null;
        this.UserRelationType = null;
        this.UserRelationId = null;
    }

    init() {
        return new ResponseUserCardModel();
    }
}

export default ResponseUserCardModel;
