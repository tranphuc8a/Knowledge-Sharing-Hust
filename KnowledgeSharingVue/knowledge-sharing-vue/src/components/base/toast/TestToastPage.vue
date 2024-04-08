<template>
    <div>
        <MTextField ref="textfield" label="Nội dung toast" />
        <MButton label="Success" :onclick="show(toastTypes.SUCCESS)" />
        <MButton label="Information" :onclick="show(toastTypes.INFORMATION)" />
        <MButton label="Warning" :onclick="show(toastTypes.WARNING)" />
        <MButton label="Error" :onclick="show(toastTypes.ERROR)" />
    </div>
</template>


<script>
import MTextField from '@/components/base/authentication/MSlotedTextfield.vue';
import MButton from '@/components/base/buttons/MButton'

export default {
    name: 'TestToastManager',
    data(){
        return {
            toastTypes: {
                INFORMATION: 0,
                SUCCESS: 1,
                WARNING: 2,
                ERROR: 3
            }
        }
    },
    components: { MTextField, MButton },
    inject: { getToastManager: {}},
    methods: {
        /**
         * Displays the toast with the text field value.
         * @param {*} toastType - the type of toast to display
         * @returns none
         * @Created PhucTV (10/4/24)
         * @Modified None
         */
        show(toastType){
            let that = this;
            return async function(){
                const text = await that.getText();
                switch(toastType) {
                    case that.toastTypes.INFORMATION:
                        that.getToastManager().information(text);
                        break;
                    case that.toastTypes.SUCCESS:
                        that.getToastManager().success(text);
                        break;
                    case that.toastTypes.WARNING:
                        that.getToastManager().warning(text);
                        break;
                    case that.toastTypes.ERROR:
                        that.getToastManager().error(text);
                        break;
                }
            }
        },

        /**
         * Lay ve gia tri cua textfield
         * @param none
         * @returns {Promise<string>} The value of the text field.
         * @Created PhucTV (10/4/24)
         * @Modified None
         */
        async getText(){
            return await this.$refs.textfield?.getValue();
        }
    }
}
</script>
