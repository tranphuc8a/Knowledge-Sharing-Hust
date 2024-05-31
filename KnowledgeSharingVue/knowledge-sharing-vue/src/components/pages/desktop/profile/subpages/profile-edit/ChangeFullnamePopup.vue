

<template>
    <MPopup  ref="popup"
            :isShowDescription="false"
            :isShowPreviousButton="false"
            :isShowCancelButton="true"
            :isShowOkayButton="true"
            :isAutoHide="false"
            v-if="dIsShow"
        
            :on-okay="callbacks.onOkay"
            :on-close="callbacks.onClose"
            :on-cancel="callbacks.onCancel"
            :on-previous="callbacks.onPrevious"

            header="Thay đổi tên hiển thị"
            cancelButtonLabel="Hủy bỏ"
            okayButtonLabel="Thay đổi"
    >
        <div class="p-change-name-popup-content">
            <div class="card-description">
                Nhập tên mới cho tài khoản mạng xã hội học tập của bạn, để bạn bè phải bật cười và thầy cô ngỡ ngàng. Với chỉ một cú gõ phím, bạn có thể biến mình từ một sinh viên bình thường thành một huyền thoại với cái tên đầy hài hước và ấn tượng, khiến mọi người phải tò mò mỗi khi bạn xuất hiện!
            </div>
            <div class="p-devide"></div>
            <div class="p-cnpc-content">
                <div class="p-cnpc-textfield-frame">
                    <MTextfield 
                        placeholder="Nhập tên mới của bạn"
                        :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false"
                        :onclick-icon="()=>{}" 
                        :validator="validator"
                        state="normal" ref="textfield"
                        />
                </div>
            </div>
        </div>
    </MPopup>
</template>



<script>
import { PatchRequest } from '@/js/services/request';
import MPopup from '@/components/base/popup/MPopup.vue';
import MTextfield from '@/components/base/inputs/MTextfield.vue';
import { LimitItemNumberValidator } from '@/js/utils/validator';
import CurrentUser from '@/js/models/entities/current-user';

export default {
    name: 'ChangeFullnamePopup',
    components: {
        MPopup,
        MTextfield,
    },
    props: {
        isShow: {
            type: Boolean,
            default: false
        },
    },
    data(){
        return {
            callbacks: {
                onOkay: this.resolveClickOkay.bind(this),
                onClose: async () => { await this.hide() },
                onCancel: async () => { await this.hide() },
                onPrevious: async () => { await this.hide() },
            },
            dIsShow: this.isShow,
            validator: new LimitItemNumberValidator("Tên quá dài")
                        .setBoundary(null, 100)
                        .setIsAcceptEmpty(false, "Tên không được để trống"),
        }
    },
    async mounted(){
    },
    methods: {
        async show(){
            this.dIsShow = true;
        },
        async hide(){
            this.dIsShow = false;
        },

        async resolveClickOkay(){
            try {
                let textfield = this.$refs.textfield;
                if (! await textfield.validate()) {
                    textfield.startDynamicValidate();
                    textfield.focus();
                    return;
                }
                let value = await textfield.getValue();
                await new PatchRequest('Users/me/update-fullname')
                    .setBody(value).execute();
                this.hide();
                this.getToastManager().success("Thay đổi tên thành công");
                let currentUser = await CurrentUser.getInstance();
                currentUser.FullName = value;
                await CurrentUser.setInstance(currentUser);

                setTimeout(async () => {
                    window.location.reload();
                }, 1000);
            } catch (e){
                console.error(e);
            }
        },
    },
    inject: {
        getToastManager: {},
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-change-name-popup-content {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
    max-width: 750px;
    width: 100%;
    height: fit-content;
    gap: 16px;
}

.p-cnpc-content {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
    padding: 48px 0px;
    gap: 16px;
    width: 100%;
}

.p-cnpc-textfield-frame {
    width: 50%;
    max-width: 750px;
}



</style>

