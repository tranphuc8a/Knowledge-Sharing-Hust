

<template>

    <div class="p-course-register-card card">
        <div class="p-crc-top">
            <div class="p-crc-thumbnail">
                <TooltipFrame ref="p-tooltip-course-thumbnail">
                    <template #tooltipMask>
                        <slot></slot>
                    </template>
            
                    <template #tooltipContent>
                        <CourseRegisteredCardTooltip :course="course" />
                    </template>
                </TooltipFrame>
            </div>

            <div class="p-crc-thumbnail-button">
                <!-- View Detail Course Button -->
                <MSecondaryButton
                    label="Xem chi tiết"
                    :onclick="resolveViewCourseDetail"
                    :buttonStyle="buttonStyle"
                />
            </div>
        </div>

        <div class="p-crc-bottom">
            <div class="p-crc-bottom-infor">
                <div class="p-crc-course-title">
                    {{ course?.Title ?? "" }}
                </div>
                <div class="p-crc-course-owner">
                    <TooltipUserAvatar :user="user" />
                    <TooltipUsername :user="user" />
                </div>
            </div>
            <div class="p-crc-bottom-relation">
                <div class="p-crc-course-cost">
                    
                </div>
                <div class="p-crc-course-relation-button">
                    <!-- Course Relation Button -->
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
import SecondaryButton from '@/components/base/buttons/MSecondaryButton.vue';

import { useRouter } from 'vue-router';


export default {
    name: 'CourseRegisteredCard',
    components: {
        TooltipFrame,
        CourseRegisteredCardTooltip,
        TooltipUserAvatar, TooltipUsername,
        MSecondaryButton
    },
    props: {
        courseRegister: {
            type: Object,
            required: true
        }
    },
    watch: {
        courseRegister: {
            handler(val){
                this.title = val.course.title;
                this.isLessonExisted =!!val.lesson;
                this.lesson = val.lesson;
            },
            deep: true
        }
    },
    data(){
        return {
            user: null,
            course: null,
            router: useRouter(),
            buttonStyle: {}
        }
    },
    mounted(){

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
        }
    }
}

</script>

<style scoped>

.p-course-register-card{
    width: 100%;
    max-width: 100%;
}

</style>

