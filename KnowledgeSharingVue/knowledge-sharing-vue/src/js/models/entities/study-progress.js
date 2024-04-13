
import Entity from './entity.js';
class StudyProgress extends Entity{
    constructor() {
        super();
        this.StudyProgressId = null;
        this.UserId = null;
        this.KnowledgeId = null;
        this.Progress = null;
    }

    init() {
        return new StudyProgress();
    }
}

export default StudyProgress;