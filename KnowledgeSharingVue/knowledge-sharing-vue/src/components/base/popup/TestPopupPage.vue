<template>
    <div>
        <MTextField ref="textfield" label="Nội dung popup" />
        <MButton label="Success" :onclick="show(popupTypes.SUCCESS)" />
        <MButton label="Information" :onclick="show(popupTypes.INFORMATION)" />
        <MButton label="Warning" :onclick="show(popupTypes.WARNING)" />
        <MButton label="Error" :onclick="show(popupTypes.ERROR)" />
    </div>
</template>


<script>
import MTextField from '@/components/base/authentication/MSlotedTextfield.vue';
import MButton from '@/components/base/buttons/MButton'

export default {
    name: 'TestPopupPage',
    data(){
        return {
            popupTypes: {
                INFORMATION: 0,
                SUCCESS: 1,
                WARNING: 2,
                ERROR: 3
            }
        }
    },
    components: { MTextField, MButton },
    inject: { getPopupManager: {}},
    methods: {
        /**
         * Displays the popup with the text field value.
         * @param {*} popupType - the type of popup to display
         * @returns none
         * @Created PhucTV (10/4/24)
         * @Modified None
         */
        show(popupType){
            let that = this;
            return async function(){
                const text = await that.getText();
                switch(popupType) {
                    case that.popupTypes.INFORMATION:
                        that.getPopupManager().information(text);
                        break;
                    case that.popupTypes.SUCCESS:
                        that.getPopupManager().success(text);
                        break;
                    case that.popupTypes.WARNING:
                        that.getPopupManager().warning(text);
                        break;
                    case that.popupTypes.ERROR:
                        that.getPopupManager().error(text);
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
