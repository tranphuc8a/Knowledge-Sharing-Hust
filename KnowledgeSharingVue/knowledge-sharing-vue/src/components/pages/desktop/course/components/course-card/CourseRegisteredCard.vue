

<template>

    <div class="p-course-register-card card">
        <div class="p-crc-top">
            <div class="p-crc-thumbnail">
                <TooltipFrame ref="p-tooltip-course-thumbnail">
                    <template #tooltipMask>
                        <div class="p-crc-thumbnail" :style="{  
                            backgroundImage: `url(${courseThumbnail})`
                        }">
                            <div class="p-crc-thumbnail-overlay">
                            </div>
                        </div>
                    </template>
            
                    <template #tooltipContent>
                        <CourseRegisteredCardTooltip :course="course" />
                    </template>
                </TooltipFrame>
            </div>

            <div class="p-crc-thumbnail-button">
                <!-- View Detail Course Button -->
                <MCancelButton
                    label="Xem chi tiết"
                    :onclick="resolveViewCourseDetail"
                    :buttonStyle="buttonStyle"
                />
            </div>
        </div>

        <div class="p-crc-bottom">
            <div class="p-crc-bottom-infor">
                <div class="p-crc-course-title" :title="course?.Title ?? ''">
                    {{ course?.Title ?? "" }}
                </div>
                <div class="p-crc-course-owner">
                    <TooltipUserAvatar :user="user" />
                    <div class="p-crc-course-owner__username">
                        <TooltipUsername :user="user" />
                    </div>
                </div>
            </div>
            <div class="p-crc-bottom-devider">
            </div>
            <div class="p-crc-bottom-relation">
                <div class="p-crc-course-cost">
                    <VisualizedCurrency :money="course.Fee" />
                </div>
                <div class="p-crc-course-relation-button">
                    <!-- Course Relation Button -->
                    <CourseRelationButton />
                </div>
            </div>
        </div>

        
    </div>
</template>



<script>
import TooltipFrame from '@/components/base/tooltip/TooltipFrame.vue';
import CourseRegisteredCardTooltip from './CourseRegisteredCardTooltip.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';
import MCancelButton from '@/components/base/buttons/MCancelButton.vue';
import CourseRelationButton from '../course-relation-button/CourseRelationButton.vue';
import Common from '@/js/utils/common';
import { useRouter } from 'vue-router';
import ResponseCourseCardModel from '@/js/models/api-response-models/response-course-card-model';
import { myEnum } from '@/js/resources/enum';
import VisualizedCurrency from '../course-cost/VisualizedCurrency.vue';

export default {
    name: 'CourseRegisteredCard',
    components: {
        TooltipFrame,
        CourseRegisteredCardTooltip,
        TooltipUserAvatar, TooltipUsername,
        MCancelButton, CourseRelationButton,
        VisualizedCurrency
    },
    props: {
        courseRegister: {
            type: Object,
            required: true
        }
    },
    watch: {
        courseRegister: {
            handler(){
                this.refresh();
            },
            deep: true
        }
    },
    data(){
        return {
            user: null,
            course: null,
            router: useRouter(),
            buttonStyle: {},
            courseThumbnail: null,
            defaultCourseThumbnail: require('@/assets/default-thumbnail/course-image-icon.png')
        }
    },
    async created(){
        this.refresh();
    },
    mounted(){

    },
    provide(){
        return {
            getCourse: () => this.course,
        }
    },
    methods: {

        async resolveViewCourseDetail(){
            try {
                let courseId = this.course?.UserItemId;
                if (courseId == null) return;
                this.router.push('/course/' + courseId);
            } catch (error){
                console.error(error);
            }
        },

        async refresh(){
            try {
                this.courseThumbnail = this.defaultCourseThumbnail;
                // update course card
                let course = this.courseRegister?.getCourse?.();
                let courseCard = new ResponseCourseCardModel().copy(course);
                courseCard.CourseRoleType = myEnum.ECourseRoleType.Member;
                courseCard.CourseRelationId = this.courseRegister?.CourseRegisterId;
                this.course = courseCard;
                this.user = this.courseRegister?.getOwner?.();
                // update thumbnail
                if (await Common.isValidImage(courseCard?.Thumbnail)){
                    console.log("image is valid");
                    // this.courseThumbnail = courseCard.Thumbnail;
                }
            } catch (error){
                console.error(error);
            }
        }
    }
}

</script>

<style scoped>

@import url(@/css/pages/desktop/components/course-registered-card.css);

</style>

