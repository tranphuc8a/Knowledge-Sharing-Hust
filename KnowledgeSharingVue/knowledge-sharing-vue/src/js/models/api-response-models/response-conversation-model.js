

import Conversation from '../conversation-model';

class ResponseConversationModel extends Conversation {
    constructor() {
        super();
        this.Participants = null;
        this.Messages = null;
    }

    init() {
        return new ResponseConversationModel();
    }
}

export default ResponseConversationModel;
