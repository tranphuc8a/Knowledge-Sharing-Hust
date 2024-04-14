

import CourseLesson from '../course-lesson-model';

class ResponseCourseLessonModel extends CourseLesson {
    constructor() {
        super();
        this.Course = null;
        this.Lesson = null;
    }

    init() {
        return new ResponseCourseLessonModel();
    }
}

export default ResponseCourseLessonModel;
