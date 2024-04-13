

import Entity from './entity';
class CoursePayment extends Entity {
    constructor() {
        super();
        this.CoursePaymentId = null;
        this.UserId = null;
        this.CourseId = null;
        this.Fee = 0;
        this.PaymentMethod = null;
    }

    init() {
        return new CoursePayment();
    }
}

export default CoursePayment;