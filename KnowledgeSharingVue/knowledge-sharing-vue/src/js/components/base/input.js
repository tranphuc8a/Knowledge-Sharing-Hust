import { Validator } from "@/js/utils/validator";

let input = {
   
    watch: {
        // input frame props
        state(newVal)           { this.inputFrame.state = newVal; },
        label(newVal)           { this.inputFrame.label = newVal; },
        title(newVal)           { this.inputFrame.title = newVal; },
        isObligate(newVal)      { this.inputFrame.isObligate = newVal; },
        isFull(newVal)          { this.inputFrame.isFull = newVal; },
        errorMessage(newVal)    { this.inputFrame.errorMessage = newVal; },
        // props:
        validator(newVal)       { this.data.validator = newVal; },
        value(newVal)           { this.setValue(newVal); }
        // events: not neccessary to watch
    },
    props: {
        // input frame
        state: { default: 'normal' },
        // label: { default: "Họ tên" },
        title: { default: null },
        label: { default: null },
        isObligate: { default: true },
        isDynamicValidate: { default: false },
        // isFull: { default: true },
        isShowTitle: { default: true },
        isShowError: { default: true },
        errorMessage: {default: 'Không hợp lệ'},
        // props:
        value: { default: '' },
        validator: { 
            default: {
                validate: async function(){
                    return { isValid: true }
                }
            }
        },

        onchange: {
            type: Function,
            default: async function(){
                // console.log(`Textfield has value is ${value}`);
            }
        },
        onfocus: {
            type: Function,
            default: async function(){}
        },
        onblur: {
            type: Function,
            default: async function(){}
        },
        onPressEnter: {
            type: Function,
            default: async function(){}
        }
    },
    methods: {
        /**
        * Validate dữ liệu của input
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async validate(){
            try {
                let rs = await this.validator.validate(this.data.value);
                if (rs.isValid) {
                    return true;
                }
                this.inputFrame.errorMessage = rs.msg;
                return false;
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Thực hiện Validate dữ liệu của input và cập nhật trạng thái
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async actionValidate(){
            try {
                this.setState('validating');
                let rs = await this.validate();
                if (rs){
                    this.setState('validated');
                } else {
                    this.setState('error');
                }
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Hai method thực hiện đăng ký và hủy đăng ký DynamicValidate
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        startDynamicValidate(){
            try {
                this.actionValidate();
                this.data.isDynamicValidate = true;
            } catch (error){
                console.error(error);
            }
        },
        stopDynamicValidate(){
            try {
                this.setState('normal');
                this.data.isDynamicValidate = false;
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Thực hiện binding chiều ngược lại value -> this.data.value
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async bindModel(value){
            this.setValue(value);
        },
        /**
        * Các hàm xử lý sự kiện mặc định của input: resolveOn[Change, Focus, Blur]
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnChange(){
            try {
                // await this.bindModel(value);
                // console.log(this.getValue());
                if (this.data.isDynamicValidate){
                    await this.actionValidate();
                }
                await this.onchange(this.data.value);
            } catch (error){
                console.error(error);
            }
        },
        async resolveOnFocus(){
            try {
                await this.onfocus();
            } catch (error){
                console.error(error);
            }
        },
        async resolveOnBlur(){
            try {
                await this.onblur();
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Xử lý sự kiện khi bấm enter
         * @param none
         * @Created PhucTV (26/1/24)
         * @Modified None
         */
        async resolveOnPressEnter(){
            try {
                await this.onPressEnter(this.data.value);
            } catch (error){
                console.error(error);
            }
        },

        /**
        * Hai hàm thực hiện focus và blur vào thẻ input
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async focus(){
            try {
                if (this.components && this.components.input){
                    this.components.input.focus();
                }
            } catch (error){
                console.error(error);
            }
        },
        async blur(){
            try {
                if (this.components && this.components.input){
                    this.components.input.blur();
                }
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Hai hàm thực hiện lấy giá trị và đặt giá trị vào input
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        setValue(value){
            try {
                this.data.value = value;
                if (this.data.isDynamicValidate){
                    this.actionValidate();
                }
            } catch (error){
                console.error(error);
            }
        },
        getValue(){
            try {
                return this.data.value;
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Hai hàm thực hiện thay đổi trạng thái của input
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async setState(state){
            try {
                if (Validator.isEmpty(state)){
                    state = "normal";
                }
                this.inputFrame.state = state;
            } catch (error){
                console.error(error);
            }
        }
    }
}

export { input }

