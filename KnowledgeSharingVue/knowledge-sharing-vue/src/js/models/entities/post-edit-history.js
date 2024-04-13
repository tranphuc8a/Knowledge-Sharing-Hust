
import Entity from './entity';
class PostEditHistory extends Entity{
    constructor() {
        super();
        this.postEditHistoryId = null;
        this.postId = null;
        this.title = null;
        this.abstract = null;
        this.thumbnail = null;
        this.content = null;
    }

    init() {
        return new PostEditHistory();
    }
}

export default PostEditHistory;