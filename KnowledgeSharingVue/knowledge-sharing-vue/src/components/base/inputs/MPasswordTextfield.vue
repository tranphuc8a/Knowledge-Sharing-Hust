<template>
    <MTextField :placeholder="placeholder" :type="passwordType" ref="input"
        :autocomplete="autocomplete" :validator="validator" :errorMessage="errorMessage"
        :oninput="oninput">
        <div class="p-password-icon" @:click="resolveClickPasswordIcon">
            <MIcon fa="eye-slash" :style="iconStyle" v-show="passwordType !== 'password'"/>
            <MIcon fa="eye" :style="iconStyle" v-show="passwordType === 'password'"/>
        </div>
    </MTextField>
</template>

<script>
import { input } from '@/js/components/base/input';
import MTextField from './MSlotedTextfield.vue';

export default{
    name: 'MPasswordTextfield',
    components: { MTextField },
    data(){
        return {
            iconStyle: { color: 'var(--primary-color)'},
            passwordType: 'password',
            input: null
        }
    },
    mounted(){
        this.input = this.$refs.input;
    },
    methods: {

        async resolveClickPasswordIcon(){
            if (this.passwordType === 'password'){
                this.passwordType = 'text';
            } else {
                this.passwordType = 'password';
            }
        },

        async getValue(){
            try {
                return await this.input.getValue();
            } catch (error){
                console.error(error);
            }
        },
        async setValue(value){
            try {
                return await this.input.setValue(value);
            } catch (error){
                console.error(error);
            }
        },
        async startDynamicValidate(){
            try {
                return await this.input.startDynamicValidate();
            } catch (error){
                console.error(error);
            }
        },
        async stopDynamicValidate(){
            try {
                return await this.input.stopDynamicValidate();
            } catch (error){
                console.error(error);
            }
        },
        async focus(){
            try {
                return await this.input.focus();
            } catch (error){
                console.error(error);
            }
        },
        async validate(){
            try {
                return await this.input.validate();
            } catch (error){
                console.error(error);
            }
        }
    },
    props: {
        validator: input.props.validator,
        errorMessage: input.props.errorMessage,
        oninput: {},
        placeholder: {},
        autocomplete: {}
    }
}
</script>

<style scoped>

.p-password-icon{
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}
.p-password-icon > *{
    width: 24px;
    cursor: pointer;
}
</style>
