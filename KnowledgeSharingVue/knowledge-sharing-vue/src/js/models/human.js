/* eslint-disable */
import { myEnum } from "@/js/resources/enum";
import { Entity } from "./entity";
import { MyDate } from "@/js/utils/mydate";

class Human extends Entity{
    constructor(apiObj) {
        super(apiObj);
    }

    initProperties(){
        super.initProperties();
        this.keys = [...this.keys, "FullName", "DateOfBirth", "Address", "Email", "Gender", "PhoneNumber", 
                                    "IdentityCardNumber", "DateOfIdentityCard", "PlaceOfIdentityCard"];
    }

    formatEntityTable(tableObj){
        super.formatEntityTable(tableObj);
        tableObj.tDateOfBirth = this.formatDate(tableObj.tDateOfBirth);
        tableObj.tDateOfIdentityCard = this.formatDate(tableObj.tDateOfIdentityCard);
        tableObj.tGender = this.formatGender(tableObj.tGender);
        if (tableObj.tGender !== null){
            tableObj.tGender = Number(tableObj.tGender);
            let genderText = {
                0: "Nam",
                1: "Nữ",
                2: "Khác"
            };
            if ([0, 1, 2].includes(tableObj.tGender)){
                tableObj.tGender = genderText[tableObj.tGender];
            }
        }
    }

    formatEntityVisual(obj){
        super.formatEntityVisual(obj);
        obj.DateOfBirth = this.formatDate(obj.DateOfBirth);
        obj.Gender = this.formatGender(obj.Gender);
        if (obj.Gender !== null){
            obj.Gender = Number(obj.Gender);
            let mapGender = {
                0: "Nam", 1: "Nữ", 2: "Khác"
            };
            obj.Gender = mapGender[obj.Gender];
        }
        obj.DateOfIdentityCard = this.formatDate(obj.DateOfIdentityCard);
    }

    formatEntityForm(formObj){
        super.formatEntityForm(formObj);
        formObj.DateOfBirth = this.formatDate(formObj.DateOfBirth, false);
        formObj.DateOfIdentityCard = this.formatDate(formObj.DateOfIdentityCard, false);
    }

    formatEntityApi(apiObj){
        super.formatEntityApi(apiObj);
        apiObj.DateOfBirth = this.formatDate(apiObj.DateOfBirth, false);
        apiObj.DateOfIdentityCard = this.formatDate(apiObj.DateOfIdentityCard, false);
    }

    formatData(){
        super.formatData();
        this.Gender = this.formatGender(this.Gender);
        return this;
    }

    /*
    * Định dạng lại giới tính
    * @param {*} gender - string/number giới tính
    * @Author TVPhuc (28/11/23)
    * @Edit None
    **/
    formatGender(gender){
        if (gender === null || gender === undefined){
            return null;
        }
        try {
            gender = String(gender).trim();
            if (gender === String(myEnum.gender.FEMALE) || gender === "Nữ" || gender === "Female"){
                return myEnum.gender.FEMALE;
            } else if (gender === String(myEnum.gender.MALE) || gender === "Nam" || gender === "Male"){
                return myEnum.gender.MALE;
            } else if (gender === String(myEnum.gender.OTHER) || gender === "Khác" || gender === "Other") {
                return myEnum.gender.OTHER;
            } else if (gender === "" || gender === "null" || gender === "undefined"){
                return null;
            } else {
                throw new Error(`Gender is not invalid: ${this.gender}`);
            }
        } catch (error){
            console.error(error);
            return null;
        }
    }
}


export {Human};