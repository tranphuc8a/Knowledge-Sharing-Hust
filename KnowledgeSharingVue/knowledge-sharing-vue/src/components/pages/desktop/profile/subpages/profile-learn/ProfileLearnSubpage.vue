

<template>
    <div class="profile-learn-subpage p-profile-main-subpage">
        <div class="pls-top">            
            <!-- Profile Learn Toolbar Subpage -->
            <ProfileLearnToolbarSubpage 
                :list-courses="listCourse"
                :on-change-config="resolveOnchangeConfig" />
        </div>

        <div class="pls-bottom">
            <!-- Profile Learn Content Subpage -->
            <ProfileLearnContentSubpage
                :list-courses="listFilteredCourse"
            />
        </div>
            
    </div>
</template>



<script>
import { GetRequest, Request } from '@/js/services/request';
import ViewCourseRegister from '@/js/models/views/view-course-register';
import StringAlgorithm from '@/js/utils/string-algorithm';
import { MyDate } from '@/js/utils/mydate';
import { Validator } from '@/js/utils/validator';
import ProfileLearnToolbarSubpage from './ProfileLearnToolbarSubpage.vue';
import ProfileLearnContentSubpage from './ProfileLearnContentSubpage.vue';

export default {
    name: 'ProfileLearnSubpage',
    components: {
        ProfileLearnToolbarSubpage,
        ProfileLearnContentSubpage
    },
    props: {
    },
    data(){
        return {
            isMySelf: false,
            listFilteredCourse: null,
            listCourse: [],
        }
    },
    async created(){
        try {
            this.refresh();
        } catch (error){
            console.error(error);
        }
    },
    async mounted(){
    },
    methods: {
        async resolveOnchangeConfig(config){
            try {
                let { text, isLatest } = config;
                this.listFilteredCourse = null;
                if (Validator.isEmpty(text) && (isLatest !== true && isLatest !== false)){
                    this.listFilteredCourse = this.listCourse;
                    return;
                }
                if (Validator.isNotEmpty(text)){
                    let mapTextScore = {};
                    this.listCourse.forEach(function(vCourse){
                        let titleScore = StringAlgorithm.similiar(text, vCourse.Title);
                        // let abstractScore = vCourse.Abstract != null ? StringAlgorithm.similiar(text, vCourse.Abstract) : 0;
                        let usernameScore = StringAlgorithm.similiar(text, vCourse.CourseOwnerFullName);
                        // let titleWeight = 0.75, usernameWeight = 0.25;
                        // let totalScore = titleScore * titleWeight + usernameScore * usernameWeight;
                        let totalScore = Math.max(titleScore, usernameScore);
                        mapTextScore[vCourse.CourseId] = totalScore;
                    });
                    let threshold = 0.0;

                    let sortedCourse = this.listCourse
                        .filter(function(vCourse){
                            return mapTextScore[vCourse.CourseId] >= threshold;
                        })
                        .sort(function(a, b){
                            return mapTextScore[b.CourseId] - mapTextScore[a.CourseId];
                        });
                    this.listFilteredCourse = sortedCourse;
                    return;
                } 
                if (isLatest === true || isLatest === false) {
                    let sortedCourse = this.listCourse.sort(function(a, b){
                        let bCreatedDate = new MyDate(b.CreatedTime);
                        let aCreatedDate = new MyDate(a.CreatedTime);
                        if (isLatest) {
                            return bCreatedDate.getTime() - aCreatedDate.getTime();
                        }
                        return aCreatedDate.getTime() - bCreatedDate.getTime();
                    });
                    this.listFilteredCourse = sortedCourse;
                    return;
                }
                this.listFilteredCourse = this.listCourse;
            } catch (error){
                console.error(error);
            }
        },

        async refresh(){
            try {
                this.isMySelf = await this.getIsMySelf();
                this.listFilteredCourse = null;

                // Lay ve 20 ban ghi dau
                let res = await new GetRequest('Courses/my/registered-courses')
                    .setParams({ 
                        limit: 20,
                        offset: 0,
                    }).execute();
                let body = await Request.tryGetBody(res);
                let total = body.Total;
                let result = body.Results;
                this.listCourse = result.map(function(vCourse){
                    return new ViewCourseRegister().copy(vCourse);
                });
                this.listFilteredCourse = this.listCourse;

                // Lay ve toan bo ban ghi
                if (total > 20){
                    let res = await new GetRequest('Courses/my/registered-courses')
                        .setParams({ 
                            limit: total,
                            offset: 0,
                        }).execute();
                    body = await Request.tryGetBody(res);
                    result = body.Results;
                    this.listCourse = result.map(function(vCourse){
                        return new ViewCourseRegister().copy(vCourse);
                    });
                    this.listFilteredCourse = null;
                    this.$nextTick(() => {
                        this.listFilteredCourse = this.listCourse;
                    });
                }

            } catch (error){
                console.error(error);
            }
        },
    },
    inject: {
        getIsMySelf: {}
    }
}

</script>


<style scoped>

.profile-learn-subpage{
    display: flex;
    flex-flow: column nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

.profile-learn-subpage > div{
    width: 100%;

}

</style>

