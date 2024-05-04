
import ViewComment from "../views/view-comment";
class ResponseCommentModel extends ViewComment {
    constructor() {
        super();
        this.AverageStars = null;
        this.MyStars = null;
        this.TotalStars = null;
        this.TotalReplies = null;
    }

    init() {
        return new ResponseCommentModel();
    }
}

export default ResponseCommentModel;

