
import { Validator } from "@/js/utils/validator";
import ViewUser from "../views/view-user";
class CurrentUser extends ViewUser {
    static _instance = null;

    constructor() {
        super();
    }

    /**
     * Lấy ra đối tượng người dùng hiện tại (Singleton)
     * @param none
     * @returns {CurrentUser}
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    static async getInstance(){
        try {
            if(!this._instance){
                let storageUser = await localStorage.getItem('currentUser');
                storageUser = JSON.parse(storageUser)
                if (Validator.isEmpty(storageUser)) return null;
                this._instance = new CurrentUser();
                this._instance.copy(storageUser);
            }
            return this._instance;
        } catch (error){
            console.error(error);
            return null;
        }
        
    }

    /**
     * Lưu đối tượng người dùng hiện tại (Singleton) vao local storage
     * @param user - đối tượng người dùng cần lưu
     * @returns none
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    static async setInstance(user){
        try {
            if (user != null && this._instance != null){
                this._instance.copy(user);
            }
            await localStorage.setItem('currentUser', JSON.stringify(user));
        } catch (e) {
            console.error(e);
        }
    }

    /**
     * Xóa đối tượng người dùng hiện tại (Singleton) khỏi local storage
     * @param none
     * @returns none
     * @Created PhucTV (20/5/24)
     * @Modified None
     */
    static async deleteInstance(){
        try {
            await localStorage.removeItem('currentUser');
            this._instance = null;
        } catch (e) {
            console.error(e);
        }
    }

    init() {
        return new CurrentUser();
    }
}

export default CurrentUser;
