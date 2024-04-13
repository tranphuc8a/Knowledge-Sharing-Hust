

class ViewComment extends Comment {
    constructor() {
        super();
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
    }

    init() {
        return new ViewComment();
    }
}

export default ViewComment;
