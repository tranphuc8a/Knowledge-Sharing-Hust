

let toast = {
    methods: {
       /**
         * Lấy về label chứa các nhãn trong language resource
         * @param none
         * @return label
         * @Created PhucTV (30/1/24)
         * @Modified None
        */
       getLabel(){
            if (this.label === null || this.label === undefined){
                this.label = this.lang?.components?.toast;
            }
            return this.label;
        },

        /**
         * Các hàm xử lý sự kiện tương ứng trên toast
         * @param  none
         * @returns none
         * @Created PhucTV (28/1/24)
         * @Modified None
        */
        async resolveOnUndo(){
            try {
                await this.onUndo();
            }
            catch (e) {
                console.error(e);
            }
        },
        async resolveOnClose(){
            try {
                this.$refs.toast.style.display = 'none';
                await this.onClose();
            }
            catch (e){
                console.error(e);
            }
        },
        async resolveClickHelp(){
            try {
                await this.onHelp();
            } catch (e){
                console.error(e);
            }
        }
    },

    props: {
        description: {
            type: String,
            default: "Đây là mô tả của toast"
        },
        onClose: {
            type: Function,
            default: async function(){
                console.log("Close Toast");
            }
        },
        onUndo: {
            type: Function,
            default: async function(){
                console.log("Undo Toast");
            }
        },
        onHelp: {
            type: Function,
            default: async function(){
                console.log("Help Toast");
            }
        }
    },

}


export default toast;