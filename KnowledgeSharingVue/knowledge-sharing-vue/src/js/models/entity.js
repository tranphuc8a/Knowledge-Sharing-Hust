import Validator from "@/js/utils/validator";

class Entity {
    constructor() {
        this.CreatedBy = null;
        this.CreatedTime = null;
        this.ModifiedBy = null;
        this.ModifiedTime = null;
    }


    /**
     * Lấy ra các thuộc tính của Entity
     * @param none
     * @returns {Array} - mảng chứa các tên thuộc tính của Entity
     * @Author TVPhuc (13/04/24)
     * @Edit None
     **/
    getProperties() {
        return Object.getOwnPropertyNames(this);
    }


    /**
     * Khởi tạo một entity mặc định
     * @param none
     * @returns {Entity} - entity mặc định
     * @Author TVPhuc (13/04/24)
     * @Edit None
     */
    init() {
        return new Entity();
    }

    /**
     * Clone một entity
     * @param none
     * @returns {Entity} - entity được clone
     * @Author TVPhuc (13/04/24)
     * @Edit None
     */
    clone() {
        const entity = this.init();
        const properties = this.getProperties();
        properties.forEach((prop) => {
            entity[prop] = this[prop];
        });
        return entity;
    }

    /**
     * Copy dữ liệu từ entity khác
     * @param {*} entity - entity cần copy dữ liệu
     * @returns {Entity} - entity được copy dữ liệu
     * @Author TVPhuc (13/04/24)
     * @Edit None
     */
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