

import CourseLesson from '../course-lesson-model';
import ResponseCourseModel from './response-course-model';
import ResponseLessonModel from './response-lesson-model';

class ResponseCourseLessonModel extends CourseLesson {
    constructor() {
        super();
        this.Course = null;
        this.Lesson = null;
    }

    init() {
        return new ResponseCourseLessonModel();
    }

    copy(entity) {
        super.copy(entity);
        this.Course = entity.Course? new ResponseCourseModel().copy(entity.Course) : null;
        this.Lesson = entity.Lesson? new ResponseLessonModel().copy(entity.Lesson) : null;
        return this;
    }
}

export default ResponseCourseLessonModel;
