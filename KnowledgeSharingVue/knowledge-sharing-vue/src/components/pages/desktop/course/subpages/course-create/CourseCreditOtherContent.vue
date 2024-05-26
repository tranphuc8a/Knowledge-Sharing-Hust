

<template>
    <div class="p-course-credit-general-content">
        <div class="p-ccg-card card">
            <div class="p-ccg-header">
                <div class="card-heading">
                    Thông Tin Khác - Những Gì Còn Lại!
                </div>
                <div class="card-description">
                    Cuối cùng nhưng không kém phần quan trọng, đây là nơi bạn xác định các chi tiết “nho nhỏ mà có võ”! 
                    Hãy chắc chắn rằng mọi thứ đều rõ ràng và cụ thể. 
                    Tại đây, bạn có thể chia sẻ về quyền riêng tư của khóa học, ước lượng thời gian cần thiết để hoàn thành và danh mục các chủ đề mà khóa học của bạn sẽ bao gồm. <br/>
                    Nào, cùng hoàn thiện nhé!
                </div>
            </div>
            
            <div class="p-ccg-form">
                <div class="p-input-item">
                    <div class="p-input-heading card-subheading">
                        Danh mục khóa học
                    </div>
                    <div class="p-input-description card-description">
                        Chọn các danh mục phù hợp để khóa học của bạn dễ dàng được tìm thấy bởi các sinh viên khác. 
                        Bạn có thể chọn ít nhất 2 và tối đa 5 danh mục. <br />
                        Ví dụ: "Lập trình, Kỹ năng sống, Kinh doanh, Thể thao". <br/>
                        Hãy tưởng tượng như bạn đang chọn topping cho chiếc pizza của mình vậy! Đừng chọn hết trừ khi bạn muốn pizza của mình thành... đống lộn xộn!"
                    </div>
                    <div class="p-ccg-input-content">
                        <CategoryInput :label="'Nhập category'" 
                            ref="Categories" 
                            :validator="validators.Categories"
                            :is-show-title="false" :is-show-error="true" :is-obligate="false"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading card-subheading">
                        Quyền riêng tư
                    </div>
                    <div class="p-input-description card-description">
                        Bạn muốn khóa học của mình công khai (public) hay riêng tư (private)? 
                        Nếu công khai, hãy để mọi người đều có thể tham gia và học hỏi từ bạn. 
                        Nếu riêng tư, chỉ những người tham gia mới có thể thấy được. 
                    </div>
                    <div class="p-ccg-input-content">
                        <MRadio
                            :is-show-title="false" :is-show-error="false" :is-obligate="false"
                            :items="privacyItems" direction="column" group="privacy-2"
                            ref="Privacy" :validator="null"
                            :onchange="()=>{}" :value="privacy"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading card-subheading">
                        Ước lượng thời gian học
                    </div>
                    <div class="p-input-description card-description">
                        Cung cấp một con số ước lượng về thời gian cần thiết để hoàn thành khóa học. 
                        Điều này giúp các bạn sinh viên dễ dàng lên kế hoạch cho lịch học của họ. <br/>
                        (Đơn vị: phút)
                    </div>
                    <div class="p-ccg-input-content">
                        <MTextfield 
                            placeholder="Nhập thời gian học" type="number"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false"
                            :validator="validators.EstimateTimeInMinutes" :value="0"
                            state="normal" ref="EstimateTimeInMinutes"
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
import MRadio from './../../../../../base/inputs/MRadio.vue';
import CategoryInput from '@/components/base/category/CategoryInput.vue';
import courseCreditContent from './course-credit-content';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'CourseCreditOtherContent',
    components: {
        MTextfield,
        MRadio, CategoryInput
    },
    props: {
    },
    data(){
        return {
            keys: ["Privacy", "EstimateTimeInMinutes", "Categories"],
            validators: {
                Privacy: null,
                EstimateTimeInMinutes: new RangeNumberValidator("Thời gian học không hợp lệ").setBoundary(0, null)
                    .setIsAcceptEmpty(false, "Thời gian học không được để trống"),
                Categories: new LimitItemNumberValidator("Số lượng danh mục phải từ 2 đến 5 loại khác nhau").setBoundary(2, 5),
            },
            components: {
            },
            privacyItems: [
                { label: "Công khai", value: myEnum.EPrivacy.Public },
                { label: "Riêng tư", value: myEnum.EPrivacy.Private },
            ],
            privacy: myEnum.EPrivacy.Public,
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
        ...courseCreditContent.methods,

        async refresh(){
            try {
                this.components = {
                    Privacy: this.$refs.Privacy,
                    EstimateTimeInMinutes: this.$refs.EstimateTimeInMinutes,
                    Categories: this.$refs.Categories,
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

.p-course-credit-information-content{
    width: 100%;
}

.p-ccg-card{
    width: 100%;
    padding: 16px;
    padding-bottom: 96px;

    display: flex;
    flex-flow: column nowrap;
    align-items: stretch;
    justify-content: flex-start;

    gap: 64px;
}

.p-ccg-card > * {
    text-align: left;
}

.p-ccg-header{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
    gap: 16px;
}


.p-ccg-form{
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 64px;
}

.p-ccg-form .p-input-item{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 8px;
}

.p-ccg-input-content{
    width: 50%;
}

</style>

