

<template>
    <div class="p-profile-edit-information-content">
        <div class="p-pei-card card">
            <div class="p-pei-header">
                <div class="p-profile-card-heading">
                    Chỉnh sửa thông tin chi tiết 
                </div>
                <div class="p-profile-card-description">
                    Hãy cập nhật thông tin cá nhân của bạn để mọi người biết rõ hơn về bạn nhé! 
                    Thông tin cá nhân giúp bạn kết nối với những người bạn mới, 
                    cũng như giúp bạn bè hiểu rõ hơn về bạn hơn.  <br/>
                    Hãy chia sẻ thông tin cá nhân của bạn để tạo ra một môi trường kết nối tốt hơn nhé!
                </div>
            </div>
            
            <div class="p-pei-form">
                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Biệt danh
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Biệt danh của bạn là gì? 
                        Hãy chia sẻ tên gọi thân mật của bạn để bạn bè dễ dàng nhận ra và gọi tên bạn một cách thân thương. 
                        Biệt danh sẽ làm cho bạn trở nên gần gũi và dễ mến hơn trong mắt mọi người.
                    </div>
                    <div class="p-pec-input-content">
                        <MTextfield 
                            placeholder="Tối đa 50 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Nickname"
                            state="normal" ref="Nickname"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Giới tính
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Chọn giới tính của bạn để mọi người biết cách xưng hô đúng nhé! 
                        Dù là nam, nữ hay không xác định, bạn đều là chính mình. 
                        Hãy tự tin thể hiện bản thân và để mọi người tôn trọng giới tính của bạn.
                    </div>
                    <div class="p-pec-input-content">
                        <MRadio
                            :is-show-title="false" :is-show-error="false" :is-obligate="false"
                            :items="genderItems" direction="row" group="gender-2"
                            ref="Gender" :validator="null"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Ngày sinh
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Ngày sinh của bạn là khi nào? 
                        Hãy để mọi người biết để không ai quên gửi lời chúc mừng sinh nhật đến bạn nhé! 
                        Ngày sinh cũng là dịp để bạn nhận được những món quà và lời chúc tốt đẹp từ bạn bè.
                    </div>
                    <div class="p-pec-input-content">
                        <MDatePicker 
                            placeholder="dd/mm/yy"
                            :is-show-title="false" :is-show-error="true" :is-obligate="false"
                            :validator="validators.DateOfBirth"
                            state="normal" ref="DateOfBirth"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Quê quán
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Bạn đến từ đâu? 
                        Hãy cho mọi người biết quê quán của bạn để xem có ai là đồng hương không nhé! 
                        Biết đâu bạn sẽ gặp được những người bạn cũ hoặc kết nối với những người đồng hương mới.
                    </div>
                    <div class="p-pec-input-content">
                        <MTextfield 
                            placeholder="Tối đa 255 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Country"
                            state="normal" ref="Country"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Địa chỉ
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Địa chỉ hiện tại của bạn ở đâu? 
                        Để lỡ có dịp nào đó, bạn bè có thể gửi quà hay thư tay đến cho bạn. 
                        Chia sẻ địa chỉ cũng giúp bạn kết nối với những người bạn sống gần khu vực của mình.
                    </div>
                    <div class="p-pec-input-content">
                        <MTextfield 
                            placeholder="Tối đa 255 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Address"
                            state="normal" ref="Address"
                        />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import { LimitItemNumberValidator, GreaterThanTodayValidator } from '@/js/utils/validator';
import MTextfield from './../../../../../base/inputs/MTextfield.vue'
import MRadio from './../../../../../base/inputs/MRadio.vue'
import MDatePicker from '@/components/base/inputs/MDatePicker.vue';
import { myEnum } from '@/js/resources/enum';
import profileEditContent from './profile-edit-content';

export default {
    name: 'ProfileEditInformationContent',
    components: {
        MTextfield,
        MRadio,
        MDatePicker
    },
    props: {
    },
    data(){
        return {
            keys: ["Nickname", "Gender", "DateOfBirth", "Country", "Address"],
            validators: {
                Nickname: new LimitItemNumberValidator("Biệt danh không được vượt quá 50 ký tự")
                    .setBoundary(null, 50),
                Gender: null,
                DateOfBirth: new GreaterThanTodayValidator("Ngày sinh không được lớn hơn ngày hiện tại"),
                Country: new LimitItemNumberValidator("Quê quán không được vượt quá 255 ký tự")
                    .setBoundary(null, 255),
                Address: new LimitItemNumberValidator("Địa chỉ không được vượt quá 255 ký tự")
                    .setBoundary(null, 255),
            },
            genderItems: [
                {
                    label: "Nam",
                    value: myEnum.EGender.Male,
                },
                {
                    label: "Nữ",
                    value: myEnum.EGender.Female,
                },
                {
                    label: "Khác",
                    value: myEnum.EGender.Other,
                },
            ],
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
                    Nickname: this.$refs.Nickname,
                    Gender: this.$refs.Gender,
                    DateOfBirth: this.$refs.DateOfBirth,
                    Country: this.$refs.Country,
                    Address: this.$refs.Address,
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

.p-pei-card{
    width: 100%;
    padding: 16px;
    padding-bottom: 96px;

    display: flex;
    flex-flow: column nowrap;
    align-items: stretch;
    justify-content: flex-start;

    gap: 64px;
}

.p-pei-card > * {
    text-align: left;
}

.p-pei-header{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
    gap: 16px;
}


.p-pei-form{
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 64px;
}

.p-pei-form .p-input-item{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 8px;
}

.p-pec-input-content{
    width: 50%;
}

</style>

