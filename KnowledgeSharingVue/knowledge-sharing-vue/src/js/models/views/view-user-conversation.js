// Technology Stack: C# 

import UserConversation from '../entities/user-conversation';

class ViewUserConversation extends UserConversation {
    constructor() {
        super();
        this.Username = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
    }

    init() {
        return new ViewUserConversation();
    }
}

export default ViewUserConversation;
