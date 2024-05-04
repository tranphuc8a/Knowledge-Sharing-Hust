// Framework: .NET Core
// Technology Stack: Entity Framework

import Lesson from "../entities/lesson";

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

    getUser(){
        return {
            UserId: this.UserId,
            Username: this.Username,
            FullName: this.FullName,
            Avatar: this.Avatar,
            Cover: this.Cover
        }
    }
}

export default ViewLesson;