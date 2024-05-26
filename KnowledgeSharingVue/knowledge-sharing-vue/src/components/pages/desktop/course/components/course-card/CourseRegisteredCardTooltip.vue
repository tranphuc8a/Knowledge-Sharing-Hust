

<template>
    <div class="p-crc-tooltip">
        <div class="p-crct-card card">
            <div class="p-crctc-top">
                <div class="p-crctc-thumbnail"
                    :click="resolveViewCourseDetail"
                    :style="{
                        'background-image': `url(${courseThumbnail})`
                    }"    
                >
                </div>
            </div>
            <div class="p-crctc-bottom">
                <div class="p-crc-information">
                    <div class="p-crc-title" @:click="resolveViewCourseDetail">
                        {{ dCourse?.Title ?? "" }}
                    </div>
                    <!-- <div class="p-crc-abstract" v-if="dCourse?.Abstract != null">
                        {{ dCourse?.Abstract ?? "" }}
                    </div> -->
                    <div class="p-crc-abstract" v-if="dCourse?.Abstract != null">
                        <EllipsisText 
                            :text="dCourse?.Abstract ?? ''" 
                            :max-line="3"
                            :style="{}"/>
                    </div>
                </div>
                <div class="p-crc-button">
                    <!-- Course Relation Button -->
                    <CourseRelationButton />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import Common from '@/js/utils/common';
import { GetRequest, Request } from '@/js/services/request';
import ResponseCourseModel from '@/js/models/api-response-models/response-course-model';
import { useRouter } from 'vue-router';
import CourseRelationButton from '../course-relation-button/CourseRelationButton.vue';
import EllipsisText from '@/components/base/text/EllipsisText.vue';

export default {
    name: 'CourseRegisteredCardTooltip',
    components: {
        CourseRelationButton,
        EllipsisText,
    },
    props: {
        course: {
            type: Object,
            required: true
        }
    },
    data(){
        return {
            courseThumbnail: null,
            defaultThumbnail: require('@/assets/default-thumbnail/course-image-icon.png'),
            dCourse: null,
            isReloadCourse: false,
            router: useRouter(),
        }
    },
    created(){
        this.refresh();
    },
    mounted(){
    },
    methods: {
        async refresh(){
            try {
                this.courseThumbnail = this.defaultThumbnail;
                this.dCourse = this.course;
                let courseThumbnail = this.dCourse?.Thumbnail;
                if (await Common.isValidImage(courseThumbnail)){
                    // console.log("thumbnail is valid");
                    this.courseThumbnail = courseThumbnail;
                }
                if (this.isReloadCourse){
                    let courseId = this.dCourse?.UserItemId;
                    if (courseId == null) return;
                    let res = await new GetRequest('Courses/' + courseId).execute();
                    let body = await Request.tryGetBody(res);
                    if (body != null){
                        let tempCourse = new ResponseCourseModel().copy(body);
                        this.dCourse = tempCourse;
                        this.isReloadCourse = false;
                        this.refresh();
                    }
                }
            } catch (e) {
                console.error(e);
            }
        },

        async resolveViewCourseDetail(){
            try {
                let courseId = this.dCourse?.UserItemId;
                if (courseId == null) return;
                this.router.push('/course/' + courseId);
            } catch (e) {
                console.error(e);
            }
        }
    },
    watch: {
        course: {
            handler: function() {
                this.refresh();
            },
            deep: true
        }
    }
}

</script>

<style scoped>

@import url(@/css/pages/desktop/components/course-registered-card-tooltip.css);

</style>

