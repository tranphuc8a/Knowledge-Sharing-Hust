

<template>
    <div class="p-profile-edit-navigation-card">
        <div class="p-penav card">
            <div class="p-penav-header">
                <div class="p-penav-heading">
                    Chỉnh sửa thông tin cá nhân
                </div>
            </div>
            <div class="p-penav-tabs">
                <div :class="['p-penav-tab', {'p-penav-active': dTabIndex == tabEnum.General }]" 
                    @:click="resolveOnChangeTab(tabEnum.General)"
                >
                    Tổng quan
                </div>

                <div :class="['p-penav-tab', {'p-penav-active': dTabIndex == tabEnum.Information }]" 
                    @:click="resolveOnChangeTab(tabEnum.Information)"
                >
                    Thông tin chi tiết
                </div>

                <div :class="['p-penav-tab', {'p-penav-active': dTabIndex == tabEnum.Contact }]" 
                    @:click="resolveOnChangeTab(tabEnum.Contact)"
                >
                    Thông tin liên hệ
                </div>

                <div :class="['p-penav-tab', {'p-penav-active': dTabIndex == tabEnum.Career }]" 
                    @:click="resolveOnChangeTab(tabEnum.Career)"
                >
                    Thông tin học tập, nghề nghiệp
                </div>
            </div>

            <div class="p-devide"></div>
            
            <div class="p-penav__save" @:click="resolveOnSave">

                <MButton 
                    label="Lưu thay đổi"
                    :onclick="resolveOnSave"
                />
            </div>
        </div>
    </div>
</template>



<script>
import MButton from './../../../../../base/buttons/MButton.vue';


export default {
    name: 'ProfileEditNavigationCard',
    components: {
        MButton,
    },
    props: {
        onChangeTab: {
            required: true,
        },
        onSave: {
            required: true,
        },
        tabIndex: {
            required: true,
        },
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
                Information: "Information",
                Contact: "Contact",
                Career: "Career",
            },
            dTabIndex: this.tabIndex,
            isWorking: false,
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

        async resolveOnSave(){
            if (this.isWorking){
                return;
            }
            try {
                this.isWorking = true;
                if (this.onSave){
                    await this.onSave();
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

.p-profile-edit-navigation-card{
    width: 100%;
}

.p-penav{
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}

.p-penav > * {
    text-align: left;
}

.p-penav-heading{
    font-family: 'ks-font-semibold';
    font-size: 24px;
}

.p-penav-tabs{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 4px;
}

.p-penav-tab{
    padding: 16px 8px;
    cursor: pointer;
    border-radius: 4px;
    font-family: 'ks-font-semibold';
    color: var(--grey-color-600);
}

.p-penav-tab:hover{
    background-color: var(--grey-color-200);
}

.p-penav-tab.p-penav-active{
    background-color: var(--primary-color-100);
    color: var(--primary-color);
}

</style>

