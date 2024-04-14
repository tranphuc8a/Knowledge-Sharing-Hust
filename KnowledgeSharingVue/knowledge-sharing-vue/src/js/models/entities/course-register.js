

import Entity from './entity';

class CourseRegister extends Entity {
    constructor() {
        super();
        this.CourseRegisterId = null;
        this.UserId = null;
        this.CourseId = null;
    }

    init() {
        return new CourseRegister();
    }
}

module.exports = CourseRegister;
