// Framework and Technology Stack: C#

import Profile from "../entities/profile";

class ViewUser extends Profile {
    constructor() {
        super();
        this.Email = null;
        this.Username = null;
        this.Role = null;

        this.TotalFriend = null; 
        this.TotalRequester = null; 
        this.TotalRequestee = null; 
        this.TotalFollower = null; 
        this.TotalFolowee = null; 
        this.TotalBlocker = null; 
        this.TotalBlockee = null; 
    }

    init() {
        return new ViewUser();
    }
}

export default ViewUser;

