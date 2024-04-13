// Framework and Technology Stack: C#

import Profile from './profile';

class ViewUser extends Profile {
    constructor() {
        super();
        this.Email = null;
        this.Username = null;
        this.Role = null;
    }

    init() {
        return new ViewUser();
    }
}

export default ViewUser;

