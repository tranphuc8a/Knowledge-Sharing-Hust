
import Star from '../star-model';

class ResponseStarModel extends Star {
    constructor() {
        super();
        this.User = null;
        this.Item = null;
    }

    init() {
        return new ResponseStarModel();
    }
}

export default ResponseStarModel;

