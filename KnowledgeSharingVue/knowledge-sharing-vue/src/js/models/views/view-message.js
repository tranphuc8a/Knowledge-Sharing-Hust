// Framework: .NET Core
// Technology stack: C#

import Message from './message';

class ViewMessage extends Message {
    constructor() {
        super();
        this.Username = null;
        this.UserId = null;
        this.ConversationId = null;
        this.Nickname = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
    }

    init() {
        return new ViewMessage();
    }
}

export default ViewMessage;

