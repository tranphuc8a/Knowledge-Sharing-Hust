
import Entity from './entity.js';
class User extends Entity {
    constructor() {
        super();
        this.UserId = null; // Assuming Guid.Empty equivalent in JS is an empty string
        this.Email = null;
        this.Username = null;
        this.HashPassword = null;
        this.Role = null;
    }

    init() {
        return new User();
    }
}

export default User;
