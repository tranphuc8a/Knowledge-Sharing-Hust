

import { Entity } from './entity.js';
class Notification extends Entity{
    constructor() {
        super();
        this.NotificationId = null;
        this.UserId = null;
        this.Thumbnail = null;
        this.Title = null;
        this.Content = null;
        this.ReferenceLink = null;
        this.Time = null;
        this.IsRead = false;
    }

    init() {
        return new Notification();
    }
}

export default Notification;