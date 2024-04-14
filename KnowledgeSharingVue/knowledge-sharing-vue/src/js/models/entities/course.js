
import Knowledge from './knowledge';

class Course extends Knowledge {
    constructor() {
        super();
        this.Introduction = null;
        this.Fee = null;
        this.EstimateTimeInMinutes = null;
        this.IsFree = null;
    }
    init() {
        return new Course();
    }
}

module.exports = Course;