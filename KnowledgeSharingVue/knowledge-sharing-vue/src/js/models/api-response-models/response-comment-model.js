
import ViewComment from "../views/view-comment";
class ResponseCommentModel extends ViewComment {
    constructor() {
        super();
        this.AverageStars = null;
        this.MyStars = null;
        this.TotalStars = null;
        this.TotalReplies = null;
        this.Reply = null;
    }

    init() {
        return new ResponseCommentModel();
    }

    copy(entity){
        super.copy(entity);
        if (this.Reply != null){
            this.Reply = new ResponseCommentModel().copy(this.Reply);
        }
        return this;
    }
}

export default ResponseCommentModel;

