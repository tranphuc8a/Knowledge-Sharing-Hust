

<template>
    <div class="prb-resume-bio" >

        <div class="prb-bio-viewer" v-show="isShowBio && isEditing === false">
            <LightLatexMarkdownRender :markdown-content="dBio" />
            <MSecondaryButton 
                label="Chỉnh sửa tiểu sử"
                :onclick="resolveEditBio"
                :buttonStyle="buttonStyle"
                v-show="isMySelf"
            />
            <div class="prb-devider"
                v-show="!isMySelf"
            ></div>
        </div>

        <div class="prb-add-bio" v-show="isMySelf && !isShowBio && isEditing === false">
            <MSecondaryButton 
                label="Thêm tiểu sử"
                :onclick="resolveAddBio"
                :buttonStyle="buttonStyle"
            />
        </div>

        <div class="prb-bio-editor" v-show="isEditing === true">
            <div class="prb-text-area">
                <MTextArea
                    ref="textarea"
                    :placeholder="null"
                    :is-show-title="false" :is-show-error="true" 
                    :validator="null"
                    max-height="100px" rows="3"
                />
            </div>
            <div class="prb-buttons">
                <div class="prb-buttons__left">
                    <div class="prb-button">
                        <MDeleteButton 
                            label="Xóa tiểu sử"
                            :onclick="resolveDeleteBio"
                            :buttonStyle="buttonStyle"
                        />
                    </div>
                </div>
                <div class="prb-buttons__right">
                    <div class="prb-button">
                        <MSecondaryButton
                            label="Hủy"
                            :onclick="resolveCancelEditBio"
                            :buttonStyle="buttonStyle"
                        />
                    </div>
                    <div class="prb-button">
                        <MButton
                            label="Lưu"
                            :onclick="resolveSaveBio"
                            :buttonStyle="buttonStyle"
                        />
                    </div>
                </div>
            </div>

        </div>
    </div>
</template>



<script>
import { Validator } from '@/js/utils/validator';
import LightLatexMarkdownRender from '@/components/base/markdown/LightLatexMarkdownRender.vue';
import MSecondaryButton from '@/components/base/buttons/MSecondaryButton';
import MButton from '@/components/base/buttons/MButton';
import MTextArea from '@/components/base/inputs/MTextArea';
import MDeleteButton from '@/components/base/buttons/MDeleteButton';
import { PatchRequest } from '@/js/services/request';
import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'ResumeBio',
    components: {
        LightLatexMarkdownRender,
        MSecondaryButton, MButton, MDeleteButton,
        MTextArea
    },
    props: {
    },
    data(){
        return {
            isShowBio: true,
            isEditing: false,
            dBio: "",
            buttonStyle: {
                padding: '16px'
            },
            currentUser: null,
            isMySelf: false,
            isWaiting: false,
        }
    },
    mounted(){
        this.refreshBio();
    },
    methods: {
        async refreshBio(){
            try {
                this.isWaiting = true;
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser != null){
                    this.isMySelf = this.currentUser.UserId === this.getUser()?.UserId;
                }
                let bio = this.getUser()?.Bio;
                if (Validator.isEmpty(bio)){
                    this.isShowBio = false;
                    return;
                }
                this.dBio = bio;
                this.isShowBio = true;
                this.isEditing = false;
            } catch (error) {
                console.error(error);
            } finally {
                this.isWaiting = false;
            }
        },

        async resolveEditBio(){
            try {
                if (this.isWaiting) return;
                this.$refs['textarea'].setValue(this.dBio);
                this.$refs['textarea'].focus();
                this.isEditing = true;
            } catch (error) {
                console.error(error);
            }
        },

        async resolveAddBio(){
            try {
                if (this.isWaiting) return;
                this.$refs['textarea'].setValue("");
                this.$refs['textarea'].focus();
                this.isEditing = true;
            } catch (error) {
                console.error(error);
            }
        },

        async resolveDeleteBio(){
            try {
                if (this.isWaiting) return;
                let informText = "Bạn có chắc chắn muốn xóa tiểu sử này không?";
                this.getPopupManager().inform(informText, this.submitDeleteBio.bind(this));
            } catch (error) {
                console.error(error);
            }
        },

        async submitDeleteBio(){
            try {
                this.isWaiting = true;
                await new PatchRequest('Users/me/update-bio')
                    .setBody("").execute();
                this.getUser().Bio = "";
                this.isEditing = false;
                this.currentUser = await CurrentUser.getInstance();
                this.currentUser.Bio = "";
                await CurrentUser.setInstance(this.currentUser);
                this.refreshBio();
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.isWaiting = false;
            }
        },

        async resolveCancelEditBio(){
            if (this.isWaiting) return;
            this.isEditing = false;
        },

        async resolveSaveBio(){
            if (this.isWaiting) return;
            try {
                this.isWaiting = true;

                let text = await this.$refs['textarea'].getValue();
                await new PatchRequest('Users/me/update-bio')
                    .setBody(text).execute();
                this.getUser().Bio = text;
                this.dBio = text;
                this.currentUser = await CurrentUser.getInstance();
                this.currentUser.Bio = text;
                await CurrentUser.setInstance(this.currentUser);
                this.isEditing = false;
                this.refreshBio();
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.isWaiting = false;
            }
        },
    },
    inject: {
        getUser: {},
        getPopupManager: {},
        getToastManager: {},
    }
}

</script>

<style scoped>

.prb-resume-bio{
    width: 100%;
}

.prb-bio-viewer{
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
    width: 100%;
    gap: 8px;
}

.prb-bio-editor{
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: center;
    gap: 8px;
    width: 100%;
}

.prb-text-area{
    width: 100%;
}

.prb-buttons{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: space-between;
    width: 100%;
}

.prb-buttons > * {
    flex-shrink: 0;
}

.prb-buttons__right{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: space-between;
    gap: 8px;
}

.prb-buttons__right > * {
    flex-shrink: 0;
}

.prb-devider{
    width: 100%;
    height: 1.5px;
    background-color: var(--primary-color-200);
}

.prb-button{
    width: fit-content;
}

</style>

