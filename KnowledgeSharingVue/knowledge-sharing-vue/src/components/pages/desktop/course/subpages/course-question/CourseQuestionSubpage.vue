

<template>
    <div class="p-course-question-subpage p-profile-main-subpage">
        <div class="p-course-question-subpage__left">
            <!-- CourseQuestionToolbar -->
            <CourseQuestionToolbar 
                :on-click-search="resolveOnChangeConfig"
            />
        </div>

        <div class="p-course-question-subpage__right">
            <!-- CourseQuestionFeedSubpage -->
            <PostSubpage 
                :get-post="getPostCallback"
            >
                <template #addpost>
                    <CourseAddQuestionCard />
                </template>
            </PostSubpage>
        </div>
    </div>
</template>



<script>
import CourseQuestionToolbar from './CourseQuestionToolbar.vue';
import PostSubpage from '../../../home/sub-pages/PostSubpage.vue';
import { Validator } from '@/js/utils/validator';
import { GetRequest } from '@/js/services/request';
import CourseAddQuestionCard from './CourseAddQuestionCard.vue';

export default {
    name: 'CourseQuestionSubpage',
    components: {
        CourseQuestionToolbar,
        PostSubpage,
        CourseAddQuestionCard,
    },
    props: {
    },
    data(){
        return {
            getPostCallback: null,
        }
    },
    async mounted(){
        try {
            await this.resolveOnChangeConfig({});
        } catch (error){
            console.error(error);
        }
    },
    methods: {

        getOrderString(config){
            let { isLatest, isMostStar, isMostComment } = config;

            let orderCreatedTime, orderStar, orderComment;
            if (isLatest === true || isLatest === false){
                orderCreatedTime = "CreatedTime:" + (isLatest ? "desc" : "asc");
            }
            if (isMostStar === true || isMostStar === false){
                orderStar = "TotalStar:" + (isMostStar ? "desc" : "asc");
            }
            if (isMostComment === true || isMostComment === false){
                orderComment = "TotalComment:" + (isMostComment ? "desc" : "asc");
            }
            let orders = [orderCreatedTime, orderStar, orderComment].filter(x => x?.length > 0).join(",");

            return orders;
        },

        getFilterString(config){
            let { username } = config;

            let filterUsername = "";
            if (Validator.isNotEmpty(username)){
                filterUsername = "Username:" + username;
            }
            let filters = [filterUsername].filter(x => x?.length > 0).join(",");

            return filters;
        },


        async resolveOnChangeConfig(config){
            try {
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;

                if (config === null || config === undefined){
                    config = {};
                }
                let { text } = config;
                
                // Get url
                let url = "Courses/questions/" + courseId;
                if (Validator.isNotEmpty(text)){
                    url = "Courses/search/questions/" + courseId;
                }
                
                // Get orderString
                let orders = this.getOrderString(config);
                // Get filterString
                let filters = this.getFilterString(config);
                let params = {
                    search: text,
                    order: orders,
                    filter: filters,
                }

                // create async callback function:
                let callback = async function(limit, offset){
                    let param = {
                        ...params,
                        limit: limit,
                        offset: offset,
                    };
                    let res = await new GetRequest(url).setParams(param).execute();
                    return res;
                }.bind(this);

                this.getPostCallback = callback;
            } catch (error){
                console.error(error);
            }
        },
    },
    inject: {
        getCourse: {}
    }
}

</script>

<style>
.p-profile-main-subpage{
    max-width: var(--profile-page-max-width);
    width: 100%;
    height: fit-content;
}
</style>

<style scoped>

.p-course-question-subpage{
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

.p-course-question-subpage__left{
    width: 0;
    flex-shrink: 1;
    flex-grow: 2;

    position: sticky;
    top: 16px;

    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-course-question-subpage__right{
    width: 0;
    flex-shrink: 1;
    flex-grow: 3;
}

</style>

