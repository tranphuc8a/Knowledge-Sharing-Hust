

<template>
    <div class="p-feed-subpage" v-if="!isLoaded">
        <slot name="addcourse"></slot>

        <div class="p-list-course-card">
            <div :style="{
                    width: `calc((100% - ${(rowCount - 1)*16}px)/${rowCount})`,
                }"
            >
                <CourseShortCardSkeleton />
            </div>
            <div :style="{
                    width: `calc((100% - ${(rowCount - 1)*16}px)/${rowCount})`,
                }"
            >
                <CourseShortCardSkeleton />
            </div>
            <div :style="{
                    width: `calc((100% - ${(rowCount - 1)*16}px)/${rowCount})`,
                }"
            >
                <CourseShortCardSkeleton />
            </div>
        </div>
    </div>


    <div class="p-feed-subpage" v-if="isLoaded" ref="list" >
        <slot name="addcourse"></slot>

        <div class="p-feed-notfound" v-show="isOutOfCourse && !(listCourses.length > 0)">
            <NotFoundPanel text="Không tìm thấy mục nào" />
        </div>

        <div class="p-list-course-card">
            <div v-for="item in listCourses" :key="item?.UserItemId"
                :style="{
                    width: `calc((100% - ${(rowCount - 1)*16}px)/${rowCount})`,
                }"
            >
                <CourseShortCard :course="item">
                    <template #courseMenuContext>
                        <CourseShortCardMenuContext/>
                    </template>
                </CourseShortCard>
            </div>
            
            <div v-if="!isOutOfCourse"
                :style="{
                    width: `calc((100% - ${(rowCount - 1)*16}px)/${rowCount})`,
                }"
            >
                <CourseShortCardSkeleton />
            </div>
            <div v-if="!isOutOfCourse"
                :style="{
                    width: `calc((100% - ${(rowCount - 1)*16}px)/${rowCount})`,
                }"
            >
                <CourseShortCardSkeleton />
            </div>
            <div v-if="!isOutOfCourse"
                :style="{
                    width: `calc((100% - ${(rowCount - 1)*16}px)/${rowCount})`,
                }"
            >
                <CourseShortCardSkeleton />
            </div>

        </div>
    </div>

    
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import CourseShortCard from '../../../course/components/course-card/CourseShortCard.vue';
import CourseShortCardSkeleton from '../../../course/components/course-card/CourseShortCardSkeleton.vue';
import CourseShortCardMenuContext from '../../../course/components/course-card/CourseShortCardMenuContext.vue';

import ResponseCourseModel from '@/js/models/api-response-models/response-course-model';

import CurrentUser from '@/js/models/entities/current-user';
import { Request } from '@/js/services/request';

export default {
    name: 'PostSubpage',
    components: {
        CourseShortCard,
        CourseShortCardMenuContext,
        CourseShortCardSkeleton,
        NotFoundPanel,
    },
    props: {
        getCourse: {
            required: async function(limit, offset){
                console.log("Get course at " + limit + " " + offset);
                return [];
            }
        },
        rowCount: {
            default: 3,
        }
    },
    data(){
        return {
            isLoaded: false,
            isOutOfCourse: false,
            isLoadingMore: false,
            listCourses: [],
            currentUser: null,
        }
    },
    created(){
        this.refresh();
    },
    mounted(){
        this.registerScrollHandler(this.resolveOnScroll);
    },
    methods: {
        async resolveOnScroll(scrollContainer){
            try {
                if (scrollContainer == null) return;
                if (this.isOutOfCourse || this.isLoadingMore){
                    return;
                }

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averageCourseHeight = 500;
                let leftPostNumber = 5;

                if (scrollHeight - scrollPosition < averageCourseHeight * leftPostNumber){
                    // console.log("Load more post");
                    this.loadMorePost();
                }
            } catch (e){
                console.error(e);
            }
        },

        async loadMorePost(){
            if (this.isOutOfCourse || this.isLoadingMore){
                return;
            }
            try {
                this.isLoadingMore = true;
                
                // get more course
                let offset = this.listCourses?.length ?? 0;
                let limit = 10;
                if (this.getCourse == null) return;
                let res = await this.getCourse(limit, offset);
                
                let body = await Request.tryGetBody(res);
                if (!(body?.length > 0) && (body?.Results?.length > 0)){
                    body = body.Results;
                }
                let tempListCourses = body.map(function(post){
                    return new ResponseCourseModel().copy(post);
                });
                if (tempListCourses.length < limit){
                    this.isOutOfCourse = true;
                } 
                
                if (tempListCourses.length > 0){
                    let listLoadedIds = this.listCourses.map(function(course){
                        return course?.UserItemId;
                    });
                    tempListCourses = tempListCourses.filter(function(course){
                        return !listLoadedIds.includes(course?.UserItemId);
                    });
                    this.listCourses = this.listCourses.concat(tempListCourses);
                }

            } catch (e){
                Request.resolveAxiosError(e);
                this.isOutOfCourse = true;
            } finally {
                this.isLoadingMore = false;
            }
        },

        async refresh(){
            try {
                this.isLoaded = false;
                this.isOutOfCourse = false;
                this.currentUser = await CurrentUser.getInstance();
                this.listCourses = [];
                if (this.getCourse == null) return;
                await this.loadMorePost();
                this.isLoaded = true;
            } catch (e){
                console.error(e);
            }
        },

        async resolveDeletedCourse(courseId){
            try {
                let index = this.listCourses.findIndex(function(course){
                    return course?.UserItemId == courseId;
                });
                if (index >= 0){
                    this.listCourses.splice(index, 1);
                }
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
        registerScrollHandler: {},
    },
    watch: {
        getCourse(){
            this.refresh();
        },
    },
    provide(){
        return {
            onCourseDeleted: this.resolveDeletedCourse,
        }
    }
}

</script>

<style scoped>

.p-feed-subpage{
    width: 100%;
    gap: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
}

.p-feed-notfound{
    width: 100%;
    height: 200px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-list-course-card{
    width: 100%;
    gap: 16px;
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: flex-start;
}

.p-list-course-card > *{
    flex-shrink: 0;
    flex-grow: 0;
}


</style>

