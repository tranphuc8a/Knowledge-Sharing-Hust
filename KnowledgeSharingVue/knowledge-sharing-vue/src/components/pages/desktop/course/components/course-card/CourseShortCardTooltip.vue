

<template>
    <div class="p-csc-tooltip">
        <div class="p-csct-card card">
            <div class="p-csctc-top">
                <router-link :to="courseDetailLink" class="router-link">
                    <div class="p-csctc-thumbnail"
                        :click="resolveViewCourseDetail"
                        :style="{
                            'background-image': `url(${courseThumbnail})`
                        }"    
                    >
                        <div class="p-csc-thumbnail-overlay">
                        </div>
                    </div>
                </router-link>
            </div>
            <div class="p-csctc-bottom">
                <div class="p-csc-information">
                    <div class="p-csc-title" @:click="resolveViewCourseDetail">
                        {{ dCourse?.Title ?? "" }}
                    </div>
                    <!-- <div class="p-csc-abstract" v-if="dCourse?.Abstract != null">
                        {{ dCourse?.Abstract ?? "" }}
                    </div> -->
                    <div class="p-csc-privacy">
                        <VisualizedPrivacy :privacy="dCourse?.Privacy" :iconStyle="{fontSize: '16px'}"/>
                        {{ privacyText }}
                    </div>

                    <div class="p-csc-abstract" v-if="dCourse?.Abstract != null">
                        <EllipsisText 
                            :text="dCourse?.Abstract ?? ''" 
                            :max-line="3"
                            :style="{}"/>
                    </div>
                </div>

                <div class="p-devide" />

                <div class="p-csc-button">
                    <!-- Course Relation Button -->
                    <div class="p-csc-relation-button">
                        <CourseRelationButton />
                    </div>

                    <!-- Save Button -->
                    <div class="p-csc-save-button">
                        <SaveButton :knowledgeId="dCourse?.UserItemId" :initValue="dCourse?.IsMarked" />
                    </div>
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
import VisualizedPrivacy from '@/components/base/visualized-components/VisualizedPrivacy.vue';
import SaveButton from '@/components/base/others/SaveButton.vue';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'CourseShortCardTooltip',
    components: {
        CourseRelationButton,
        EllipsisText,  
        SaveButton,
        VisualizedPrivacy,
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
            dCourse: this.course,
            isReloadCourse: false,
            privacyText: null,
            courseDetailLink: '',
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
                this.courseDetailLink = '/course/' + this.dCourse?.UserItemId;
                if (await Common.isValidImage(courseThumbnail)){
                    // console.log("thumbnail is valid");
                    this.courseThumbnail = courseThumbnail;
                }
                this.privacyText = (this.dCourse?.Privacy == myEnum.EPrivacy.Public) ? 'Công khai' : 'Riêng tư';
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
    provide(){
        return {
            getCourse: () => this.dCourse,
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

@import url(@/css/pages/desktop/components/course-short-card-tooltip.css);

</style>

