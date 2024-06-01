
import { language } from "../resources/language";
import { MyDate } from "./mydate";

// Lớp này quản lý tất cả những logic về validate input
class Validator {
    /**
    * Trả về một validator
    * @param none
    * @Author TVPhuc (30/11/23)
    * @Edit None
    **/
    constructor(errorMsg){
        this.setErrorMsg(errorMsg);
        this.langType = 'vi';
        this.messages = language['vi'].validator.messages;
        this.fields = language['vi'].validator.fields;
        this.entities = language['vi'].validator.entities;
        this.msgEmpty ??= this.errorMsg;
        this.isAcceptEmpty = true;
    }

    /**
     * Đặt valildator là có cho phép giá trị empty hay không
     * @param {*} isAcceptEmpty - true: có cho phép empty, false: không cho phép empty 
     * @returns true - hợp lệ, false - không hợp lệ
     * @Created PhucTV (5/3/24)
     * @Modified None
     */
    setIsAcceptEmpty(isAcceptEmpty = true, msgEmpty = null){
        this.isAcceptEmpty = isAcceptEmpty;
        this.msgEmpty = msgEmpty ?? this.msgEmpty;
        return this;
    }

    /**
    * Kiểm tra xem một giá trị có khác rỗng hay không 
    * @param {*} value
    * @Author TVPhuc (28/11/23)
    * @Edit TVPhuc (30/11/23) - chuyển thành static và đưa vào trong lớp Validator
    **/
    static isNotEmpty(val){
        try {
            if (val === null || val === undefined || val === ""){
                return false;
            } 
            return true;
        } catch (error){
            console.error(error);
        }
    }

    /**
    * Kiểm tra xem một giá trị có là rỗng hay không 
    * @param {*} value - giá trị cần kiểm tra
    * @Author TVPhuc (02/12/23)
    * @Edit 
    **/
    static isEmpty(val){
        try {
            if (val === null || val === undefined || (typeof val === "string" && val === "")){
                return true;
            } 
            return false;
        } catch (error){
            console.error(error);
        }
    }

    /**
    * Kiểm tra xem một giá trị có là rỗng hay không 
    * @param {*} value - giá trị cần kiểm tra
    * @Author TVPhuc (02/12/23)
    * @Edit 
    **/
    static isEmptyOrSpace(val){
        try {
            if (val === null || val === undefined || (typeof val === "string" && val === "")){
                return true;
            }
            return false;
        } catch (error){
            console.error(error);
        }
    }

    /**
    * abtract method để validate dữ liệu 
    * @Param {*} value
    * @Author TVPhuc (30/11/23)
    * @Edit none 
    * @Output {
    *   isValid: true/false,
    *   msg: string
    * } 
    **/
    validate(value){
        if (!this.isAcceptEmpty && Validator.isEmpty(value)){
            return {
                isValid: false,
                msg: this.msgEmpty
            }
        }
        return {
            isValid: true,
            msg: this.errorMsg
        }
    }

    /**
     * Đặt errorMessage cho validator
     * @param {*} errorMsg - errorMessage cần đặt
     * @returns none
     * @Created PhucTV (20/2/24)
     * @Modified None 
     */
    setErrorMsg(errorMsg){
        this.errorMsg = errorMsg;
    }

}

class RegexValidator extends Validator{
    constructor(errorMsg){
        super(errorMsg ?? "Regex Validator");
    }
    validate(val){
        try {
            if (Validator.isNotEmpty(val)) {
                if (!this.regex.test(val)){
                    return {
                        isValid: false,
                        msg: this.errorMsg
                    };
                }
            }
            return super.validate(val);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            };
        }
    }
}

class DateValidator extends RegexValidator{
    constructor(errorMsg){
        super(errorMsg ?? "Date Validator");
        this.regex = /^(0?[1-9]|[12][0-9]|3[01])\/(0?[1-9]|1[0-2])\/(?!0000)\d{4}$/;
    }
}

class NotEmptyValidator extends Validator{
    constructor(errorMsg){
        super(errorMsg ?? "Not Empty Validator");
    }
    validate(value){
        try {
            if (!Validator.isNotEmpty(value)) {
                return {
                    isValid: false,
                    msg: this.errorMsg
                };
            }
            return super.validate(value);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            };
        }
    }
}

class PositiveNumberValidator extends Validator{
    constructor(errorMsg){
        super(errorMsg ?? "Positive Number Validator");
    }
    validate(value){
        try {
            if (!Validator.isNotEmpty(value) && value > 0) {
                return {
                    isValid: false,
                    msg: this.errorMsg
                };
            }
            return super.validate(value);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            };
        }
    }
}

class RangeNumberValidator extends Validator{
    constructor(errorMsg){
        super(errorMsg ?? "Positive Number Validator");
        this.min = null;
        this.max = null;
    }

    setBoundary(min, max){
        this.min = min;
        this.max = max;
        return this;
    }

    validate(value){
        try {
            if (Validator.isNotEmpty(value)) {
                if (this.min != null && value < this.min){
                    return {
                        isValid: false,
                        msg: this.errorMsg
                    };
                }
                if (this.max != null && value > this.max){
                    return {
                        isValid: false,
                        msg: this.errorMsg
                    };
                }
            }
            return super.validate(value);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            };
        }
    }
}


class UsernameValidator extends RegexValidator{
    constructor(errorMsg){
        super(errorMsg ?? "Username Validator");
        this.regex = /^[a-zA-Z][a-zA-Z0-9_]{3,15}$/;
    }
    
    validate(val){
        try {
            if (Validator.isEmpty(val)) {
                return {
                    isValid: false,
                    msg: this.errorMsg
                };
            }
            return super.validate(val);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            };
        }
    }
}

class PasswordValidator extends RegexValidator{
    constructor(errorMsg){
        super(errorMsg ?? "Password Validator");
        this.regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[a-zA-Z\d@$!%*?&]{8,}$/;
    }

    validate(val){
        try {
            if (Validator.isEmpty(val)) {
                return {
                    isValid: false,
                    msg: this.errorMsg
                };
            }
            return super.validate(val);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            };
        }
    }
}

class PhoneValidator extends RegexValidator{
    constructor(errorMsg){
        super(errorMsg ?? "Phone Validator");
        // eslint-disable-next-line
        // this.regex = /^(1[ \-\+]{0,3}|\+1[ -\+]{0,3}|\+1|\+)?((\(\+?1-[2-9][0-9]{1,2}\))|(\(\+?[2-8][0-9][0-9]\))|(\(\+?[1-9][0-9]\))|(\(\+?[17]\))|(\([2-9][2-9]\))|([ \-\.]{0,3}[0-9]{2,4}))?([ \-\.][0-9])?([ \-\.]{0,3}[0-9]{2,4}){2,3}$/;
        this.regex = /^.*$/;
    }
}

class UrlValidator extends RegexValidator{
    constructor(errorMsg){
        super(errorMsg ?? "Url Validator");
        // eslint-disable-next-line
        // this.regex = /^(1[ \-\+]{0,3}|\+1[ -\+]{0,3}|\+1|\+)?((\(\+?1-[2-9][0-9]{1,2}\))|(\(\+?[2-8][0-9][0-9]\))|(\(\+?[1-9][0-9]\))|(\(\+?[17]\))|(\([2-9][2-9]\))|([ \-\.]{0,3}[0-9]{2,4}))?([ \-\.][0-9])?([ \-\.]{0,3}[0-9]{2,4}){2,3}$/;
        // eslint-disable-next-line
        this.regex = /https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)/;
    }
}

class EmailValidator extends RegexValidator{
    constructor(errorMsg){
        super(errorMsg ?? "Email Validator");
        this.regex = /^[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9_-]+)*\.[a-zA-Z]{2,}$/;
    }
}

class IdentityCardNumberValidator extends RegexValidator{
    constructor(errorMsg){
        super(errorMsg ?? "Identity Card Validator");
        this.regex = /^[0-9]{9}|[0-9]{12}$/;
    }
}

class MoneyValidator extends RegexValidator{
    constructor(errorMsg){
        super(errorMsg ?? "Money Validator");
        this.regex = /^\$?[\d,]+(\.\d*)?$/;
    }
}

class RepasswordValidator extends Validator{
    constructor(errorMsg){
        super(errorMsg ?? "Repassword Validator");
        this.originPassword = "";
    }

    /**
     * Hàm đặt mật khẩu gốc cần so khớp
     * @param {*} originPassword - mật khẩu gốc cần so khớp 
     * @returns this
     * @Created PhucTV (04/03/24)
     * @Modified None
     */
    setOriginPassword(originPassword){
        this.originPassword = originPassword;
        return this;
    }

    validate(repassword){
        try {
            if (repassword !== this.originPassword) {
                return {
                    isValid: false,
                    msg: this.errorMsg
                };
            }
            return super.validate(repassword);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            };
        }
    }
}


class ComboboxValidator extends Validator{
    constructor(errorMsg){
        super(errorMsg ?? "List Items Validator");
        this.isAcceptEmpty = true;
        this.msgEmpty = null;
    }

    validate(items, text){
        try {
            if (Validator.isEmpty(text)){
                return { 
                    isValid: this.isAcceptEmpty,
                    msg: this.msgEmpty ?? this.errorMsg
                };
            }
            if (Validator.isEmpty(items)){
                return {
                    isValid: false,
                    msg: this.errorMsg
                }
            }
            if (items.some(function(item){
                return item.label === text;
            })) {
                return { isValid : true };
            }
            return {
                isValid: false,
                msg: this.errorMsg
            }
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            }
        }
    }
}

class GreaterThanTodayValidator extends Validator {
    constructor(errorMsg){
        super(errorMsg ?? "Greater than today validator");
    }

    validate(value){
        try {
            let time = new MyDate(value);
            let now = new MyDate();
            if (time > now){
                return {
                    isValid: false,
                    msg: this.errorMsg
                };
            }

            return super.validate(value);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            }
        }
    }
}

class LimitItemNumberValidator extends Validator {
    constructor(errorMsg){
        super(errorMsg ?? "Limit item number validator");
        this.min = null;
        this.max = null;
    }

    /**
     * Hàm đặt giới hạn số lượng phần tử
     * @param {*} min - số lượng tối thiểu
     * @param {*} max - số lượng tối đa
     * @returns this
     * @Created PhucTV (11/5/24)
     * @Modified None
     */
    setBoundary(min, max) {
        this.min = min;
        this.max = max;
        return this;
    }

    validate(value){
        try {
            let numbers = value?.length ?? 0;
            if (this.min != null && numbers < this.min){
                return {
                    isValid: false,
                    msg: this.errorMsg
                };
            }
            if (this.max != null && numbers > this.max){
                return {
                    isValid: false,
                    msg: this.errorMsg
                };
            }
            return super.validate(value);
        } catch (error){
            console.error(error);
            return {
                isValid: false,
                msg: error
            }
        }
    }
}



export { 
    Validator, RegexValidator, NotEmptyValidator,
    DateValidator, PhoneValidator, EmailValidator, UrlValidator,
    IdentityCardNumberValidator, MoneyValidator, 
    PositiveNumberValidator, RangeNumberValidator,
    UsernameValidator, PasswordValidator, RepasswordValidator,
    ComboboxValidator, GreaterThanTodayValidator,
    LimitItemNumberValidator
}
