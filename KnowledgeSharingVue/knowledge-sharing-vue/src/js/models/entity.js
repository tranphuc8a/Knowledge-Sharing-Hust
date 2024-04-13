import Validator from "@/js/utils/validator";

class Entity {
    constructor() {
        this.CreatedBy = null;
        this.CreatedTime = null;
        this.ModifiedBy = null;
        this.ModifiedTime = null;
    }


    getProperties() {
        return Object.getOwnPropertyNames(this);
    }

    init() {
        return new Entity();
    }

    clone() {
        const entity = this.init();
        const properties = this.getProperties();
        properties.forEach((prop) => {
            entity[prop] = this[prop];
        });
        return entity;
    }

    copy(entity) {
        if (Validator.isEmpty(entity)) {
            return;
        }
        const properties = this.getProperties();
        properties.forEach((prop) => {
            if (prop in entity) {
                this[prop] = entity[prop];
            }
        });
        return this;
    }
}

export default Entity;