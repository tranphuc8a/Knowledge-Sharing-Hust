// Framework: .NET Core
// Technology Stack: Entity Framework

import Lesson from "../entities/lesson";

class ViewLesson extends Lesson {
    constructor() {
        super();
        this.Username = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
        this.TotalStar = null;
        this.SumStar = null;
        this.AverageStar = null;
        this.TotalComment = null;
    }

    init() {
        return new ViewLesson();
    }

}

export default ViewLesson;