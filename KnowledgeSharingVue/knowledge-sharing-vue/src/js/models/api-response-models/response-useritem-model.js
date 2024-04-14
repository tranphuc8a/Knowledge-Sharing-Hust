
import UserItem from '../useritem-model';

class ResponseUserItemModel extends UserItem {
    constructor() {
        super();
        this.AverageStars = null;
        this.MyStars = null;
        this.TotalStars = null;
    }

    init() {
        return new ResponseUserItemModel();
    }
}

export default ResponseUserItemModel;