
import Entity from '../entity.js';

class ResponseCourseCardModel extends Entity{
    constructor() {
        super();
        this.UserItemId = null;
        this.Title = null;
        this.Abstract = null;
        this.Thumbnail = null;
        this.Role = null;
    }

    init() {
        return new ResponseCourseCardModel();
    }
}

export default ResponseCourseCardModel;

