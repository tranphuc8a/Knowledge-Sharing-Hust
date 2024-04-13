// Technology stack: C#, .NET


import ViewQuestion from '../views/view-question';

class ResponseQuestionModel extends ViewQuestion {
    constructor() {
        super();
        this.NumberComments = null;
        this.TopComments = null;
        this.IsMarked = null;
        this.AverageStars = null;
        this.MyStars = null;
        this.TotalStars = null;
    }

    init() {
        return new ResponseQuestionModel();
    }
}

export default ResponseQuestionModel;
