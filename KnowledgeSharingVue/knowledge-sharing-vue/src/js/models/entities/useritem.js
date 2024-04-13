// Technology stack: C# Entity Framework
import Entity from './entity';

class UserItem extends Entity {
    constructor() {
        super();
        this.userItemId = null;
        this.userId = null;
        this.userItemType = null;
    }

    init() {
        return new UserItem();
    }
}

export default UserItem;