// Technology Stack: C# 

import UserConversation from '../entities/user-conversation';

class ViewUserConversation extends UserConversation {
    constructor() {
        super();
        this.FullName = "";
        this.Avatar = null;
        this.Cover = null;
    }

    init() {
        return new ViewUserConversation();
    }
}

export default ViewUserConversation;
