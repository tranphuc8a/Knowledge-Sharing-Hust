

<template>
    <InputFrame :label="inputFrame.label" :title="inputFrame.title" 
                :state="inputFrame.state" :errorMessage="inputFrame.errorMessage"
                :is-obligate="inputFrame.isObligate"
                :isShowTitle="inputFrame.isShowTitle" :isShowError="inputFrame.isShowError">
        <label>

            <div class="p-image-input">
                <div class="p-image" v-if="data.imgSrc != null">
                    <img :src="data.imgSrc" alt="image"  />
                </div>
                <div class="p-image-icon">
                    <MIcon fa="upload" :style="iconStyle" class="p-upload-icon" />
                </div>
            </div>
            <input type="file" ref="input" :style="{display: 'none'}" 
                @:change="resolveOnChange"
            />
        </label>
    </InputFrame>

</template>

<script>
import InputFrame from './MInputFrame.vue';
import { input } from '@/js/components/base/input';

export default {
    name: 'ImageInput',
    components: {
        InputFrame,
    },
    data(){
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
                imgSrc: require('@/assets/default-thumbnail/lesson-image-icon.png'),
                value: this.value,
                isDynamicValidate: this.isDynamicValidate,
                validator: this.validator,
                placeholder: this.placeholder,
                type: this.type,
                isShowIcon: this.isShowIcon,
                file: null
            },
            components: {
                input: null,
            },
            iconStyle: {
                fontSize: '48px',
                color: 'red',
            }
        }
    },
    mounted(){
        this.components.input = this.$refs['input'];
    },
    props: {
        ...input.props,
    },
    methods: {
        ...input.methods,

        handleFileInput(e){
            console.log(e.target.files)
        },

        async resolveOnChange(){
            try {
                console.log("change data");
                let file = this.components.input.files[0];
                this.data.file = file;
                let that = this;
                if (file != null){
                    const reader = new FileReader();
                    reader.onload = function(e){
                        that.data.imgSrc = e.target.result;
                    }
                    reader.readAsDataURL(file);
                }
            } catch (error){
                console.error(error);
            }
        },

        async validate(){
            return true;
        },

        async getValue(){
            return this.data?.file;
        },

        async setValue(value){
            try {
                this.data.file = value;
                this.components.input.files = [value];
            }
            catch (error){
                console.error(error);
            }
        },
    },

}

</script>

<style scoped>

label{
    width: fit-content;
    height: fit-content;
}

.p-image-input{
    width: 100px;
    height: 100px;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-flow: row nowrap;
    border-radius: 50px;
    overflow: hidden;

    box-shadow: 0px 0px 8px rgba(var(--primary-color-rgb), .86);
    cursor: pointer;
    background-color: var(--grey-color-100);
    position: relative;
}

.p-image-input:hover{
    background-color: var(--grey-color-200);
} 

.p-image,
.p-image-icon{
    width: 100%;
    height: 100%;
    
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    flex-shrink: 0;
    flex-grow: 1;
}

.p-image-icon{
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
}

.p-image img{
    width: 100%;
    height: 100%;

}

.p-image-icon .p-upload-icon{
    visibility: hidden;
}

.p-image-icon:hover .p-upload-icon{
    visibility: visible;
}

</style>