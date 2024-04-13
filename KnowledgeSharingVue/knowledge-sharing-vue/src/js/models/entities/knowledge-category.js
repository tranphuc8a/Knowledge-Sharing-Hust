
import Entity from './entity';

class KnowledgeCategory extends Entity {
    constructor() {
        super();
        this.knowledgeCategoryId = null;
        this.knowledgeId = null;
        this.categoryId = null;
    }

    init() {
        return new KnowledgeCategory();
    }
}

export default KnowledgeCategory;