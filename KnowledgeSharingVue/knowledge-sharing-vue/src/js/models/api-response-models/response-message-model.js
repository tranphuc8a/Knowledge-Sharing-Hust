
import ViewMessage from "../views/view-message";

class ResponseMessageModel extends ViewMessage {
    constructor() {
        super();
        this.ReplyMessage = null;
    }

    init() {
        return new ResponseMessageModel();
    }

    copy(entity){
        super.copy(entity);
        if (this.ReplyMessage != null) {
            this.ReplyMessage = new ResponseMessageModel().copy(this.ReplyMessage);
        }
        return this;
    }
}

export default ResponseMessageModel;
