
import Entity from './entity';

class KnowledgeCategory extends Entity {
    constructor() {
        super();
        this.KnowledgeCategoryId = null;
        this.KnowledgeId = null;
        this.CategoryId = null;
    }

    init() {
        return new KnowledgeCategory();
    }
}

export default KnowledgeCategory;