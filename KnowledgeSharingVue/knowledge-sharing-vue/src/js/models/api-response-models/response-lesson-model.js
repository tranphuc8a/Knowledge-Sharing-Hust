

import ViewLesson from '../view-models/view-lesson.js';

class ResponseLessonModel extends ViewLesson {
    constructor() {
        super();
        this.AverageStars = null;
        this.MyStars = null;
        this.TotalStars = null;
        this.NumberComments = null;
        this.TopComments = null;
        this.IsMarked = null;
    }

    init() {
        return new ResponseLessonModel();
    }
}


export default ResponseLessonModel;