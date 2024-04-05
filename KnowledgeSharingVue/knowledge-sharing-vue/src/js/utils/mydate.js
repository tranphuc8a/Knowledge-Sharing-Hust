/* eslint-disable */

// Lớp Mydate này kế thừa lớp Date nhưng hỗ trợ thêm định dạng chuỗi dd/mm/yyyy
class MyDate extends Date{
    static myDateFormat1 = /^([1-9]|0[1-9]|[12][0-9]|3[01])\/([1-9]|0[1-9]|1[0-2])\/\d{4}$/;
    static myDateFormat2 = /^([1-9]|0[1-9]|[12][0-9]|3[01])-([1-9]|0[1-9]|1[0-2])-\d{4}$/;

    /*
    * Constructor nhận date thuộc 2 format trên dd/mm/yyyy và dd-mm-yyyy
    * @param dateStr - chuỗi date
    * @Author TVPhuc (29/11/23)
    * @Edit None
    **/
    constructor(dateStr){
        try {
            if (dateStr === undefined || dateStr === null || dateStr === "") {
                return super();
            }
            if (MyDate.myDateFormat1.test(dateStr)){
                // Tách ngày, tháng, năm từ chuỗi
                const dateParts = dateStr.split('/');
                // Đưa về định dạng được hỗ trợ: yyyy-mm-dd
                let formated = `${dateParts[2]}-${dateParts[1]}-${dateParts[0]}`;
                super(formated);
            } else if (MyDate.myDateFormat2.test(dateStr)){
                const dateParts = dateStr.split('-');
                super(`${dateParts[2]}-${dateParts[1]}-${dateParts[0]}`);
            }else {
                super(dateStr);
            }
        } catch (error){
            throw error;
        }
    }

    /*
    * Trả ngày tháng về chuỗi định dạng dd/mm/yyyy
    * @param none
    * @Author TVPhuc (29/11/23)
    * @Edit None
    **/
    toMyDateFormat(){
        try {
            let day     = this.getDate();
            let month   = this.getMonth() + 1; // Tháng bắt đầu từ 0, nên cần cộng thêm 1
            let year    = this.getFullYear();

            let res = `${day > 9 ? "" : "0"}${day}/${month > 9 ? "" : "0"}${month}/${year}`;
            // console.log(res);
            return res;
        } catch (error){
            console.error(error);
        }
    }

    /*
    * Trả ngày tháng về chuỗi định dạng yyyy-mm-dd
    * @param none
    * @Author TVPhuc (29/11/23)
    * @Edit None
    **/
    toNormalFormat(){
        try {
            let day     = this.getDate();
            let month   = this.getMonth() + 1; 
            let year    = this.getFullYear();
            return `${year}-${month > 9 ? "" : "0"}${month}-${day > 9 ? "" : "0"}${day}`;
        } catch (error){
            console.error(error);
        }
    }

    /**
     * Trả về chuỗi ngày tháng theo định dạng format
     * @param {*} format - định dạng chuỗi (Ex: dd/MM/yyyy, yyyy-MM-dd, ...)
     * @return {*} 
     * @memberof MyDate
     * @Created Phuc (22/3/24)
     * @Modified None
     */
    toFormat(format){
        const mappings = {
            yyyy: this.getFullYear(),
            yy: ('' + this.getFullYear()).slice(-2),
            MM: ('0' + (this.getMonth() + 1)).slice(-2),
            M: this.getMonth() + 1,
            dd: ('0' + this.getDate()).slice(-2),
            d: this.getDate(),
            HH: ('0' + this.getHours()).slice(-2),
            hh: ('0' + ((this.getHours() % 12) || 12)).slice(-2),
            mm: ('0' + this.getMinutes()).slice(-2),
            ss: ('0' + this.getSeconds()).slice(-2)
        };
        for (const key in mappings) {
            format = format.replace(key, mappings[key]);
        }
        return format;
    }
}

export { MyDate };
