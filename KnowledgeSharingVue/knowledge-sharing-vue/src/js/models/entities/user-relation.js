

import Entity from './entity.js';

class UserRelation extends Entity {
    constructor() {
        super(); 
        this.userRelationId = null; 
        this.senderId = null;
        this.receiverId = null;
        this.userRelationType = null;
        this.time = null;
    }

    init() {
        return new UserRelation();
    }
}


export default UserRelation; 