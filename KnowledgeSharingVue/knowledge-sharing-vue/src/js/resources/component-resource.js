
import changeLanguageButtonResource from "./authentication/change-language-button-resource";

export default {
    vi: {
        input: {
            notObligate: 'Không bắt buộc',
            combobox: {
                noOptions: 'Hiện không có lựa chọn nào',
            }
        },
        dialog: {
            cancelButton: 'Hủy bỏ',
            okayButton: 'Đồng ý'
        },
        toast: {
            undo: 'Hoàn tác',
            help: 'Xem hướng dẫn',
            error: 'Lỗi! ',
            inform: 'Thông tin! ',
            success: 'Thành công! ',
            warning: 'Cảnh báo! ',
        },
        popupManager: {
            header: 'Thông báo',
            emptyError: 'Nội dung thông báo không được rỗng',
            formatError: 'Không đúng định dạng thông báo',
            previousButtonLabel: 'Trở lại',
            cancelButtonLabel: 'Hủy bỏ',
            okayButtonLabel: 'Đồng ý'
        },
        changeLanguageButton: changeLanguageButtonResource.vi,
    },
    en: {
        input: {
            notObligate: 'Not obligate',
            combobox: {
                noOptions: 'There is no options',
            }
        },
        dialog: {
            cancelButton: 'Cancel',
            okayButton: 'Okay'
        },
        toast: {
            undo: 'Undo',
            help: 'Help',
            error: 'Error! ',
            inform: 'Inform! ',
            success: 'Success! ',
            warning: 'Warning! ',
        },
        popupManager: {
            header: 'Information',
            emptyError: 'Popup content is not allow to empty',
            formatError: 'Not in Popup format',
            previousButtonLabel: 'Back',
            cancelButtonLabel: 'Cancel',
            okayButtonLabel: 'Okay'
        },
        changeLanguageButton: changeLanguageButtonResource.en,
    }
};

