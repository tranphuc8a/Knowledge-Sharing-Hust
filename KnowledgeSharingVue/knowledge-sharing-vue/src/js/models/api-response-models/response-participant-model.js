
import Entity from '../entity.js';

class ResponseParticipantModel extends Entity{
    constructor() {
        super();
        this.UserConversationId = null;
        this.UserId = null;
        this.ConversationId = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
        this.Nickname = null;
        this.Time = null;
        this.LastReadTime = null;
    }

    init() {
        return new ResponseParticipantModel();
    }
}

export default ResponseParticipantModel;
