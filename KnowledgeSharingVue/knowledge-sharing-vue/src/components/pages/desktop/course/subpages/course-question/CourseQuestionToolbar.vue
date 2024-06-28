

<template>
    <div class="p-course-question-toolbar">
        <div class="p-toolbar-card card">
            <div class="p-toolbar-header">
                <div class="p-toolbar-left">
                    Công cụ tìm kiếm
                </div>
                <div class="p-toolbar-right">

                </div>
            </div>
            <div class="p-toolbar-content">
                <div class="p-toolbar-row">
                    <div class=p-toolbar-row__left>
                        <MSelectOption 
                            label="Thời gian" placeholder="--Chọn 1 mục--"
                            :is-show-title="true" :is-show-error="false" :is-obligate="false"
                            :listItemLoader="() => items.time"
                            state="normal" ref="time"
                        />
                    </div>
                    <div class=p-toolbar-row__right>
                        <!-- <MSelectOption 
                            label="Quyền riêng tư" placeholder="--Chọn 1 mục--"
                            :is-show-title="true" :is-show-error="false" :is-obligate="false"
                            :listItemLoader="() => items.privacy"
                            state="normal" ref="privacy"
                        /> -->
                        <MTextfield 
                            label="Username" placeholder="Lọc theo username"
                            :is-show-icon="false" :is-show-title="true" :is-show-error="false" :is-obligate="false"
                            :onclick-icon="()=>{}" 
                            :validator="null"
                            state="normal" ref="username"
                        />
                    </div>
                </div>
                <div class="p-toolbar-row">
                    <div class=p-toolbar-row__left>
                        <MSelectOption 
                            label="Đánh giá" placeholder="--Chọn 1 mục--"
                            :is-show-title="true" :is-show-error="false" :is-obligate="false"
                            :listItemLoader="() => items.star"
                            state="normal" ref="star"
                        />
                    </div>
                    <div class=p-toolbar-row__right>
                        <MSelectOption 
                            label="Lượng bình luận" placeholder="--Chọn 1 mục--"
                            :is-show-title="true" :is-show-error="false" :is-obligate="false"
                            :listItemLoader="() => items.comment"
                            state="normal" ref="comment"
                        />
                    </div>
                </div>
                <div class="p-toolbar-row p-search-row">
                    <MTextfieldButton 
                        label="Tìm kiếm"
                        title="Nhập từ khóa tìm kiếm" placeholder="Tìm kiếm"
                        :is-show-icon="true" 
                        :is-show-title="true" 
                        :is-show-error="false" 
                        :is-obligate="false"
                        :onclick-icon="resolveClickSearch" 
                        :validator="null"
                        state="normal"
                        ref="text"
                    />
                </div>
            </div>
        </div>
    </div>
</template>


<script>
import MTextfieldButton from './../../../../../base/inputs/MTextfieldButton.vue';
import MTextfield from './../../../../../base/inputs/MTextfield.vue'
import MSelectOption from './../../../../../base/inputs/MSelectOption.vue';

/**
 * search config:
 * { text, isLatest, username, isMostStar, isMostComment }
*/

export default {
    name: 'CourseQuestionToolbar',
    components: {
        MTextfieldButton,
        MTextfield,
        MSelectOption
    },
    props: {
        onClickSearch: {
            type: Function,
            default: async function(){
                // console.log(config);
            }
        }
    },
    data(){
        return {
            items: {
                time: [
                    {
                        label: 'Mới nhất',
                        value: true
                    }, {
                        label: 'Cũ nhất',
                        value: false
                    }
                ],
                privacy: [
                    {
                        label: 'Riêng tư',
                        value: true
                    },
                    {
                        label: 'Công khai',
                        value: false
                    }
                ],
                star: [
                    {
                        label: 'Đánh giá cao nhất',
                        value: true
                    },
                    {
                        label: 'Đánh giá thấp nhất',
                        value: false
                    }
                ],
                comment: [
                    {
                        label: 'Nhiều bình luận nhất',
                        value: true
                    },
                    {
                        label: 'Ít bình luận nhất',
                        value: false
                    }
                ]
            }
        }
    },
    mounted(){

    },
    methods: {
        async resolveClickSearch(){
            try {
                // get config
                let components = {
                    time: this.$refs.time,
                    // privacy: this.$refs.privacy,
                    username: this.$refs.username,
                    star: this.$refs.star,
                    comment: this.$refs.comment,
                    text: this.$refs.text
                };
                let config = {
                    text: await components.text.getValue(),
                    isLatest: await components.time.getValue(),
                    // isPrivate: await components.privacy.getValue(),
                    username: await components.username.getValue(),
                    isMostStar: await components.star.getValue(),
                    isMostComment: await components.comment.getValue()
                }

                // call prop callback:
                if (this.onClickSearch != null){
                    await this.onClickSearch(config);
                }
            } catch (error){
                console.error(error);
            }
        }
    }
}

</script>

<style scoped>

.p-course-question-toolbar{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-toolbar-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 32px;
}

.p-toolbar-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 16px;
}

.p-toolbar-left{
    font-family: 'ks-font-semibold';
    font-size: 24px;
}

.p-toolbar-content{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-toolbar-row{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-toolbar-row > * {
    flex-grow: 1;
    width: 0;
}

.p-search-row{
    margin-top: 16px;
    padding-bottom: 16px;
}


</style>

