

<template>
    <div class="p-profile-edit-career-content">
        <div class="p-pec-card card">
            <div class="p-pec-header">
                <div class="p-profile-card-heading">
                    Chỉnh sửa thông tin học tập, nghề nghiệp
                </div>
                <div class="p-profile-card-description">
                    Hãy cập nhật thông tin học tập, nghề nghiệp của bạn để mọi người biết rõ hơn về bạn nhé! 
                    Thông tin học tập, nghề nghiệp giúp bạn kết nối với những người bạn mới, 
                    cũng như giúp bạn bè hiểu rõ hơn về bạn hơn. <br/>
                    Hãy chia sẻ thông tin học tập, nghề nghiệp của bạn để tạo ra một môi trường kết nối tốt hơn nhé!
                </div>
            </div>
            
            <div class="p-pec-form">
                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Trường học
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Bạn học trường nào? Hãy chia sẻ tên trường để mọi người biết bạn là 'thần đồng' từ đâu ra nhé! 
                        Biết đâu bạn sẽ tìm được những người bạn mới học cùng trường hay thậm chí là cùng lớp.
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Nhập tên trường học"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.School"
                            state="normal" ref="School"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Khóa
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Bạn học khóa bao nhiêu? Hãy chia sẻ năm bạn nhập học để xem có ai cùng khóa không nhé! 
                        Khóa học của bạn sẽ giúp mọi người biết bạn là 'ma mới' hay 'ma cũ' của trường.
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Nhập khóa học. Ex 64, 65..." type="number"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Grade"
                            state="normal" ref="Grade"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Lớp học
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Lớp học cụ thể của bạn là gì? Hãy để bạn bè dễ dàng nhận ra bạn trong đám đông và gọi bạn bằng tên lớp thân thương. 
                        Lớp học cũng là nơi bạn có thể gặp gỡ và làm quen với nhiều bạn mới.
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Tối đa 255 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Class"
                            state="normal" ref="Class"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Chuyên ngành
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Chuyên ngành bạn đang theo học là gì? Hãy cho mọi người biết bạn đang đắm chìm trong lĩnh vực nào nhé! 
                        Để khi cần, bạn bè có thể tìm đến bạn để học hỏi thêm kiến thức chuyên sâu..
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Tối đa 255 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Profession"
                            state="normal" ref="Profession"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        CPA
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Điểm trung bình tích lũy (CPA) của bạn là bao nhiêu? 
                        Hãy tự hào chia sẻ thành tích học tập của mình để mọi người biết bạn học giỏi đến mức nào nhé! 
                        Điểm CPA cao sẽ giúp bạn có thêm nhiều cơ hội học bổng và việc làm.
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Nhập số thực. Ex 4.0, 3.66..." type="number"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Cpa"
                            state="normal" ref="Cpa"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading p-profile-card-subheading">
                        Công việc
                    </div>
                    <div class="p-input-description p-profile-card-description">
                        Bạn đang làm công việc gì? Hãy chia sẻ để mọi người biết bạn đang 'cày cuốc' ở lĩnh vực nào và có thể tìm kiếm sự giúp đỡ khi cần. 
                        Công việc của bạn sẽ là điều thú vị để mọi người hiểu thêm về cuộc sống và kinh nghiệm của bạn.
                    </div>
                    <div class="p-input-content">
                        <MTextfield 
                            placeholder="Tối đa 255 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Job"
                            state="normal" ref="Job"
                        />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import { LimitItemNumberValidator, RangeNumberValidator } from '@/js/utils/validator';
import MTextfield from './../../../../../base/inputs/MTextfield.vue';
import profileEditContent from './profile-edit-content';

export default {
    name: 'ProfileEditCareertContent',
    components: {
        MTextfield,
    },
    props: {
    },
    data(){
        return {
            keys: ["School", "Grade", "Class", "Profession", "Cpa", "Job"],
            validators: {
                School: new LimitItemNumberValidator("Giá trị không được vượt quá 255 ký tự"),
                Grade: new LimitItemNumberValidator("Giá trị không được vượt quá 255 ký tự"),
                Class: new LimitItemNumberValidator("Giá trị không được vượt quá 255 ký tự"),
                Profession: new LimitItemNumberValidator("Giá trị không được vượt quá 255 ký tự"),
                Cpa: new RangeNumberValidator("Điểm CPA phải là số thực trong khoảng từ 0 đến 4.0")
                    .setBoundary(0, 4.0),
                Job: new LimitItemNumberValidator("Giá trị không được vượt quá 255 ký tự"),
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
                    School: this.$refs.School,
                    Grade: this.$refs.Grade,
                    Class: this.$refs.Class,
                    Profession: this.$refs.Profession,
                    Cpa: this.$refs.Cpa,
                    Job: this.$refs.Job,
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

