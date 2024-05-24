

<template>
    <div class="p-course-revitation-content" v-if="true">
        <div class="p-empty-relation" v-if="isOutOfUser && !(listUser.length > 0)">
            <NotFoundPanel text="Không tìm thấy mục nào" />
        </div>
        <div class="p-prc-list-user">
            <div class="p-prc-user-item"
                v-for="(usercard, index) in listUser"
                :key="index + random()"
            >
                <CourseUserCard :user="usercard" />
            </div>
            
            <div class="p-prc-user-item"
                v-if="!isOutOfUser" >
                <CourseUserCardSkeleton />
            </div>

            <div class="p-prc-user-item"
                v-if="!isOutOfUser" >
                <CourseUserCardSkeleton />
            </div>

            <div class="p-prc-user-item"
                v-if="!isOutOfUser" >
                <CourseUserCardSkeleton />
            </div>

        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import CourseUserCardSkeleton from './CourseUserCardSkeleton.vue';
import CourseUserCard from './CourseUserCard.vue';
import ResponseUserCardModel from '@/js/models/api-response-models/response-user-card-model';
import ResponseCourseRelationModel from '@/js/models/api-response-models/response-course-relation-model';
import { MyRandom } from '@/js/utils/myrandom';
// import { myEnum } from '@/js/resources/enum';
import { Request } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';

export default {
    name: 'ProfileRelationContent',
    components: {
        CourseUserCard, CourseUserCardSkeleton, NotFoundPanel
    },
    props: {
        getMoreUser: {
            required: true,
        },
        courseRoleType: {}
    },
    watch: {
        getMoreUser: {
            handler: function() {
                this.refresh();
            },
            immediate: true,
        },
    },
    data(){
        return {
            listUser: [],
            isOutOfUser: false,
            isLoaded: false,
            isWorking: false,
        }
    },
    async mounted(){
        try {
            this.refresh();
            this.registerScrollHandler(this.resolveOnScroll.bind(this));
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        random(){
            return MyRandom.generateUUID();
        },

        async resolveOnScroll(scrollContainer){
            try {
                if (scrollContainer == null) return;

                let scrollHeight = scrollContainer.scrollHeight;
                let scrollTop = scrollContainer.scrollTop;
                let clientHeight = scrollContainer.clientHeight;
                let scrollPosition = clientHeight + scrollTop;
                // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);
                let averageItemHeight = 300;
                let leftItemNumber = 5;

                if (scrollHeight - scrollPosition < averageItemHeight * leftItemNumber){
                    // console.log("Load more post");
                    await this.loadMoreUser();
                }
            } catch (e){
                console.error(e);
            }
        },

        async refresh(){
            try {
                this.isLoaded = false;
                this.isOutOfUser = false;
                this.listUser = [];
                await this.loadMoreUser();
                this.isLoaded = true;
            } catch (e){
                console.error(e);
            }
        },

        async loadMoreUser(){
            if (this.isWorking || this.isOutOfUser) return;
            try {
                // update status
                this.isWorking = true;

                // prepare params and call api
                let limit = 15;
                let offset = this.listUser?.length;
                if (this.getMoreUser == null) return; 
                let response = await this.getMoreUser(limit, offset);
                let body = await Request.tryGetBody(response);

                // read response
                let listed = body;
                if (listed?.Results != null) listed = listed.Results;

                let tempResponseUserModel = listed.map(function(item){
                    // 2 kha nang: ReponseCourseRegisterMode and ResponseCourseRelationModel
                    if (item.User != null){
                        // response course relations model:
                        let resCoReMo = new ResponseCourseRelationModel().copy(item);
                        return resCoReMo.User;
                    }
                    // Response Course Register Model
                    let user = new ResponseUserCardModel().copy(item);
                    user.CourseRoleType = myEnum.ECourseRoleType.Member;
                    user.CourseRelationId = item.CourseRegisterId;
                });

                let courseRoleType = this.courseRoleType;
                tempResponseUserModel.forEach(function(item){
                    item.CourseRoleType = item.CourseRoleType ?? courseRoleType;
                });

                // update status and data
                if (tempResponseUserModel.length < limit) {
                    this.isOutOfUser = true;
                }
                if (tempResponseUserModel.length > 0){
                    this.listUser = this.listUser.concat(tempResponseUserModel);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        }
    },
    inject: {
        getIsMySelf: {},
        getUser: {},
        registerScrollHandler: {}
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-course-revitation-content{
    width: 100%;
    min-height: 150px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
}

.p-prc-list-user{
    width: 100%;
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
    padding: 0px;
    margin: 0px;
}

.p-prc-user-item{
    flex-shrink: 0;
    flex-grow: 0;
    width: calc((100% - 16px)/2);
}

.p-empty-relation{
    width: 100%;
    flex-grow: 1;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

</style>

