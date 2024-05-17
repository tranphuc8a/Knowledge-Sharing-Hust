/* eslint-disable */

import { Validator } from "./validator";

class Unicode{
    static acents = "àáãảạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệđùúủũụưừứửữựòóỏõọôồốổỗộơờớởỡợìíỉĩịäëïîöüûñçýỳỹỵỷ";
    static ascii  = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeduuuuuuuuuuuoooooooooooooooooiiiiiaeiiouuncyyyyy";

    /**
     * Hàm thực hiện chuyển một chuỗi tiếng việt Unicode về mã ASCII
     * @param {*} str - chuỗi cần chuyển đổi
     * @returns chuỗi anscii sau khi được chuyển dổi
     * @Created PhucTV (28/1/24)
     * @Modified None
    */
    static unicodeToAscii(str){
        try {
            if (Validator.isEmpty(str)){
                return "";
            }
            str = String(str);
            let from = Unicode.acents, to = Unicode.ascii;
            for (let i = 0; i < from.length; i++) {
                str = str.replace(RegExp(from[i], "gi"), to[i]);
            }

            str = str.toLowerCase()
                .trim()
                .replace(/[^a-z0-9\-]/g, '-')
                .replace(/-+/g, '-');

            return str;
        } catch (error){
            console.error(error);
        }
    }

}

export { Unicode };