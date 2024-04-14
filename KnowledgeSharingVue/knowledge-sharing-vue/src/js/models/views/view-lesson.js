// Framework: .NET Core
// Technology Stack: Entity Framework

import Lesson from './lesson';

class ViewLesson extends Lesson {
    constructor() {
        super();
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
    }

    init() {
        return new ViewLesson();
    }
}

export default ViewLesson;