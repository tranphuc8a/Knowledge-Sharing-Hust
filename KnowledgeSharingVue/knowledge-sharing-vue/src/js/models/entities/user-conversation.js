
import Entity from './entity.js';

class UserConversation extends Entity{
    constructor() {
        super();
        this.UserConversationId = null;
        this.UserId = null;
        this.ConversationId = null;
        this.Nickname = null;
        this.Time = null;
        this.LastReadTime = null;
        this.LastDeleteTime = null;
    }

    init() {
        return new UserConversation();
    }
}

export default UserConversation;

