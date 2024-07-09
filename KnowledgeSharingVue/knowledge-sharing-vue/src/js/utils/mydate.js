// Lớp Mydate này kế thừa lớp Date nhưng hỗ trợ thêm định dạng chuỗi dd/mm/yyyy
class MyDate extends Date{
    static myDateFormat1 = /^([1-9]|0[1-9]|[12][0-9]|3[01])\/([1-9]|0[1-9]|1[0-2])\/\d{4}$/;
    static myDateFormat2 = /^([1-9]|0[1-9]|[12][0-9]|3[01])-([1-9]|0[1-9]|1[0-2])-\d{4}$/;
    static myDateTimeFormat1 = /^([1-9]|0[1-9]|[12][0-9]|3[01])\/([1-9]|0[1-9]|1[0-2])\/\d{4} (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
    static myDateTimeFormat2 = /^([1-9]|0[1-9]|[12][0-9]|3[01])-([1-9]|0[1-9]|1[0-2])-\d{4} (\d{1,2}):(\d{1,2}):(\d{1,2})$/;

    static convertToISO(dateString) {
        if (dateString.toISOString != undefined) {
            return dateString.toISOString();
        }
        if (MyDate.myDateFormat1.test(dateString)) {
            let parts = dateString.split("/");
            let day = parts[0].padStart(2, '0');
            let month = parts[1].padStart(2, '0');
            let year = parts[2];
            return `${year}-${month}-${day}`;
        } else if (MyDate.myDateFormat2.test(dateString)) {
            let parts = dateString.split("-");
            let day = parts[0].padStart(2, '0');
            let month = parts[1].padStart(2, '0');
            let year = parts[2];
            return `${year}-${month}-${day}`;
        } else if (MyDate.myDateTimeFormat1.test(dateString)) {
            let parts = dateString.split(/\/|:|\s/);
            let day = parts[0].padStart(2, '0');
            let month = parts[1].padStart(2, '0');
            let year = parts[2];
            let hour = parts[3].padStart(2, '0');
            let minute = parts[4].padStart(2, '0');
            let second = parts[5].padStart(2, '0');
            return `${year}-${month}-${day}T${hour}:${minute}:${second}`;
        } else if (MyDate.myDateTimeFormat2.test(dateString)) {
            let parts = dateString.split(/-|:|\s/);
            let day = parts[0].padStart(2, '0');
            let month = parts[1].padStart(2, '0');
            let year = parts[2];
            let hour = parts[3].padStart(2, '0');
            let minute = parts[4].padStart(2, '0');
            let second = parts[5].padStart(2, '0');
            return `${year}-${month}-${day}T${hour}:${minute}:${second}`;
        } else {
            // Nếu định dạng không khớp, trả về chuỗi gốc
            return dateString;
        }
    }

    /**
    * Constructor nhận date thuộc 2 format trên dd/mm/yyyy và dd-mm-yyyy
    * @param dateString - chuỗi date
    * @Author TVPhuc (29/11/23)
    * @Edit None
    **/
    constructor(dateString){
        if (dateString === undefined || dateString === null || dateString === "") {
            super();
            return;
        }
        
        let formattedDateString = MyDate.convertToISO(dateString);

        // Kiểm tra xem chuỗi đầu vào có chứa thông tin về múi giờ hay không
        const hasTimeZone = /[+-]\d{2}:\d{2}$|Z$/.test(formattedDateString);
        
        // Nếu không có thông tin về múi giờ, thêm "Z" để chỉ định UTC
        if (!hasTimeZone) {
            formattedDateString += 'Z';
        }

        // Gọi constructor của lớp Date
        super(formattedDateString);
    }

    /**
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

    /**
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
        // this.Get
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
            ss: ('0' + this.getSeconds()).slice(-2),
            day: `${this.getDay()}`
        };
        for (const key in mappings) {
            format = format.replace(key, mappings[key]);
        }
        return format;
    }

    toTimeSince() {
        const now = new Date();
        const secondsPast = (now - this) / 1000;
        
        if (secondsPast < 60) {
            return `${Math.round(secondsPast)} giây trước`;
        }
        if (secondsPast < 3600) {
            return `${Math.round(secondsPast / 60)} phút trước`;
        }
        if (secondsPast < 86400) {
            return `${Math.round(secondsPast / 3600)} giờ trước`;
        }
        if (secondsPast < 604800) {
            return `${Math.round(secondsPast / 86400)} ngày trước`;
        }
        if (secondsPast < 2419200) {
            return `${Math.round(secondsPast / 604800)} tuần trước`;
        }
        if (secondsPast < 29030400) {
            return `${Math.round(secondsPast / 2419200)} tháng trước`;
        }
        return `${Math.round(secondsPast / 29030400)} năm trước`;
    }

    toFullyText() {
        const daysOfWeek = ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'];
        
        const dayName = daysOfWeek[this.getDay()];
        const day = this.getDate();
        const month = this.getMonth() + 1; // getMonth() trả về từ 0-11
        const year = this.getFullYear();
        const hour = this.getHours().toString().padStart(2, '0');
        const minutes = this.getMinutes().toString().padStart(2, '0');
        
        return `${dayName}, ngày ${day} tháng ${month} năm ${year}, lúc ${hour}:${minutes}`;
    }

    toFullyDate(){
        const daysOfWeek = ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'];
        
        const dayName = daysOfWeek[this.getDay()];
        const day = this.getDate();
        const month = this.getMonth() + 1; // getMonth() trả về từ 0-11
        const year = this.getFullYear();
        
        return `${dayName}, ngày ${day} tháng ${month} năm ${year}`;
    }

    toFullyTime(){
        const hour = this.getHours().toString().padStart(2, '0');
        const minutes = this.getMinutes().toString().padStart(2, '0');
        
        return `lúc ${hour}:${minutes}`;
    }
    
}

export { MyDate };
