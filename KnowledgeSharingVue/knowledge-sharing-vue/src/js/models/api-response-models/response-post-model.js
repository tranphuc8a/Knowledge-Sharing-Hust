
import ViewPost from '../view-models/view-post-model';

class ResponsePostModel extends ViewPost {
    constructor() {
        super();
        this.NumberComments = null;
        this.TopComments = null;
        this.IsMarked = null;
        this.AverageStars = null;
        this.MyStars = null;
        this.TotalStars = null;
    }

    init() {
        return new ResponsePostModel();
    }
}

export default ResponsePostModel;
