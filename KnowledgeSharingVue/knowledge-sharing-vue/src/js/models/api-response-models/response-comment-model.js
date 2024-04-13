
import ViewComment from '../view-models/view-comment-model';
class ResponseCommentModel extends ViewComment {
    constructor() {
        super();
        this.averageStars = null;
        this.myStars = null;
        this.totalStars = null;
        this.totalReplies = null;
    }

    init() {
        return new ResponseCommentModel();
    }
}

export default ResponseCommentModel;

