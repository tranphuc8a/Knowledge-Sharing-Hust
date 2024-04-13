

import Entity from './entity';
class CoursePayment extends Entity {
    constructor() {
        super();
        this.coursePaymentId = null;
        this.userId = null;
        this.courseId = null;
        this.fee = 0;
        this.paymentMethod = null;
    }

    init() {
        return new CoursePayment();
    }
}

export default CoursePayment;