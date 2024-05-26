

<template>
    <div class="p-course-credit-introduction-content">
        <div class="p-cci-card card">
            <div class="p-cci-header">
                <div class="card-heading">
                    Giới Thiệu Khóa Học - Kể Câu Chuyện Của Bạn!
                </div>
                <div class="card-description">
                    Đây là phần bạn tỏa sáng và khiến mọi người phải chú ý! <br />
                    Hãy tạo nên một đoạn giới thiệu chi tiết, đầy đủ và hấp dẫn để mọi người cảm thấy khóa học của bạn là thứ họ không thể bỏ lỡ. 
                    Giống như việc viết kịch bản cho một bộ phim bom tấn, hãy khiến người đọc cảm thấy hứng thú ngay từ những dòng chữ đầu tiên.
                </div>
            </div>
            
            <div class="p-cci-form">
                <div class="p-input-item">
                    <div class="p-input-heading card-subheading">
                        Soạn thảo nội dung giới thiệu
                    </div>
                    <div class="p-input-description card-description">
                        Đây là cơ hội để bạn thuyết phục các bạn sinh viên khác tham gia khóa học của mình. 
                        Hãy viết một cách tự nhiên, lôi cuốn và truyền tải hết niềm đam mê của bạn! 
                        Hãy kể về những điều thú vị mà họ sẽ được trải nghiệm, những kiến thức “bí kíp” họ sẽ học được và những giá trị tuyệt vời mà khóa học mang lại. <br />
                        Ví dụ: “Bạn sẽ trở thành bậc thầy trong việc nấu mì gói, làm bạn bè trầm trồ vì kỹ năng nâng tầm món ăn của mình!” hay “Hãy sẵn sàng để trở thành một chuyên gia lập trình, từ cơ bản đến nâng cao, chỉ trong vài tuần và khiến bạn bè phải ghen tỵ!
                        <br /> <br />
                        Tóm tắt ngắn gọn nhưng đầy đủ về những gì khóa học của bạn sẽ bao gồm. 
                        Đây là phần để bạn mô tả tổng quan về những điều mà người tham gia sẽ học được. <br />
                        Ví dụ: “Khóa học này sẽ bao gồm tất cả mọi thứ từ cơ bản đến nâng cao trong lập trình Python, với nhiều bài tập thực hành thú vị để bạn không bao giờ thấy chán!”
                        <br /> <br />
                        Hãy liệt kê các chương hoặc bài học sẽ có trong khóa học của bạn. Điều này giúp các bạn sinh viên biết được lộ trình học tập rõ ràng. Ví dụ:
                            <li>Chương 1: Nhập môn lập trình Python </li>
                            <li>Chương 2: Cấu trúc dữ liệu và giải thuật </li>
                            <li>Chương 3: Lập trình hướng đối tượng </li>
                            <li>Chương 4: Xây dựng ứng dụng web với Flask </li>
                            <li>Chương 5: Dự án cuối khóa - Xây dựng ứng dụng thực tế </li>
                    </div>
                    <div class="p-ccg-input-content p-markdown-editor">
                        <MarkdownEditor ref="Introduction" :validator="validators.Introduction" />
                    </div>
                </div>
                
            </div>
        </div>
    </div>
</template>



<script>
import { NotEmptyValidator } from '@/js/utils/validator';
import MarkdownEditor from '../../../post-detail/components/MarkdownEditor.vue';
import courseCreditContent from './course-credit-content';

export default {
    name: 'CourseCreditIntroductiontContent',
    components: {
        MarkdownEditor
    },
    props: {
    },
    data(){
        return {
            keys: ["Introduction"],
            validators: {
                Introduction: new NotEmptyValidator("Nội dung giới thiệu không được để trống"),
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
        ...courseCreditContent.methods,

        async refresh(){
            try {
                this.components = {
                    Introduction: this.$refs.Introduction,
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

.p-cci-card{
    width: 100%;
    padding: 16px;
    padding-bottom: 96px;

    display: flex;
    flex-flow: column nowrap;
    align-items: stretch;
    justify-content: flex-start;

    gap: 64px;
}

.p-cci-card > * {
    text-align: left;
}

.p-cci-header{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
    gap: 16px;
}


.p-cci-form{
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 64px;
}

.p-cci-form .p-input-item{
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

.p-ccg-input-content.p-markdown-editor{
    width: 100%;
    height: 750px;
}

</style>

