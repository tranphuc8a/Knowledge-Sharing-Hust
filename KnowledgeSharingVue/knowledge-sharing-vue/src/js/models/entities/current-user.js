
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
        if(!this._instance){
            let storageUser = await localStorage.getItem('currentUser');
            this._instance = new CurrentUser();
            this._instance.copy(JSON.parse(storageUser));
        }
        return this._instance;
    }

    /**
     * Lưu đối tượng người dùng hiện tại (Singleton) vao local storage
     * @param user - đối tượng người dùng cần lưu
     * @returns none
     * @Created PhucTV (22/2/24)
     * @Modified None
     */
    static async setInstance(user){
        this._instance.copy(user);
        await localStorage.setItem('currentUser', user);
    }

    init() {
        return new CurrentUser();
    }
}

export default CurrentUser;
