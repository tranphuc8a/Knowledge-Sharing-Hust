<template>
    <div class="p-input" :state="state"> 
        <div class="p-input-title" v-show="isShowTitle">
            <div>
                <label class="p-input-label" :title="title"> {{ label }} </label>
                <span class="p-input-obligate" v-show="isObligate"> * </span>
            </div>
            <div>
                <label class="p-input-note" v-show="false"> {{ this.notObligate }} </label>
            </div>
        </div>
        <div class="p-input-content">
            <slot />
        </div>
        <div class="p-input-error-message" v-show="isShowError"> {{ errorMessage }} </div>
    </div>
</template>

<script>
export default {
    data(){
        return {
            notObligate: 'Không bắt buộc'
        }
    },
    mounted() {
        this.notObligate = this.lang.components.input.notObligate;
    },
    props: {
        state: {
            default: 'error'
        },
        isShowTitle: {
            default: true,
        },
        isShowError: {
            default: true
        },
        title: {},
        label: {
            default: 'Label of input'
        },
        isObligate: {
            default: false,
        },
        errorMessage: {
            default: 'Dữ liệu không hợp lệ'
        }
    }
}
</script>

<style scoped>
@import url(@/css/base/input.css);
.p-input .p-input-title .p-input-obligate{
    color: red;
}
</style>
