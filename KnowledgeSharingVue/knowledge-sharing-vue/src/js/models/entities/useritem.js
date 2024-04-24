// Technology stack: C# Entity Framework
import Entity from "../entity";

class UserItem extends Entity {
    constructor() {
        super();
        this.UserItemId = null;
        this.UserId = null;
        this.UserItemType = null;
    }

    init() {
        return new UserItem();
    }
}

export default UserItem;