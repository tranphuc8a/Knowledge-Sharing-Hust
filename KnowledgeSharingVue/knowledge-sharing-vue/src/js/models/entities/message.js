
import Entity from "../entity";

class Message extends Entity {
    constructor() {
        super();
        this.MessageId = null;
        this.UserConversationId = null;
        this.Content = null;
        this.Time = null;
        this.ReplyId = null;
        this.IsEdited = null;
    }

    init() {
        return new Message();
    }
}

export default Message;