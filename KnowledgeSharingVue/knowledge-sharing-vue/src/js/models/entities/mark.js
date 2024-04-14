
import Entity from './entity.js';
class Mark extends Entity{
    constructor() {
        super();
        this.MarkId = null;
        this.UserId = null;
        this.KnowledgeId = null;
    }

    init() {
        return new Mark();
    }
}

export default Mark;
