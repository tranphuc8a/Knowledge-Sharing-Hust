

import Entity from './entity';
class Star extends Entity{
    constructor() {
        super();
        this.StarId = null;
        this.UserId = null;
        this.UserItemId = null;
        this.Stars = null;
    }

    init() {
        return new Star();
    }
}

export default Star;