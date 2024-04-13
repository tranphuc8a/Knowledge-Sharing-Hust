// Framework: .NET
// Technology Stack: Entity Framework

import KnowledgeCategory from '../entities/knowledge-category';

class ViewKnowledgeCategory extends KnowledgeCategory {
    constructor() {
        super();
        this.CategoryName = null;
    }

    init() {
        return new ViewKnowledgeCategory();
    }
}

export default ViewKnowledgeCategory;

