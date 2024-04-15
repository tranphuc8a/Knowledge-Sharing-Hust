import { Validator } from "@/js/utils/validator";

export default {
    vi: {
        addpostcard: {
            remind(fullname){
                return `Chào ${fullname}, hãy tạo một bài học hoặc một cuộc thảo luận nào`;
            },
            addLesson: 'Tạo bài học',
            addQuestion: 'Tạo cuộc thảo luận'
        },
        postcard: {
            star: 'Đánh giá',
            comment: 'Bình luận',
            viewDetail: 'Xem chi tiết',
            valueStar(number){
                if (Validator.isEmpty(number)) {
                    return 'Đánh giá';
                }
                return `${number} sao`;
            },
            numberStar(number){
                return `${number} đánh giá`;
            },
            numberComment(number){
                return `${number} bình luận`;
            },
            numberView(number){
                return `${number} lượt xem`;
            }
        }
    }, en: {
        addpostcard: {
            remind(fullname){
                return `Hello ${fullname}, let create a lesson or a discussion`;
            },
            addLesson: 'Create a lesson',
            addQuestion: 'Create a discussion'
        },
        postcard: {
            star: 'Rating',
            comment: 'Comment',
            viewDetail: 'View detail',
            valueStar(number){
                if (Validator.isEmpty(number)) {
                    return 'Rating';
                }
                return `${number} stars`;
            },
            numberStar(number){
                return `${number} ratings`;
            },
            numberComment(number){
                return `${number} comments`;
            },
            numberView(number){
                return `${number} views`;
            }
        }
    }

}