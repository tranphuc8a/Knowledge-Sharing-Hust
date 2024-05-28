// Framework: .NET
// Technology Stack: Entity Framework

import UserRelation from '../entities/user-relation';

class ViewUserRelation extends UserRelation {
    constructor() {
        super();

        this.SenderEmail = null;
        this.SenderUsername = null;
        this.SenderRole = null;
        this.SenderName = null;
        this.SenderAvatar = null;
        this.SenderCover = null;

        this.ReceiverEmail = null;
        this.ReceiverUsername = null;
        this.ReceiverRole = null;
        this.ReceiverName = null;
        this.ReceiverAvatar = null;
        this.ReceiverCover = null;
    }

    init() {
        return new ViewUserRelation();
    }
}

export default ViewUserRelation;
