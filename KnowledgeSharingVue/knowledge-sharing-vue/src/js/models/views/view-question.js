// This is a C# source code snippet

import Question from '../entities/question';

class ViewQuestion extends Question {
    constructor() {
        super();
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
    }

    init() {
        return new ViewQuestion();
    }
}

export default ViewQuestion;
