import { Validator } from "@/js/utils/validator";
import { MyDate } from "../utils/mydate";

class Entity {
    constructor() {
        this.CreatedBy = null;
        this.CreatedTime = null;
        this.ModifiedBy = null;
        this.ModifiedTime = null;
        this.ListObservers = [];
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
        if (this.CreatedTime != null){
            this.CreatedTime = new MyDate(this.CreatedTime);
        }
        if (this.ModifiedTime != null){
            this.ModifiedTime = new MyDate(this.ModifiedTime);
        }
        if (this.User == null || this.User == undefined){
            this.User = this.getUser();
        }
        return this;
    }

    /**
     * Tra ve user cua mot entity
     * @param none
     * @returns {Object} - trả về một object chứa thông tin của người dùng
     * @Created PhucTV (12/5/24)
     * @Modified None
     */
    getUser(){
        return {
            UserId: this.UserId,
            Username: this.Username,
            FullName: this.FullName,
            Avatar: this.Avatar,
            Cover: this.Cover,
            Role: this.Role,
            UserRelationType: this.UserRelationType,
            UserRelationId: this.UserRelationId,
        };
    }

    /**
     * Dang ky mot observer
     * @param {Function} callback - hàm callback
     * @returns none
     * @Created PhucTV (12/5/24)
     * @Modified None
     */
    registerObserver(callback){
        if (callback == null) return;
        this.ListObservers.push(callback);
    }

    /**
     * Thông báo cho các observer
     * @param none
     * @returns none
     * @Created PhucTV (12/5/24)
     * @Modified None
     */
    notifyObserver(){
        this.ListObservers.forEach((callback) => {
            callback();
        });
    }
}

export default Entity;