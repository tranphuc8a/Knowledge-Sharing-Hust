
import Knowledge from './knowledge';

class Post extends Knowledge {
    constructor() {
        super();
        this.Content = null;
        this.PostType = null;
    }

    init() {
        return new Post();
    }
}

export default Post;