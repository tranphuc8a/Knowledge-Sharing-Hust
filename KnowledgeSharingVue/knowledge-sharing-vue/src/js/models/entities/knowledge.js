
import UserItem from './useritem';

class Knowledge extends UserItem {
    constructor() {
        super();
        this.Title = null;
        this.Abstract = null;
        this.Thumbnail = null;
        this.Views = null;
        this.KnowledgeType = null;
        this.Privacy = null;
        this.IsBlockComment = null;
    }

    init() {
        return new Knowledge();
    }
}

export default Knowledge;