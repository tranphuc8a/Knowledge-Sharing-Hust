
import Entity from './entity';
class CourseLesson extends Entity{
    constructor() {
        super();
        this.CourseLessonId = null;
        this.CourseId = null;
        this.LessonId = null;
        this.Offset = null;
        this.LessonTitle = null;
    }

    init() {
        return new CourseLesson();
    }
}

export default CourseLesson;