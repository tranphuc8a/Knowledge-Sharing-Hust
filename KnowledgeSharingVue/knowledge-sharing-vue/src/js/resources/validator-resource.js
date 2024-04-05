
export default {
    vi: {
        messages: {
            notNull: 'Không được để trống',
            notNullField: function(fieldName){
                return `Trường '${fieldName}' không được trống`;
            },
            invalidFormat: function(formatName = ''){
                return `Không đúng định dạng${formatName !== '' ? ' ' : ''}${formatName}`;
            },
            invalidFormatField: function(fieldName, formatName = ''){
                return `Trường '${fieldName}' không đúng định dạng${formatName !== '' ? ' ' : ''}${formatName}`;
            },
            invalidValue: 'Giá trị không hợp lệ',
            notInListed: 'Giá trị không có trong danh sách',
            notExisted: 'Giá trị không tồn tại',
            connectError: 'Không thể kết nối đến máy chủ, vui lòng quay lại sau',
        },
        fields: {
            name: 'Họ tên',
            date: 'Ngày tháng',
            phone: 'Số điện thoại',
            email: 'Email',
            cccd: 'CCCD',
            money: 'Tiền',
            id: 'ID'
        },
        entities: {
            customer: "Khách hàng",
            employee: "Nhân viên"
        }
    },
    en: {
        messages: {
            notNull: 'Not be empty',
            notNullField: function(fieldName){
                return `Field '${fieldName}' is not allowed to be empty`;
            },
            invalidFormat: function(formatName = ''){
                return `Not in '${formatName}' format`;
            },
            invalidFormatField: function(fieldName, formatName = ''){
                return `Field '${fieldName}' is not in '${formatName}' format`;
            },
            invalidValue: 'The value is invalid',
            notInListed: 'The value is not in the lists',
            notExisted: 'The value is not existed',
            connectError: 'Unable to connect to the server, please come back later',
        },
        fields: {
            name: 'Name',
            date: 'Date',
            phone: 'Phone number',
            email: 'Email',
            cccd: 'Identity card',
            money: 'Money',
            id: 'ID'
        }, 
        entities: {
            customer: "Customer",
            employee: "Employee"
        }
    }
};