

import Conversation from '../entities/conversation';
import ResponseMessageModel from './response-message-model';
import ResponseParticipantModel from './response-participant-model';

class ResponseConversationModel extends Conversation {
    constructor() {
        super();
        this.Participants = null;
        this.Messages = null;
    }

    init() {
        return new ResponseConversationModel();
    }

    copy(entity){
        super.copy(entity);
        if (this.Participants === null) {
            this.Participants = [];
        } else {
            this.Participants = this.Participants.map(item => {
                return new ResponseParticipantModel().copy(item);
            });
        }
        if (this.Messages === null) {
            this.Messages = [];
        } else {
            this.Messages = this.Messages.map(item => {
                return new ResponseMessageModel().copy(item);
            });
        }
        return this;
    }
}

export default ResponseConversationModel;
