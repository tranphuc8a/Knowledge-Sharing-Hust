

import Entity from './entity.js';

class UserRelation extends Entity {
    constructor() {
        super(); 
        this.UserRelationId = null; 
        this.SenderId = null;
        this.ReceiverId = null;
        this.UserRelationType = null;
        this.Time = null;
    }

    init() {
        return new UserRelation();
    }
}


export default UserRelation; 