
import Entity from './entity';

class CourseRelation extends Entity {
    constructor() {
        super();
        this.CourseRelationId = null;
        this.SenderId = null;
        this.ReceiverId = null;
        this.CourseId = null;
        this.CourseRelationType = null;
    }

    init() {
        return new CourseRelation();
    }
}

export default CourseRelation;