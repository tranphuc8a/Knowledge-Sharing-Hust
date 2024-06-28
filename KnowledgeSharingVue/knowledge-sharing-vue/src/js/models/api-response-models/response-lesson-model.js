

import ViewLesson from "../views/view-lesson";
import ResponseCommentModel from "./response-comment-model";

class ResponseLessonModel extends ViewLesson {
    constructor() {
        super();
        this.AverageStar = null;
        this.MyStars = null;
        this.TotalStar = null;
        this.NumberComments = null;
        this.TopComments = null;
        this.IsMarked = null;
        this.Categories = [];
    }

    init() {
        return new ResponseLessonModel();
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


export default ResponseLessonModel;