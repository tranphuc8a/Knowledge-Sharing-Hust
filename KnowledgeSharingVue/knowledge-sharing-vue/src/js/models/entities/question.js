
import Post from './post';

class Question extends Post {
    constructor() {
        super();
        this.CourseId = null;
        this.IsAccept = null;
    }

    init() {
        return new Question();
    }
}

export default Question;
