

<template>
    <div class="p-course-credit-navigation-card">
        <div class="p-ccnav card">
            <div class="p-ccnav-header">
                <div class="p-ccnav-heading">
                    {{ actionName }} khóa học
                </div>
            </div>
            <div class="p-ccnav-tabs">
                <div :class="['p-ccnav-tab', {'p-ccnav-active': dTabIndex == tabEnum.General }]" 
                    @:click="resolveOnChangeTab(tabEnum.General)"
                >
                    Tổng quan
                </div>

                <div :class="['p-ccnav-tab', {'p-ccnav-active': dTabIndex == tabEnum.Fee }]" 
                    @:click="resolveOnChangeTab(tabEnum.Fee)"
                >
                    Học phí
                </div>

                <div :class="['p-ccnav-tab', {'p-ccnav-active': dTabIndex == tabEnum.Introduction }]" 
                    @:click="resolveOnChangeTab(tabEnum.Introduction)"
                >
                    Bài giới thiệu
                </div>

                <div :class="['p-ccnav-tab', {'p-ccnav-active': dTabIndex == tabEnum.Other }]" 
                    @:click="resolveOnChangeTab(tabEnum.Other)"
                >
                    Khác
                </div>
            </div>

            <div class="p-devide"></div>
            
            <div class="p-ccnav__commit">

                <MButton 
                    :label="buttonName"
                    :onclick="resolveOnCommit"
                />
            </div>
        </div>
    </div>
</template>



<script>
import MButton from './../../../../../base/buttons/MButton.vue';


export default {
    name: 'CourseCreditNavigationCard',
    components: {
        MButton,
    },
    props: {
        onChangeTab: {
            type: Function,
            required: true,
        },
        onCommit: {
            type: Function,
            required: true,
        },
        isEdit: {
            type: Boolean,
            default: false,
        },
        tabIndex: {
            type: String,
            default: null,
        }
    },
    watch: {
        tabIndex: {
            handler: function(newValue){
                this.dTabIndex = newValue;
            },
            immediate: true,
        }
    },
    data(){
        return {
            tabEnum: {
                General: "General",
                Fee: "Fee",
                Introduction: "Introduction",
                Other: "Other",
            },
            dTabIndex: this.tabIndex || "General",
            isWorking: false,
            actionName: this.isEdit ? "Chỉnh sửa" : "Tạo mới",
            buttonName: this.isEdit ? "Lưu thay đổi" : "Tạo khóa học",
        }
    },
    async mounted(){
        this.refresh();
    },
    methods: {
        async refresh(){
            try {
                this.dTabIndex = this.tabEnum.General;
            } catch (error){
                console.error(error);
            }
        },

        async resolveOnChangeTab(dTabIndex){
            try {
                this.dTabIndex = dTabIndex;
                if (this.onChangeTab){
                    await this.onChangeTab(dTabIndex);
                }
            } catch (error){
                console.error(error);
            }
        },

        async resolveOnCommit(){
            if (this.isWorking){
                return;
            }
            try {
                this.isWorking = true;
                if (this.onCommit){
                    await this.onCommit();
                }
            } catch (error){
                console.error(error);
            } finally {
                this.isWorking = false;
            }
        },
    },
    inject: {
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-course-credit-navigation-card{
    width: 100%;
}

.p-ccnav{
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}

.p-ccnav > * {
    text-align: left;
}

.p-ccnav-heading{
    font-family: 'ks-font-semibold';
    font-size: 24px;
}

.p-ccnav-tabs{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 4px;
}

.p-ccnav-tab{
    padding: 16px 8px;
    cursor: pointer;
    border-radius: 4px;
    font-family: 'ks-font-semibold';
    color: var(--grey-color-600);
}

.p-ccnav-tab:hover{
    background-color: var(--grey-color-200);
}

.p-ccnav-tab.p-ccnav-active{
    background-color: var(--primary-color-100);
    color: var(--primary-color);
}

</style>

