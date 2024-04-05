/* eslint-disable */

import { MyDate } from "@/js/utils/mydate";
import { Validator } from "@/js/utils/validator";
import { myEnum } from "@/js/resources/enum";

class Entity{

    /*
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

    /*
    * Khởi tạo các giá trị mặc định của Entity
    * @param none
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    initProperties(){
        try {
            this.keys = ["CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate"];
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Trả về một đối tượng table row từ đối tượng hiện tại
    * @param none
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    toTableObject(){
        try {
            let tableObj = {};
            for (let key of this.keys){
                tableObj[`t${key}`] = this[key];
            }
            this.formatEntityTable(tableObj);
            tableObj.employee = this;
            return tableObj;
        } catch (error){
            console.error(error);
        }
    }


    /**
     * Ánh xạ tới đối tượng visual
     * Cặp phương thức fromEntityVisual và toEntityVisual
     * @Created PhucTV (28/1/24)
     * @Modified None
     */
    toEntityVisual(){
        let visualObj = {};
        for (let key of this.keys){
            visualObj[key] = this[key];
        }
        this.formatEntityVisual(visualObj);
        visualObj.employee = this;
        return visualObj;
    }
    fromEntityVisual(entity){
        try {
            if (Validator.isEmpty(entity)) {
                throw new Error("Visual Object is null");
            }
            for (let key of this.keys){
                this[key] = entity[key];
            }
            this.formatData();
            return this;
        } catch (error){
            console.error(error);
        }
    }

    /**
     * Định dạng dữ liệu cho Entity Visual
     * @param {*} obj đối tượng cần định dạng
     * @return none
     * @Created PhucTV (28/1/24)
     * @Modified None
     */
    formatEntityVisual(obj){
        obj.CreatedDate = this.formatDate(obj.CreatedDate);
        obj.ModifiedDate = this.formatDate(obj.ModifiedDate);
    }

    /**
     * Thực hiện format lại đối tượng của table
     * @param {*} tableObj - đối tượng cần format 
     * @Created PhucTV (23/1/24)
     * @Modified None
     */
    formatEntityTable(tableObj){
        tableObj.tCreatedDate = this.formatDate(tableObj.tCreatedDate);
        tableObj.tModifiedDate = this.formatDate(tableObj.tModifiedDate);
    }


    /*
    * Clone dữ liệu từ một table object
    * @param {*} tableObject - đối tượng table được lấy dữ liệu
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    fromTableObject(tableObject){
        try {
            if (Validator.isEmpty(tableObject)) {
                throw new Error("tableObject is null");
            }
            for (let key of this.keys){
                this[key] = tableObject[`t${key}`];
            }
            this.formatData();
            return this;
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Trả về một đối tượng entity form từ đối tượng hiện tại
    * @param none
    * @Author TVPhuc (19/12/23)
    * @Edit None
    **/
    toEntityForm(){
        try {
            let entityForm = {};
            for (let key of this.keys){
                entityForm[key] = this[key];
            }
            this.formatEntityForm(entityForm);
            return entityForm;
        } catch (error){
            console.error(error);
        }
    }

    /**
     * hàm thực hiện định dạng lại dữ liệu của formObj
     * @param {*} entityForm 
     * @Created PhucTV (23/1/24)
     * @Modified None
     */
    formatEntityForm(entityForm){
        entityForm['CreatedDate'] = this.formatDate(entityForm['CreateDate'], false);
        entityForm['ModifiedDate'] = this.formatDate(entityForm['ModifiedDate'], false);
    }

    /*
    * Clone dữ liệu từ một table object
    * @param {*} entityForm - đối tượng table được lấy dữ liệu
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    fromEntityForm(entityForm){
        try {
            if (Validator.isEmpty(entityForm)) {
                throw new Error("entityForm is null");
            }
            for (let key of this.keys){
                this[key] = entityForm[key];
            }
            this.formatData();
            return this;
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Kiếm tra một giá trị của trường có rỗng hay không
    * @param none
    * @Author TVPhuc (19/12/23)
    * @Edit None
    **/
    isEmptyField(value){
        try {
            value = String(value).trim();
            return value === "" || value === "null" || value === "undefined";
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Định dạng lại dữ liệu
    * @param none
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    formatData(){
        try {
            for (let key of this.keys){
                this[key] = String(this[key]).trim();
                if (this.isEmptyField(this[key])){
                    this[key] = "";
                }
            }
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Clone dữ liệu từ entityApi
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
            };
            this.formatData();
            return this;
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Clone dữ liệu sang entityApi Obj
    * @param none
    * @Author TVPhuc (19/12/23)
    * @Edit None
    **/
    toEntityApi(){
        try{
            let apiObj = {};
            for (let key of this.keys){
                apiObj[key] = this[key];
            };
            this.formatEntityApi(apiObj);
            return apiObj;
        } catch (error){
            console.error(error);
        }
    }

    /**
     * Định dạng lại dữ liệu của apiObj
     * @param {*} apiObj 
     * @Created PhucTV (23/1/24)
     * @Modified None
     */
    formatEntityApi(apiObj){
        apiObj.CreatedDate = this.formatDate(apiObj.CreatedDate, false);
        apiObj.ModifiedDate = this.formatDate(apiObj.ModifiedDate, false);
    }

    /*
    * Trả về định dạng của date
    * @param {*} date - date cần định dạng
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    formatDate(date, isMyDateFormat = true){
        try {
            date = String(date).trim();
            if (date === "" || date === "null" || date === "undefined"){
                return null;
            }
            if (isMyDateFormat){
                return new MyDate(date).toMyDateFormat();
            }
            return new MyDate(date).toNormalFormat();
        } catch (error){
            console.error(error);
            return null;
        }
    }

    /*
    * Định dạng lại tiền
    * @param {*} money - string/number tiền
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    formatMoney(moneys){
        try {
            if (moneys === "") {
                return "";
            }
            let money = Number(moneys);
            // return money.toLocaleString('it-IT', {style : 'currency', currency : 'VND'});
            return money.toLocaleString('it-IT', {style : 'currency', currency : 'VND'});
        } catch (error){
            console.error(error);
        }
    }

}

export { Entity };