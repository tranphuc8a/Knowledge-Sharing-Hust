import Entity from "../entity";


class ViewUTotalUserStar extends Entity {
    constructor() {
        super();
        this.UserId = null;
        this.TotalStar = null;
        this.SumStar = null;
    }

    init() {
        return new ViewUTotalUserStar();
    }
}

export default ViewUTotalUserStar;
