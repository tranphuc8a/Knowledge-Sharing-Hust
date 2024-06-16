// Technology Stack: C# 

import UserConversation from '../entities/user-conversation';

class ViewUserProfileConversation extends UserConversation {
    constructor() {
        super();
        this.Username = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
    }

    init() {
        return new ViewUserProfileConversation();
    }
}

export default ViewUserProfileConversation;
