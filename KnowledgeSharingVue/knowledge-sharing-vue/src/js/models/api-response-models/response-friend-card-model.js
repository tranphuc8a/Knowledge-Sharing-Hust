

import Entity from '../entity.js';

class ResponseFriendCardModel extends Entity{
    constructor() {
        super();
        this.FriendId = null;
        this.UserId = null;
        this.Username = null;
        this.FullName = null;
        this.Email = null;
        this.Avatar = null;
        this.IsActive = null;
        this.Time = null;
        this.UserRelationType = null;
    }

    init() {
        return new ResponseFriendCardModel();
    }
}

export default ResponseFriendCardModel;
