
import Entity from "../entity";

class Image extends Entity{
    constructor() {
        super();
        this.ImageId = null;
        this.UserId = null;
        this.ImageUrl = null;
    }
    
    init() {
        return new Image();
    }
}

export default Image;
