import Entity from './entity.js';

class Category extends Entity {
    constructor() {
        super();
        this.CategoryId = null;
        this.CategoryName = null;
    }

    init() {
        return new Category();
    }
}

export default Category;
