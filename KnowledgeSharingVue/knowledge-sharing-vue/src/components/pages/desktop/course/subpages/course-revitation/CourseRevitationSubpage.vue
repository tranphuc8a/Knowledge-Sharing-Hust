

<template>
    <div class="p-course-revitation-subpage p-course-subpage">
        <div class="p-course-revitation-card card">
            <div class="p-course-revitation-header">
                <div class="p-pfh-heading">
                    Yêu cầu và lời mời tham gia khóa học
                </div>
                <div class="p-pfh-search-button">
                    <!-- Textfield Button -->
                    <span>
                        <MButton 
                            label="Mời người tham gia"
                            :onclick="resolveClickInvite"
                            fa="user-plus"
                        />
                    </span>
                    <span>
                        <MTextfieldButton 
                            placeholder="Tìm kiếm người dùng"
                            :is-show-icon="true" 
                            :is-show-title="false" 
                            :is-show-error="false" 
                            :is-obligate="false"
                            :onclick-icon="resolveOnClickSearch" 
                            :validator="null"
                            state="normal"
                            ref="textfield"
                        />
                    </span>
                </div>
            </div>

            <div class="p-course-revitation-navigation">
                <!-- Profile Friend Navigation -->
                <CourseRevitationNavigation />
            </div>

            <div class="p-course-revitation-content">
                <router-view />
            </div>
        </div>

        <CourseInvitationPopup 
            :is-show="false"
            ref="invitationPopup"
            :on-okay="resolveOkayInvitation"
        />
    </div>
</template>



<script>
// import { Validator } from '@/js/utils/validator';
import MTextfieldButton from './../../../../../base/inputs/MTextfieldButton.vue';
import CourseRevitationNavigation from './CourseRevitationNavigation.vue';
import CourseInvitationPopup from '@/components/base/popup/course-invitation-popup/CourseInvitationPopup.vue';

export default {
    name: 'CourseRevitationSubpage',
    components: {
        MTextfieldButton,
        CourseRevitationNavigation,
        CourseInvitationPopup
    },
    props: {
    },
    data(){
        return {
            onClickSearch: null,
        }
    },
    async mounted(){
    },
    methods: {
        registerOnClickSearch(handler){
            if (handler){
                this.onClickSearch = handler;
            }
        },

        async resolveOnClickSearch(){
            try {
                // get search text:
                let searchText = await this.$refs.textfield.getValue();
                // if (Validator.isEmpty(searchText)){
                //     return;
                // }

                // call handler
                if (this.onClickSearch != null){
                    await this.onClickSearch(searchText);
                }
            } catch (e){
                console.error(e);
            }
        },

        async resolveClickInvite(){
            try {
                await this.$refs.invitationPopup.show();
            } catch (e){
                console.error(e);
            }
        },


        async resolveOkayInvitation(){
            try {
                await this.$refs.invitationPopup.hide();
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
    },
    provide(){
        return {
            registerOnClickSearch: this.registerOnClickSearch,
        }
    }
}

</script>


<style scoped>



.p-course-revitation-subpage{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    padding-bottom: 32px;
}


.p-course-revitation-subpage .p-course-revitation-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    padding-bottom: 32px;
}

.p-course-revitation-subpage .p-course-revitation-card > * {
    width: 100%;
}


.p-course-revitation-subpage .p-course-revitation-header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-course-revitation-subpage .p-pfh-heading{
    font-family: 'ks-font-semibold';
    font-size: 24px;
}

.p-pfh-search-button{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: center;
    gap: 8px;
}

</style>

