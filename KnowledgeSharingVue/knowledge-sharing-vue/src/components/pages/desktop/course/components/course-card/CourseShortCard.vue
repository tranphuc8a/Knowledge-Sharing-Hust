

<template>

    <div class="p-course-short-card card">
        <div class="p-csc-top">
            <div class="p-csc-thumbnail">
                <TooltipFrame ref="p-tooltip-course-thumbnail" :style="tooltipStyle">
                    <template #tooltipMask>
                        <div class="p-csc-thumbnail" :style="{  
                            backgroundImage: `url(${courseThumbnail})`
                        }">
                            <div class="p-csc-thumbnail-overlay">
                            </div>
                        </div>
                    </template>
            
                    <template #tooltipContent>
                        <CourseShortCardTooltip :course="course" />
                    </template>
                </TooltipFrame>
            </div>

            <div class="p-csc-thumbnail-button">
                <!-- View Detail Course Button -->
                <MCancelButton
                    label="Xem chi tiết"
                    :onclick="resolveViewCourseDetail"
                    :buttonStyle="buttonStyle"
                />
            </div>

            <div class="p-csc-menu-context">
                <slot name="courseMenuContext" />
            </div>
        </div>

        <div class="p-csc-bottom">
            <div class="p-csc-bottom-infor">
                <div class="p-csc-course-title" :title="course?.Title ?? ''">
                    <router-link :to="courseDetailLink">
                        <span>
                            {{ course?.Title ?? "" }}
                        </span>
                    </router-link>
                </div>
                <div class="p-csc-course-owner">
                    <TooltipUserAvatar :user="user" />
                    <div class="p-csc-course-owner__username">
                        <TooltipUsername :user="user" />
                    </div>
                </div>
            </div>
            <div class="p-csc-bottom-devider">
            </div>
            <div class="p-csc-bottom-relation">
                <div class="p-csc-course-totalstar">
                    <span>
                        <VisualizedTotalStar :average-star="course?.AverageStar" :total-star="course?.TotalStar" />
                    </span>
                </div>
                <div class="p-csc-course-cost">
                    <span>
                        <VisualizedCurrency :money="course?.Fee" />
                    </span>
                </div>
                <!-- <div class="p-csc-course-relation-button">
                    <CourseRelationButton />
                </div> -->
            </div>
        </div>

        
    </div>
</template>



<script>
import TooltipFrame from '@/components/base/tooltip/TooltipFrame.vue';
import CourseShortCardTooltip from './CourseShortCardTooltip.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';
import MCancelButton from '@/components/base/buttons/MCancelButton.vue';
// import CourseRelationButton from '../course-relation-button/CourseRelationButton.vue';
import Common from '@/js/utils/common';
import { useRouter } from 'vue-router';
// import ResponseCourseCardModel from '@/js/models/api-response-models/response-course-card-model';
// import { myEnum } from '@/js/resources/enum';
import VisualizedCurrency from '../course-cost/VisualizedCurrency.vue';
import ResponseCourseModel from '@/js/models/api-response-models/response-course-model';
import VisualizedTotalStar from '@/components/base/others/VisualizedTotalStar.vue';

export default {
    name: 'CourseShortCard',
    components: {
        TooltipFrame,
        CourseShortCardTooltip,
        TooltipUserAvatar, TooltipUsername,
        MCancelButton, 
        // CourseRelationButton,
        VisualizedCurrency,
        VisualizedTotalStar
    },
    props: {
        course: {
            type: Object,
            required: true
        }
    },
    watch: {
        course: {
            handler(){
                this.refresh();
            },
            deep: true
        }
    },
    data(){
        return {
            tooltipStyle: { boxShadow: '0px 0px 8px 4px rgba(var(--primary-color-rgb), .56)'},
            user: null,
            dCourse: this.course,
            router: useRouter(),
            buttonStyle: {},
            courseThumbnail: null,
            defaultCourseThumbnail: require('@/assets/default-thumbnail/course-image-icon.png'),
            courseDetailLink: '',
        }
    },
    async created(){
        this.refresh();
    },
    mounted(){

    },
    provide(){
        return {
            getCourse: () => this.dCourse,
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
                this.dCourse = new ResponseCourseModel().copy(this.course);
                this.user = this.dCourse?.getUser?.();
                this.courseDetailLink = '/course/' + this.dCourse?.UserItemId;
                
                // update thumbnail
                if (await Common.isValidImage(this.dCourse.Thumbnail)){
                    // console.log("image is valid");
                    this.courseThumbnail = this.dCourse.Thumbnail;
                }
            } catch (error){
                console.error(error);
            }
        }
    }
}

</script>

<style scoped>

@import url(@/css/pages/desktop/components/course-short-card.css);

</style>

