import Entity from "./entity";

class Session extends Entity {
    constructor() {
        super();
        this.SessionId = null;
        this.UserId = null
        this.RefreshToken = null;
        this.Expired = null;
        this.Time = null;
        this.Place = null;
        this.Device = null;
    }

    init() {
        return new Session();
    }
}

export default Session;