//import { MyDate } from "@/js/utils/mydate";
import { Validator } from "@/js/utils/validator";

class Entity{

    /**
    * Khởi tạo một entity mặc định hoặc từ entity Api
    * @param {option} entityApi - đối tượng entity được lấy về từ API
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    constructor(entityApi){
        try {
            this.initProperties();
            if (entityApi){
                return this.fromEntityApi(entityApi);
            }
        } catch (error){
            console.error(error);
        }
    }

    /**
    * Khởi tạo các giá trị mặc định cho các trường của Entity
    * @param none
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    initProperties(){
        try {
            this.keys = ["CreatedBy", "CreatedTime", "ModifiedBy", "ModifiedTime"];
        } catch (error){
            console.error(error);
        }
    }


    /**
    * Clone dữ liệu từ đối tượng entityApi
    * @param {*} entityApi - đối tượng entity từ Api cần lấy dữ liệu
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    fromEntityApi(entityApi){
        try{
            if (Validator.isEmpty(entityApi)) {
                throw new Error("entity from api is null");
            }
            for (let key of this.keys){
                this[key] = entityApi[key];
            }
            return this;
        } catch (error){
            console.error(error);
        }
    } 

}

export { Entity };