

<template>
    <div class="p-course-credit-general-content">
        <div class="p-ccg-card card">
            <div class="p-ccg-header">
                <div class="card-heading">
                    Thông Tin Cơ Bản - Bước Đầu Cho Sự Thành Công!
                </div>
                <div class="card-description">
                    Chào mừng bạn đến với bước đầu tiên trong hành trình tạo nên khóa học đỉnh cao! <br/>
                    Để bắt đầu, chúng ta cần một vài thông tin cơ bản nhưng cực kỳ quan trọng. 
                    Đây là nơi bạn sẽ đặt tên, chọn ảnh đại diện thật ngầu, và viết một đoạn mô tả hấp dẫn cho khóa học của bạn. 
                    Hãy xem đây như là bìa sách của bạn, phải thật ấn tượng để khiến người ta muốn “mở” nó ra nhé! 😎💡 <br/>
                    Nào, cùng làm cho khóa học của bạn trở nên thật "chất" ngay từ những bước đầu!
                </div>
            </div>
            
            <div class="p-ccg-form">
                <div class="p-input-item">
                    <div class="p-input-heading card-subheading">
                        Tiêu đề khóa học
                    </div>
                    <div class="p-input-description card-description">
                        Hãy nghĩ đến một cái tên thật “kêu”, thật “chất” cho khóa học của bạn! 
                        Tựa như bạn đang đặt tên cho một siêu anh hùng mới vậy. 
                        Đừng ngại sáng tạo! <br />
                        Ví dụ: “Lập Trình Siêu Cấp Vũ Trụ” hoặc “Nghệ Thuật Ngủ Trong Lớp Và Vẫn Qua Môn”. <br />
                        Tiêu đề càng độc đáo, càng thu hút mọi người!
                    </div>
                    <div class="p-ccg-input-content">
                        <MTextfield 
                            placeholder="Nhập tiêu đề khóa học"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Title"
                            state="normal" ref="Title"
                        />
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading card-subheading">
                        Ảnh thumbnail khóa học
                    </div>
                    <div class="p-input-description card-description">
                        Hình ảnh nói lên tất cả! Hãy chọn một bức ảnh thumbnail thật lung linh và ấn tượng. 
                        Hình ảnh này sẽ là bộ mặt của khóa học trên trang web. 
                        Tránh những bức ảnh "tự sướng" trong phòng tối hay chú mèo đang ngủ (trừ khi khóa học của bạn là về mèo). <br/> 
                        Ví dụ: nếu khóa học của bạn là về nấu ăn, một bức ảnh chụp món ăn ngon tuyệt chắc chắn sẽ hấp dẫn hơn là bức ảnh chụp bát mì gói bạn ăn tối qua!
                    </div>
                    <div class="p-ccg-input-content p-input-thumbnail">
                        <!-- Input Image -->
                        <div class="p-ccg-input-image-frame">
                            <MImageInput 
                                :style="imageInputStyle"
                                :is-show-title="false" :is-show-error="false" :is-obligate="false"
                                :validator="null"
                                state="normal"
                                ref="Thumbnail"
                            />
                        </div>
                    </div>
                </div>

                <div class="p-input-item">
                    <div class="p-input-heading card-subheading">
                        Mô tả khóa học
                    </div>
                    <div class="p-input-description card-description">
                        Đây là cơ hội để bạn “quảng cáo” khóa học của mình! 
                        Viết một đoạn mô tả ngắn gọn nhưng đầy sức hút để học viên muốn đăng ký ngay. 
                        Tưởng tượng bạn đang viết lời tựa cho một bộ phim bom tấn vậy. 
                        Hãy làm cho học viên cảm thấy khóa học này chính là điều họ đang tìm kiếm bấy lâu. <br /> 
                        Ví dụ: “Khóa học này sẽ biến bạn từ một 'gà mờ' thành chuyên gia lập trình chỉ trong 30 ngày!” hay “Chinh phục tiếng Anh giao tiếp với 5 bí kíp siêu dễ nhớ và thực hành ngay lập tức!”.
                    </div>
                    <div class="p-ccg-input-content p-input-abstract">
                        <!-- <MTextfield 
                            placeholder="Tối đa 1000 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Abstract"
                            state="normal" ref="Abstract"
                        /> -->
                        <MTextArea
                            placeholder="Tối đa 1000 ký tự"
                            :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false" 
                            :validator="validators.Abstract"
                            state="normal" ref="Abstract"
                            max-height="180px" rows="3"
                        />
                    </div>
                </div>

                
            </div>
        </div>
    </div>
</template>



<script>
import { LimitItemNumberValidator } from '@/js/utils/validator';
import MTextfield from './../../../../../base/inputs/MTextfield.vue';
import MImageInput from './../../../../../base/inputs/MImageInput.vue';
import MTextArea from './../../../../../base/inputs/MTextArea.vue'
import courseCreditContent from './course-credit-content';

export default {
    name: 'CourseCreditGeneraltContent',
    components: {
        MTextfield,
        MImageInput,
        MTextArea
    },
    props: {
    },
    data(){
        return {
            keys: ["Title", "Thumbnail", "Abstract"],
            validators: {
                Title: new LimitItemNumberValidator("Giá trị không được vượt quá 1000 ký tự")
                    .setIsAcceptEmpty(false, "Tiêu đề không được để trống"),
                Abstract: new LimitItemNumberValidator("Giá trị không được vượt quá 1000 ký tự").setBoundary(null, 1000)
                    .setIsAcceptEmpty(false, "Mô tả không được để trống"),
            },
            components: {
            },
            imageInputStyle: {
                width: '200px',
                height: '200px'
            }
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
                    Title: this.$refs.Title,
                    Abstract: this.$refs.Abstract,
                    Thumbnail: this.$refs.Thumbnail,
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

.p-ccg-input-content.p-input-thumbnail{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.p-ccg-input-image-frame{
    width: fit-content;
}
.p-ccg-input-content.p-input-abstract{
    width: 75%;
}

</style>

