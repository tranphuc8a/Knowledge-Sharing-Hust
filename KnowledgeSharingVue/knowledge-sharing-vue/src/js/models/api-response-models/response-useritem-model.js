
import UserItem from '../useritem-model';

class ResponseUserItemModel extends UserItem {
    constructor() {
        super();
        this.AverageStar = null;
        this.MyStar = null;
        this.TotalStar = null;
    }

    init() {
        return new ResponseUserItemModel();
    }
}

export default ResponseUserItemModel;