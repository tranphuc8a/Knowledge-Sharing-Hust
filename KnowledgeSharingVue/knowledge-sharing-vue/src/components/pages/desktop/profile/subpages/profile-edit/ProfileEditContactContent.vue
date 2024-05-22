

<template>
    <div class="p-profile-edit-contact-content">
        <div class="p-pec-card card">
            <div class="p-pec-header">
                <div class="p-profile-card-heading">
                    Chỉnh sửa thông tin liên hệ 
                </div>
                <div class="p-profile-card-description">
                    Hãy cập nhật thông tin liên hệ của bạn để mọi người có thể kết nối với bạn dễ dàng hơn! 
                    Thông tin liên hệ giúp bạn bè liên lạc với bạn một cách nhanh chóng và thuận tiện hơn. <br/>
                    Hãy chia sẻ thông tin liên hệ của bạn để tạo ra một môi trường kết nối tốt hơn nhé!
                </div>
            </div>
            
            <div class="p-pec-form">
                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Số điện thoại
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Số điện thoại của bạn là gì? Để bạn bè có thể liên lạc và tám chuyện với bạn dễ dàng hơn.
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Nhập số điện thoại của bạn"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.PhoneNumber"
                            state="normal" ref="PhoneNumber"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Email liên lạc
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Email liên lạc của bạn là gì? 
                        Hãy chia sẻ để nhận được thông báo quan trọng và những lời nhắn "yêu thương" từ bạn bè. 
                        Email cũng là kênh để bạn giữ liên lạc với các cơ hội học tập và nghề nghiệp.
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Tối đa 255 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.ContactEmail"
                            state="normal" ref="ContactEmail"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Tài khoản mạng xã hội
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Chia sẻ các tài khoản mạng xã hội của bạn đi nào! 
                        Để mọi người có thể theo dõi và kết nối với bạn trên mọi nền tảng. 
                        Hãy khoe những khoảnh khắc đẹp và những điều thú vị trong cuộc sống của bạn trên mạng xã hội.
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Nhập chính xác đường dẫn URL"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.SocialLink"
                            state="normal" ref="SocialLink"
                        />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import { PhoneValidator, EmailValidator, UrlValidator } from '@/js/utils/validator';
import MTextfield from './../../../../../base/inputs/MTextfield.vue';
import profileEditContent from './profile-edit-content';

export default {
    name: 'ProfileEditContactContent',
    components: {
        MTextfield,
    },
    props: {
    },
    data(){
        return {
            keys: ["PhoneNumber", "ContactEmail", "SocialLink"],
            validators: {
                PhoneNumber: new PhoneValidator("Số điện thoại không hợp lệ"),
                ContactEmail: new EmailValidator("Email không hợp lệ"),
                SocialLink: new UrlValidator("Đường dẫn không hợp lệ"),
            },
            components: {
            },
        }
    },
    async mounted(){
        try {
            this.refresh();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        ...profileEditContent.methods,

        async refresh(){
            try {
                this.components = {
                    PhoneNumber: this.$refs.PhoneNumber,
                    ContactEmail: this.$refs.ContactEmail,
                    SocialLink: this.$refs.SocialLink,
                }
            } catch (error){
                console.error(error);
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

.p-profile-edit-information-content{
    width: 100%;
}

.p-pec-card{
    width: 100%;
    padding: 16px;
    padding-bottom: 96px;

    display: flex;
    flex-flow: column nowrap;
    align-items: stretch;
    justify-content: flex-start;

    gap: 64px;
}

.p-pec-card > * {
    text-align: left;
}

.p-pec-header{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
    gap: 16px;
}


.p-pec-form{
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 64px;
}

.p-pec-form .p-input-item{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 8px;
}

.p-input-content{
    width: 50%;
}

</style>

