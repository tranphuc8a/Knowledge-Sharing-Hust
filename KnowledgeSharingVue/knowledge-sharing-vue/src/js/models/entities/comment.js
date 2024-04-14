

import UserItem from './useritem.js';
class Comment extends UserItem {
    constructor() {
        super();
        this.KnowledgeId = null;
        this.Content = null;
        this.ReplyId = null;
    }
    
    init() {
        return new Comment();
    }
}

export default Comment;
