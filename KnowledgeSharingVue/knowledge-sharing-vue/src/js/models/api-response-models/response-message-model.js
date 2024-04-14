
import ViewMessage from './view-message-model';

class ResponseMessageModel extends ViewMessage {
    constructor() {
        super();
        this.ReplyMessage = null;
    }

    init() {
        return new ResponseMessageModel();
    }
}

export default ResponseMessageModel;
