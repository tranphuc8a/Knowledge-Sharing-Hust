
export default {
    vi: {
        addpostcard: {
            remind(fullname){
                return `Chào ${fullname}, hãy tạo một bài học hoặc một cuộc thảo luận nào`;
            },
            addLesson: 'Tạo bài học',
            addQuestion: 'Tạo cuộc thảo luận'
        }
    }, en: {
        addpostcard: {
            remind(fullname){
                return `Hello ${fullname}, let create a lesson or a discussion`;
            },
            addLesson: 'Create a lesson',
            addQuestion: 'Create a discussion'
        }
    }

}