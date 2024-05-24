

<template>
    <div class="p-course-member-subpage p-course-subpage" v-if="!isLoaded">
        <div class="p-course-member-card card">
            <div class="p-course-member-row">
                <div class="p-cmr-heading card-subheading">
                    Quản trị viên khóa học
                </div>
                <div class="p-cmr-content">
                    <CourseUserCardSkeleton />
                </div>
            </div>

            <div class="p-devide"></div>
            
            <div class="p-course-member-row">
                <div class="p-cmr-heading card-subheading">
                    <div class="skeleton" style="width: 150px; height: 28px;">
                    </div>
                </div>
                <div class="p-cmr-content">
                    <div class="p-cmr-textfield-frame">
                        <!-- Textfield Button -->
                        <MTextfieldButton 
                            placeholder="Tìm kiếm thành viên"
                            :is-show-icon="true" 
                            :is-show-title="false" 
                            :is-show-error="false" 
                            :is-obligate="false"
                            :onclick-icon="()=>{}" 
                            :validator="null"
                            state="normal"
                            ref="textfield"
                        />
                    </div>
                </div>
            </div>

            <div class="p-devide"></div>

            <div class="p-course-member-row">
                <div class="p-cmr-content">
                    <CourseUserCardSkeleton />
                    <CourseUserCardSkeleton />
                    <CourseUserCardSkeleton />
                </div>
            </div>
        </div>
    </div>

    <div class="p-course-member-subpage p-course-subpage" v-if="isLoaded">
        <div class="p-course-member-card card">
            <div class="p-course-member-row">
                <div class="p-cmr-heading card-subheading">
                    Quản trị viên khóa học
                </div>
                <div class="p-cmr-content">
                    <ProfileFriendCard :user="dCourse?.getUser?.()" />
                </div>
            </div>

            <div class="p-devide"></div>
            
            <div class="p-course-member-row">
                <div class="p-cmr-heading card-subheading">
                    <span>Thành viên khóa học</span>
                    <MIcon fa="circle" :style="dotIconStyle"/>
                    {{ numberMember }}
                </div>
                <div class="p-cmr-content">
                    <div class="p-cmr-textfield-frame">
                        <!-- Textfield Button -->
                        <MTextfieldButton 
                            placeholder="Tìm kiếm thành viên"
                            :is-show-icon="true" 
                            :is-show-title="false" 
                            :is-show-error="false" 
                            :is-obligate="false"
                            :onclick-icon="resolveOnClickSearch" 
                            :validator="null"
                            state="normal"
                            ref="textfield"
                        />
                    </div>
                </div>
            </div>

            <div class="p-devide"></div>

            <div class="p-course-member-row">
                <div class="p-cmr-content">
                    <CourseUserCard v-for="(user, index) in listMember" 
                        :key="user?.UserId ?? index" :user="user" />

                    <CourseUserCardSkeleton v-show="!isOutOfMember" />
                    <CourseUserCardSkeleton v-show="!isOutOfMember" />
                    <CourseUserCardSkeleton v-show="!isOutOfMember" />

                    <div class="p-empty-relation" v-show="isOutOfMember && !(listMember?.length > 0)">
                        <NotFoundPanel text="Không tìm thấy mục nào" />
                    </div>

                </div>
            </div>
        </div>
    </div>
</template>



<script>
// import { Validator } from '@/js/utils/validator';
import MTextfieldButton from './../../../../../base/inputs/MTextfieldButton.vue';
import ProfileFriendCard from '../../../profile/components/profile-friend-sp/ProfileFriendCard.vue';
import CourseUserCard from '../course-revitation/CourseUserCard.vue';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import CourseUserCardSkeleton from '../course-revitation/CourseUserCardSkeleton.vue';
import Common from '@/js/utils/common';
import { Validator } from '@/js/utils/validator';
import { GetRequest, Request } from '@/js/services/request';
import ResponseCourseRegisterModel from '@/js/models/api-response-models/response-course-register-model';
import ResponseUserCardModel from '@/js/models/api-response-models/response-user-card-model';
import { myEnum } from '@/js/resources/enum';
import scrollHandler from '@/js/components/base/scroll-handler';


export default {
    name: 'CourseRevitationSubpage',
    components: {
        MTextfieldButton,
        ProfileFriendCard,
        CourseUserCard,
        CourseUserCardSkeleton,
        NotFoundPanel,
    },
    props: {
    },
    data(){
        return {
            isLoaded: false,
            dCourse: null,
            numberMember: null,
            listMember: [],
            dotIconStyle: {fontSize: '4.5px', color: 'var(--grey-color-600)'},
            isWorking: false,
            isOutOfMember: false,
            memberCardHeight: 300,
            numberMemberLeft: 5,
        }
    },
    async created(){
        try {
            await this.refresh();
            let that = this;
            if (this.registerScrollHandler != null){
                let handler = async function(container){
                    await scrollHandler.resolve(container, that.loadMoreMember.bind(that), that.memberCardHeight, that.numberMemberLeft);
                }
                this.registerScrollHandler(handler);
            }
        } catch (e){
            console.error(e);
        }
    },
    async mounted(){
    },
    methods: {
        async refresh(){
            try {
                this.dCourse = this.getCourse();
                this.numberMember = Common.formatNumber(this.dCourse?.TotalRegister ?? 0);
                this.isLoaded = this.dCourse != null;
                if (this.dCourse != null){
                    await this.loadMoreMember();
                }
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnClickSearch(){
            try {
                this.listMember = [];
                this.isOutOfMember = false;
                await this.loadMoreMember();
            } catch (e){
                console.error(e);
            }
        },

        async loadMoreMember(){
            if (this.isWorking || this.isOutOfMember) return;
            try {
                this.isWorking = true;

                // Get search text
                let textfield = this.$refs.textfield;
                let text = await textfield?.getValue?.() ?? "";
                let courseId = this.dCourse?.UserItemId;
                if (courseId == null) return;

                // prepare url
                let url = 'CourseRelations/registers/' + courseId;
                if (Validator.isNotEmpty(text)){
                    // search
                    url = 'CourseRelations/search/registers/' + courseId;
                }

                // prepare pagination:
                let offset = this.listMember.length;
                let limit = 15;

                // request api:
                let res = await new GetRequest(url)
                    .setParams({ offset, limit, search: text })
                    .execute();
                let body = await Request.tryGetBody(res);
                if (body.Results != null) body = body.Results;

                // get list of members
                let tempMembers = body.map(function(item){
                    let register = new ResponseCourseRegisterModel().copy(item);
                    let userCard = new ResponseUserCardModel().copy(register);
                    userCard.CourseRoleType = myEnum.ECourseRoleType.Member;
                    userCard.CourseRelationId = register.CourseRegisterId;
                    return userCard;
                });
                if (tempMembers.length < limit){
                    this.isOutOfMember = true;
                }
                if (tempMembers.length > 0){
                    this.listMember = this.listMember.concat(tempMembers);
                }
            } catch (e){
                Request.resolveAxiosError(e);
            } finally {
                this.isWorking = false;
            }
        },
    },
    inject: {
        registerScrollHandler: {},
        getCourse: {}
    },
    provide(){
        return {
            registerOnClickSearch: this.registerOnClickSearch,
        }
    }
}

</script>


<style scoped>

.p-course-member-subpage{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    padding-bottom: 32px;
}
.p-course-member-subpage .p-course-member-card{
    width: 55%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 32px;
    padding-bottom: 32px;
}

.p-course-member-subpage .p-course-member-card > * {
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}

.p-cmr-heading{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 8px;
}

.p-cmr-content{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}


</style>

