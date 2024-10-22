// Technology stack: C#, .NET


import ViewQuestion from '../views/view-question';
import ResponseCommentModel from './response-comment-model';

class ResponseQuestionModel extends ViewQuestion {
    constructor() {
        super();
        this.NumberComments = null;
        this.TopComments = null;
        this.IsMarked = null;
        this.AverageStar = null;
        this.MyStars = null;
        this.TotalStar = null;
        this.Categories = [];

        this.myCourse = null;
    }

    init() {
        return new ResponseQuestionModel();
    }

    copy(entity) {
        super.copy(entity);
        if (this.TopComments?.length > 0){
            this.TopComments = this.TopComments.map(function(com){
                let comm = new ResponseCommentModel();
                comm.copy(com);
                return comm;
            });
        }
        return this;
    }
}

export default ResponseQuestionModel;
