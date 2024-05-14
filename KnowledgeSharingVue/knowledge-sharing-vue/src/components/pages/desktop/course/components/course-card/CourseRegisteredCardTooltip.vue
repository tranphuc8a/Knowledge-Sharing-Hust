

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
                    <div class="p-crc-title">
                        {{ dCourse?.Title ?? "" }}
                    </div>
                    <div class="p-crc-abstract">
                        {{ dCourse?.Abstract ?? "" }}
                    </div>
                </div>
                <div class="p-crc-button">
                    <!-- Course Relation Button -->
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import { faRefresh } from '@fortawesome/free-solid-svg-icons';
import Common from '@/js/utils/common';
import { GetRequest } from '@/js/services/request';
import ResponseCourseModel from '@/js/models/api-response-models/response-course-model';
import { useRouter } from 'vue-router';


export default {
    name: 'CourseRegisteredCardTooltip',
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
            isReloadCourse: true,
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

</style>

