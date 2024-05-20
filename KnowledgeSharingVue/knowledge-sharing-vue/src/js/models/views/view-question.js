// This is a C# source code snippet

import Question from '../entities/question';

class ViewQuestion extends Question {
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
        return new ViewQuestion();
    }
}

export default ViewQuestion;
