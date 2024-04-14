
import Post from './post';

class Lesson extends Post {
    constructor() {
        super();
        this.EstimateTimeInMinutes = null;
    }

    init() {
        return new Lesson();
    }
}

export default Lesson;