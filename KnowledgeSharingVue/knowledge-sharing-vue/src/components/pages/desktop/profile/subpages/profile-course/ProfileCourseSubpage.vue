

<template>
    <div class="profile-course-subpage p-profile-main-subpage">
        <div class="pls-top">            
            <!-- Profile Course Toolbar Subpage -->
            <ProfileCourseToolbarSubpage 
                :list-courses="listCourse"
                :on-change-config="resolveOnchangeConfig" />
        </div>

        <div class="pls-bottom">
            <!-- Profile Course Content Subpage -->
            <ProfileCourseContentSubpage
                :list-courses="listFilteredCourse"
            />
        </div>
            
    </div>
</template>



<script>
import { GetRequest, Request } from '@/js/services/request';
import StringAlgorithm from '@/js/utils/string-algorithm';
import { MyDate } from '@/js/utils/mydate';
import { Validator } from '@/js/utils/validator';
import ProfileCourseToolbarSubpage from './ProfileCourseToolbarSubpage.vue';
import ProfileCourseContentSubpage from './ProfileCourseContentSubpage.vue';
import ResponseCourseModel from '@/js/models/api-response-models/response-course-model';

export default {
    name: 'ProfileCourseSubpage',
    components: {
        ProfileCourseToolbarSubpage,
        ProfileCourseContentSubpage
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
                    let mapCourseScore = {};
                    let listTitle = this.listCourse.map(function(vCourse){
                        return vCourse.Title;
                    });
                    let listFullName = this.listCourse.map(function(vCourse){
                        return vCourse.FullName;
                    });
                    let mapTitleScore = StringAlgorithm.similiarityList(text, listTitle);
                    let mapFullNameScore = StringAlgorithm.similiarityList(text, listFullName);
                    this.listCourse.forEach(function(vCourse){
                        let titleScore = mapTitleScore[vCourse.Title];
                        // let abstractScore = vCourse.Abstract != null ? StringAlgorithm.similiar(text, vCourse.Abstract) : 0;
                        let fullnameScore = mapFullNameScore[vCourse.FullName];
                        // let titleWeight = 0.75, usernameWeight = 0.25;
                        // let totalScore = titleScore * titleWeight + usernameScore * usernameWeight;
                        let totalScore = titleScore + fullnameScore;
                        mapCourseScore[vCourse.UserItemId] = totalScore;
                    });
                    let threshold = 0.0;

                    let sortedCourse = this.listCourse
                        .filter(function(vCourse){
                            return mapCourseScore[vCourse.UserItemId] >= threshold;
                        })
                        .sort(function(a, b){
                            return mapCourseScore[b.UserItemId] - mapCourseScore[a.UserItemId];
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
                let url = 'Courses/my';
                if (!this.isMySelf){
                    let userId = this.getUser()?.UserId;
                    if (userId == null) return;
                    url = `Users/courses/${userId}`;
                }
                this.listFilteredCourse = null;

                // Lay ve 20 ban ghi dau
                let res = await new GetRequest(url)
                    .setParams({ 
                        limit: 20,
                        offset: 0,
                    }).execute();
                let body = await Request.tryGetBody(res);
                let total = body.Total;
                let result = body.Results;
                this.listCourse = result.map(function(vCourse){
                    return new ResponseCourseModel().copy(vCourse);
                });
                this.listFilteredCourse = this.listCourse;

                // Lay ve toan bo ban ghi
                if (total > 20){
                    let res = await new GetRequest(url)
                        .setParams({ 
                            limit: total,
                            offset: 0,
                        }).execute();
                    body = await Request.tryGetBody(res);
                    result = body.Results;
                    this.listCourse = result.map(function(vCourse){
                        return new ResponseCourseModel().copy(vCourse);
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

        async resolveDeletedCourse(courseId){
            try {
                let index = this.listCourse.findIndex(function(vCourse){
                    return vCourse.UserItemId === courseId;
                });
                if (index === -1) return;
                this.listCourse.splice(index, 1);
                this.listFilteredCourse = this.listCourse;
            } catch (error){
                console.error(error);
            }
        }
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
    },
    provide(){
        return {
            onCourseDeleted: this.resolveDeletedCourse,
        }
    }
}

</script>


<style scoped>

.profile-course-subpage{
    display: flex;
    flex-flow: column nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

.profile-course-subpage > div{
    width: 100%;

}

</style>

