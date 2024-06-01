
import Entity from "../entity";

class Conversation extends Entity {
    constructor() {
        super();
        this.ConversationId = null;
        this.ConversationName = null;
        this.Thumbnail = null;
    }
    
    init() {
        return new Conversation();
    }
}

export default Conversation;
