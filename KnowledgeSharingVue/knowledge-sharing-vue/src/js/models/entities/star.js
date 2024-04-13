

import Entity from './entity';
class Star extends Entity{
    constructor() {
        super();
        this.starId = null;
        this.userId = null;
        this.userItemId = null;
        this.stars = null;
    }

    init() {
        return new Star();
    }
}

export default Star;