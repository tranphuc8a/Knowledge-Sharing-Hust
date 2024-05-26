

<template>
    <div class="p-profile-edit-subpage p-profile-main-subpage">
        <div class="p-profile-edit-subpage__left">
            <!-- Navigation -->
            <ProfileEditNavigationCard
                :on-save="resolveOnSaveProfile"
                :on-change-tab="resolveOnChangeTab"
                :tab-index="tabIndex"
            />
        </div>

        <div class="p-profile-edit-subpage__right">
            <!-- 4 content -->
            <ProfileEditGeneralContent v-show="tabIndex === tabEnum.General"
                ref="general"
            />

            <ProfileEditInformationContent v-show="tabIndex === tabEnum.Information"
                ref="information"
            />

            <ProfileEditContactContent v-show="tabIndex === tabEnum.Contact"
                ref="contact"
            />

            <ProfileEditCareerContent v-show="tabIndex === tabEnum.Career"
                ref="career"
            />

        </div>
    </div>
</template>



<script>
import ProfileEditNavigationCard from './ProfileEditNavigationCard.vue';
import ProfileEditGeneralContent from './ProfileEditGeneralContent.vue';
import ProfileEditInformationContent from './ProfileEditInformationContent.vue';
import ProfileEditContactContent from './ProfileEditContactContent.vue';
import ProfileEditCareerContent from './ProfileEditCareerContent.vue';
import CurrentUser from '@/js/models/entities/current-user';
import ViewUser from '@/js/models/views/view-user';
import Profile from '@/js/models/entities/profile';
import { PatchRequest, Request } from '@/js/services/request';

export default {
    name: 'ProfileEditSubpage',
    components: {
        ProfileEditNavigationCard,
        ProfileEditGeneralContent,
        ProfileEditInformationContent,
        ProfileEditContactContent,
        ProfileEditCareerContent,
    },
    props: {
    },
    data(){
        return {
            tabEnum: {
                General: "General",
                Information: "Information",
                Contact: "Contact",
                Career: "Career",
            },
            tabIndex: null,
            components: {
                General: null,
                Information: null,
                Contact: null,
                Career: null,
            },
            user: null,
        }
    },
    async mounted(){
        try {
            this.components = {
                General: this.$refs.general,
                Information: this.$refs.information,
                Contact: this.$refs.contact,
                Career: this.$refs.career,
            }
            this.user = await CurrentUser.getInstance();
            await this.refresh();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        async refresh(){
            try {
                this.tabIndex = this.tabEnum.General;
                this.user = new ViewUser().copy(await CurrentUser.getInstance());
                await this.setInputValue();
            } catch (error){
                console.error(error);
            }
        },

        async resolveOnChangeTab(tabIndex){
            try {
                this.tabIndex = tabIndex;
            } catch (error){
                console.error(error);
            }
        },

        async resolveOnSaveProfile(){
            try {
                if (! await this.validateData()){
                    return;
                }

                // Get data from input
                let inputValue = await this.getInputValue();

                // Get data from current user
                let currentUser = await CurrentUser.getInstance();
                let profile = new Profile().copy(currentUser).copy(inputValue);

                // set patch request
                await new PatchRequest('Users/me/update-profile')
                    .setBody(profile)
                    .execute();

                // success: update profile for current user
                let newUser = new ViewUser().copy(currentUser).copy(profile);
                await CurrentUser.setInstance(newUser);

                // update profile for input:
                this.user = newUser;
                await this.setInputValue();

                // toast Success:
                this.getToastManager().success('Cập nhật thông tin thành công');
                this.refreshProfilePage?.();
            } catch (e){
                Request.resolveAxiosError(e);
            }
        },

        async validateData(){
            try {
                let errorTabIndex = null;
                if (! (await this.components.General.validate())) {
                    errorTabIndex = this.tabEnum.General;
                } else if (! (await this.components.Information.validate())) {
                    errorTabIndex = this.tabEnum.Information;
                } else if (! (await this.components.Contact.validate())) {
                    errorTabIndex = this.tabEnum.Contact;
                } else if (! (await this.components.Career.validate())) {
                    errorTabIndex = this.tabEnum.Career;
                }
                if (errorTabIndex != null){
                    await this.startDynamicValidate();
                    await this.focusError(errorTabIndex);
                    return false;
                }

                return true;                
            } catch (error){
                console.error(error);
                return false;
            }
        },

        async startDynamicValidate(){
            try {
                this.components.General.startDynamicValidate();
                this.components.Information.startDynamicValidate();
                this.components.Contact.startDynamicValidate();
                this.components.Career.startDynamicValidate();
            } catch (e){
                console.error(e);
            }
        },

        async stopDynamicValidate(){
            try {
                this.components.General.stopDynamicValidate();
                this.components.Information.stopDynamicValidate();
                this.components.Contact.stopDynamicValidate();
                this.components.Career.stopDynamicValidate();
            } catch (e){
                console.error(e);
            }
        },

        async focusError(tabIndex){
            try {
                this.tabIndex = tabIndex;
                let component = this.components[tabIndex];
                await component.focusError();
            } catch (e){
                console.error(e);
            }
        },

        async getInputValue(){
            try {
                let informationData = await this.components.Information.getValue();
                let contactData = await this.components.Contact.getValue();
                let careerData = await this.components.Career.getValue();
                let combinedData = { ...informationData, ...contactData, ...careerData };
                return combinedData;
            } catch (e){
                console.error(e);
                return null;
            }
        },

        async setInputValue(){
            try {
                await this.components.Information.setValue(this.user);
                await this.components.Contact.setValue(this.user);
                await this.components.Career.setValue(this.user);
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
        refreshProfilePage: {},
    }
}

</script>

<style>
.p-profile-main-subpage{
    max-width: var(--profile-page-max-width);
    width: 100%;
    height: fit-content;
}
</style>

<style scoped>

.p-profile-edit-subpage{
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

.p-profile-edit-subpage__left{
    width: 0;
    flex-shrink: 1;
    flex-grow: 1;

    position: sticky;
    bottom: 0;

    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-profile-edit-subpage__right{
    width: 0;
    flex-shrink: 1;
    flex-grow: 2;
}

</style>

