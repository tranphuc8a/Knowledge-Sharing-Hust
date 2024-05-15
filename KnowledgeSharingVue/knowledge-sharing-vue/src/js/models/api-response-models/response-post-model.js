
import ViewPost from '../view-models/view-post-model';
import ResponseCommentModel from './response-comment-model';
import Category from '../entities/category';

class ResponsePostModel extends ViewPost {
    constructor() {
        super();
        this.NumberComments = null;
        this.TopComments = null;
        this.IsMarked = null;
        this.AverageStar = null;
        this.MyStar = null;
        this.TotalStar = null;
        this.Categories = [];
    }

    init() {
        return new ResponsePostModel();
    }

    copy(entity) {
        super.copy(entity);
        if (this.TopComments?.length > 0){
            this.TopComments = this.TopComments.map(function(com){
                let comm = new ResponseCommentModel();
                comm.copy(com);
                return comm;
            });
        }
        if (this.Categories?.length > 0){
            this.Categories = this.Categories.map(function(cat){
                let cate = new Category();
                cate.copy(cat);
                return cate;
            });
        }
        return this;
    }
}

export default ResponsePostModel;
