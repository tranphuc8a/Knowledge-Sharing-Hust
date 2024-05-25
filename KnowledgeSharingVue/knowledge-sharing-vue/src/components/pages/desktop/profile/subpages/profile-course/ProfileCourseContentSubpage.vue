

<template>
    <div class="p-profile-course-content-subpage" v-show="listCourses == null">
        <div class="p-pccs-list-course">
            <div class="p-pccs-list-course-content">
                <div class="p-pccs-card-item"
                    v-for="(index) in [1, 2, 3, 4]"
                    :key="index"
                    style="background-color: white; border-radius: 8px; overflow: hidden;"
                >   
                    <CourseShortCardSkeleton />
                </div>
            </div>
        </div>
    </div>

    <div class="p-profile-course-content-subpage" v-if="listCourses != null">
        <div class="p-pccs-pagination card">
            <a-pagination
                v-model:current="current"
                v-model:pageSize="pageSize"
                :total="total" />
        </div>
        
        <div class="p-pccs-list-course">
            <transition :name="transitionName">
                <div class="p-pccs-list-course-content">
                    <div class="p-pccs-card-item"
                        v-for="(course, index) in listFilteredCourses"
                        :key="(course.UserItemId ?? index)"
                    >
                        <CourseShortCard :course="course"/>
                    </div>
                </div>
            </transition>
        </div>

        <div class="p-pccs-pagination card">
            <a-pagination
                v-model:current="current"
                v-model:pageSize="pageSize"
                :total="total" />
        </div>
    </div>
</template>



<script>
import CourseShortCardSkeleton from '../../../course/components/course-card/CourseShortCardSkeleton.vue';
import CourseShortCard from '../../../course/components/course-card/CourseShortCard.vue';
import { MyRandom } from '@/js/utils/myrandom';


export default {
    name: 'ProfileCourseContentSubpage',
    components: {
        CourseShortCard,
        CourseShortCardSkeleton
    },
    props: {
        listCourses: {
            required: true,
        }
    },
    data(){
        return {
            dCurrent: 1,
            listFilteredCourses: [],
            transitionName: '',
            
            // a pagination:
            current: 1,
            total: 0,
            pageSize: 9,
        }
    },
    mounted(){
        this.refresh();
    },
    methods: {
        random(){
            return MyRandom.generateUUID();
        },

        async refresh(){
            try {
                this.pageSize = 9,
                this.current = 1;
                this.dCurrent = 1;
                this.total = this.listCourses?.length ?? 0;
                this.filterListCourse();
            } catch (e) {
                console.error(e);
            }    
        },

        async filterListCourse(){
            try {
                let offset = this.pageSize * (this.current - 1);
                let limit = this.pageSize;
                if (this.listCourses?.length > 0){
                    this.listFilteredCourses = this.listCourses.slice(offset, offset + limit);
                }
            } catch (e) {
                console.error(e);
            }
        }
    },
    watch: {
        listCourses: {
            handler: function () {
                this.refresh();
            },
            deep: true
        },
        current: {
            handler: function () {
                if (this.current > this.dCurrent) {
                    this.transitionName = 'slide-left';
                } else if (this.current < this.dCurrent) {
                    this.transitionName ='slide-right';
                }
                this.dCurrent = this.current;
                this.filterListCourse();
            }
        }
    }
}

</script>

<style scoped>

@import url(@/css/base/animation/animation.css);

.p-profile-course-content-subpage {
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-pccs-list-course{
    width: 100%;
}
.p-pccs-list-course-content{
    width: 100%;
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-pccs-card-item{
    width: calc((100% - 32px)/3);
    background-color: white;
    border-radius: 8px;
}

.p-pccs-pagination{
    min-width: 50%;
    max-width: 100%;
    width: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    padding: 16px 64px;
}

</style>

