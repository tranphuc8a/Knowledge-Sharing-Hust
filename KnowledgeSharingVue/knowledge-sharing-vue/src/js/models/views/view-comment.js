
import Comment from "../entities/comment";

class ViewComment extends Comment {
    constructor() {
        super();
        this.Username = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
        this.Role = null;
    }

    init() {
        return new ViewComment();
    }

}

export default ViewComment;
