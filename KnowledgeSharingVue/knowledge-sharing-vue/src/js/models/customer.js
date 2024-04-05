/* eslint-disable */

import { MyDate } from "@/js/utils/mydate";
import { Validator } from "@/js/utils/validator";
import { myEnum } from "@/js/resources/enum";

class Customer{

    /*
    * Khởi tạo một customer mặc định hoặc từ customer Api
    * @param {option} customerApi - đối tượng customer được lấy về từ API
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    constructor(customerApi){
        try {
            this.initProperties();
            if (customerApi){
                return this.fromCustomerApi(customerApi);
            }
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Khởi tạo các giá trị mặc định của Input
    * @param none
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    initProperties(){
        try {
            this.keys = ["guid", "id", "name", "dob", "gender", "phone", "email", 
                         "cccd", "cccd-date", "cccd-place", "debt", "company", "address"];
            for (let key of this.keys) {
                this[key] = '';
            }
            this.id = "00000";
            this.name = "Nguyễn Văn A";
            this.gender = "Nam";
            this.dob = "20/11/2023";
            this.company = "Công ty cổ phần MISA";
            this.debt = "10000";
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
                tableObj[key] = this[key];
            }
            let tableKeys = ['id', 'name', 'gender', 'dob', 'debt'];
            for (let key of tableKeys){
                tableObj[`t${key}`] = tableObj[key];
            }
            tableObj['tdebt'] = this.formatDebt(tableObj['tdebt']);
            tableObj['tgender'] = this.formatGender(tableObj['tgender']);
            return tableObj;
        } catch (error){
            console.error(error);
        }
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
                this[key] = tableObject[key];
            }
            return this;
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Trả về một đối tượng customer form từ đối tượng hiện tại
    * @param none
    * @Author TVPhuc (19/12/23)
    * @Edit None
    **/
    toCustomerForm(){
        try {
            let customerForm = {};
            for (let key of this.keys){
                customerForm[key] = this[key];
            }
            if (Validator.isNotEmpty(customerForm['dob'])){
                customerForm['dob'] = new MyDate(customerForm['dob']).toNormalFormat();
            }
            return customerForm;
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Clone dữ liệu từ một table object
    * @param {*} customerForm - đối tượng table được lấy dữ liệu
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    fromCustomerForm(customerForm){
        try {
            if (Validator.isEmpty(customerForm)) {
                throw new Error("customerForm is null");
            }
            for (let key of this.keys){
                this[key] = customerForm[key];
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
            this.dob = this.formatDate(this.dob);
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Clone dữ liệu từ customerApi
    * @param {*} customerApi - đối tượng customer từ Api cần lấy dữ liệu
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    fromCustomerApi(customerApi){
        try{
            if (Validator.isEmpty(customerApi)) {
                throw new Error("Customer from api is null");
            }
            let apiKeys = ['CustomerId', 'CustomerCode', 'FullName', 'DateOfBirth', 'Gender', 'PhoneNumber', 'Email',
                            '', '', '', 'DebitAmount', 'CompanyName', 'Address'];
            for (let i = 0; i < this.keys.length; i++){
                this[this.keys[i]] = customerApi[apiKeys[i]];
            }
            this.formatData();
            return this;
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Clone dữ liệu sang customerApi Obj
    * @param none
    * @Author TVPhuc (19/12/23)
    * @Edit None
    **/
    toCustomerApi(){
        try{
            let apiObj = {};
            let apiKeys = ['CustomerId', 'CustomerCode', 'FullName', 'DateOfBirth', 'Gender', 'PhoneNumber', 'Email',
                            '', '', '', 'DebitAmount', 'CompanyName', 'Address'];
            let key = '';
            for (let i = 0; i < apiKeys.length; i++){
                key = apiKeys[i];
                if (Validator.isEmpty(key)){
                    continue;
                }
                apiObj[key] = this[this.keys[i]];
                if (Validator.isEmpty(apiObj[key])){
                    delete apiObj[key];
                }
            }
            if (Validator.isNotEmpty(apiObj['DateOfBirth'])){
                apiObj['DateOfBirth'] = new MyDate(apiObj['DateOfBirth']).toNormalFormat();
            }
            switch (String(apiObj['Gender'])){
                case "Nam":
                    apiObj['Gender'] = myEnum.gender.MALE;
                    break;
                case "Nữ":
                    apiObj['Gender'] = myEnum.gender.FEMALE;
                    break;
                case "Khác":
                    apiObj['Gender'] = myEnum.gender.OTHER;
                    break;
                default:
                    // do nothing
                    break;
            }
            return apiObj;
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Trả về định dạng của date
    * @param {*} date - date cần định dạng
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    formatDate(date){
        try {
            date = String(date).trim();
            if (date === "" || date === "null" || date === "undefined"){
                return "";
            }
            return new MyDate(date).toMyDateFormat();
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Định dạng lại tiền nợ
    * @param {*} debt - string/number tiền nợ
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    formatDebt(debt){
        try {
            if (debt === "") {
                return "";
            }
            let money = Number(debt);
            // return money.toLocaleString('it-IT', {style : 'currency', currency : 'VND'});
            return money.toLocaleString('it-IT', {style : 'currency', currency : 'VND'});
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Định dạng lại giới tính
    * @param {*} gender - string/number giới tính
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    formatGender(gender){
        try {
            gender = String(gender).trim();
            if (gender === String(myEnum.gender.FEMALE) || gender === "Nữ"){
                return "Nữ";
            } else if (gender === String(myEnum.gender.MALE) || gender === "Nam"){
                return "Nam";
            } else if (gender === String(myEnum.gender.OTHER) || gender === "Khác") {
                return "Khác";
            } else if (gender === "" || gender === "null" || gender === "undefined"){
                return "";
            } else {
                throw new Error(`Gender is not invalid: ${this.gender}`);
            }
        } catch (error){
            console.error(error);
            return null;
        }
    }
}

export {Customer};