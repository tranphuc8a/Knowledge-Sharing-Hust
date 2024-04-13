
import Entity from './entity';
class PostEditHistory extends Entity{
    constructor() {
        super();
        this.PostEditHistoryId = null;
        this.PostId = null;
        this.Title = null;
        this.Abstract = null;
        this.Thumbnail = null;
        this.Content = null;
    }

    init() {
        return new PostEditHistory();
    }
}

export default PostEditHistory;