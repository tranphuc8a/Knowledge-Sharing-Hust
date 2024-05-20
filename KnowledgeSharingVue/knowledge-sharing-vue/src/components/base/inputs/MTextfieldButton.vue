<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate"
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">
        <div class="p-textfield-bound" :title="title ?? null">
            <input ref="textfield" class="p-textfield" v-model="data.value"
                    :type="data.type" :placeholder="data.placeholder"  
                    @:input="resolveOnInput" @:change="resolveOnChange" 
                    @:focus="resolveOnFocus" @:blur="resolveOnBlur" 
                    @:keyup.enter.prevent.stop="resolveOnPressEnter"
                    :readonly="inputFrame.state==='read-only'"/>

            <div class="p-textfield-button"
                @:click="resolveOnclickIcon"
            >
                <MIcon :fa="fa ?? 'magnifying-glass'" :family="family ?? 'fas'"
                    :style="data.iconStyle" v-show="!data.isLoading" />
                <MSpinner :style="data.iconStyle" v-show="data.isLoading" />
            </div>
        </div>
    </InputFrame>
</template>

<script>
import InputFrame from './MInputFrame.vue';
import { input } from '@/js/components/base/input';
// import { myEnum } from '@/js/resources/enum';

let textfield = {
    name: "TextfieldButton",
    data() {
        return {
            inputFrame: {
                state: this.state,
                label: this.label,
                title: this.title,
                errorMessage: this.errorMessage,
                isObligate: this.isObligate,
                // isFull: this.isFull
                isShowTitle: this.isShowTitle,
                isShowError: this.isShowError
            },
            data: {
                isLoading: false,
                iconStyle: {
                    color: 'white'
                },
                value: this.value,
                isDynamicValidate: this.isDynamicValidate,
                validator: this.validator,
                placeholder: this.placeholder,
                type: this.type,
                isShowIcon: this.isShowIcon
            },
            components: {
                input: null,
            }
        };
    },
    mounted() {
        this.components.input = this.$refs.textfield;
    },
    components: {
        InputFrame
    },
    methods: {
        ...input.methods,
        
        /**
        * Ghi đè sự kiện resolveOnChange
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnChange(){
            try {
                await this.onchange(this.data.value);
            } catch (error){
                console.error(error);
            }
        },
        /**
        * Xử lý sự kiện resolveOnInput
        * @param none
        * @Author TVPhuc (12/12/23)
        * @Edit None
        **/
        async resolveOnInput(){
            try {
                if (this.data.isDynamicValidate){
                    await this.actionValidate();
                }
                await this.oninput(this.data.value);
            } catch (error){
                console.error(error);
            }
        },

        /**
         * Xử lý sự kiện click vào icon
         * @param none
         * @Created PhucTV (25/1/24)
         * @Modified None
        */
        async resolveOnclickIcon(){
            if (this.data.isLoading) return;
            try {
                this.data.isLoading = true;
                await this.onclickIcon(this.data.value);
            } catch (e) {
                console.error(e);
            } finally {
                this.data.isLoading = false;
            }
        }
    },
    props: {
        ...input.props,
        fa: {},
        family: {},
        type: { default: "text" },
        placeholder: { default: "Placeholder" },
        autocomplete: { default: "email" },
        isShowIcon: { default: true },
        oninput: {
            type: Function,
            default: async function(){}
        },
        onclickIcon: {
            type: Function,
            default: null
        }
    },
    watch: {
        ...input.watch,
        type(newVal)            { this.data.type = newVal; },
        placeholder(newVal)     { this.data.placeholder = newVal; },
        isShowIcon(newVal)      { this.data.isShowIcon = newVal; },
    }
};
export default textfield;
</script>

<style scoped>
@import url(@/css/base/input/textfield.css);

.p-textfield-button{
    height: 100%;
    width: 56px;
    background-color: var(--primary-color);
    color: white;
    cursor: pointer;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.p-textfield-bound{
    padding-right: 0;
    overflow: hidden;
}

.p-textfield-button:hover{
    background-color: var(--primary-color-400);
}
.p-textfield-button:active{
    background-color: var(--primary-color);
}

</style>
