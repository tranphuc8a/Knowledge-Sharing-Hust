

<template>
    <!-- Not right -->
    <div class="p-course-update-subpage p-course-subpage" v-if="!isMyCourse">
        <div class="p-cus-card card">
            <!-- Error Panel -->
            <div class="p-cus-not-my-course">
                <NotFoundPanel text="Bạn không có quyền thực hiện hành động này" />
            </div>
        </div>
    </div>

    <!-- OK -->
    <div class="p-course-update-subpage p-course-subpage" v-if="isMyCourse">
        <div class="p-cus-card card">

            <!-- Card header -->
            <div class="p-cus-card-header card-header">
                <div class="p-cus-heading card-heading">
                    <span> Cập nhật danh sách bài giảng cho khóa học </span>
                </div>
                <div class="p-cus-header-button">
                    <span title="Khôi phục thay đổi">
                        <MActionIcon 
                            fa="rotate-left" family="fas"
                            :iconStyle="{fontSize: '20px'}" :containerStyle="{width: '36px', height: '36px'}"
                            :onclick="resolveClickReset"
                            />
                    </span>
                    <span>
                        <MButton 
                            fa="check" label="Lưu danh sách"
                            :onclick="resolveClickSaveLesson" />
                    </span>
                    <span>
                        <MSecondaryButton 
                            fa="plus" label="Thêm bài giảng vào khóa học"
                            :onclick="resolveClickAddLesson" />
                    </span>
                </div>
            </div>

            <!-- Card toolbar -->
            <div class="p-cus-card-toolbar" v-show="false">
            </div>

            <div class="p-devide"></div>

            <!-- <div>
                <div class="g-recaptcha" :data-sitekey="siteKey"></div>
            </div>
            <MButton label="Show Captcha" :onclick="resolveOnClickCaptcha" /> -->

            <!-- Card content -->
            <div class="p-cus-content">
                <div class="p-cus-not-found" v-show="!(listCourseLesson.length > 0)">
                    <NotFoundPanel text="Không tìm thấy bài giảng nào" />
                </div>

                <transition-group name="list">
                    <div class="p-cus-course-lesson-item"
                        v-for="(courseLesson, index) in listCourseLesson"
                        :key="courseLesson.CourseLessonId"
                    >
                        <div class="p-course-lesson-moving-button">
                            <div class="p-clmb-up" title="Chuyển lên trên">
                                <MActionIcon 
                                    :key="index"
                                    fa="caret-up" family="fas"
                                    :iconStyle="movingIconStyle" :containerStyle="movingContainerStyle"
                                    :onclick="resolveClickMoveUp(index)"
                                    :state="index > 0 ? iconStateEnum.NORMAL : iconStateEnum.DISABLED"
                                    :style="{
                                        visibility: index > 0 ? 'visible' : 'hidden'
                                    }"
                                    />
                            </div>
                            <div class="p-clmb-down" title="Chuyển xuống dưới">
                                <MActionIcon 
                                    :key="index"
                                    fa="caret-down" family="fas"
                                    :iconStyle="movingIconStyle" :containerStyle="movingContainerStyle"
                                    :onclick="resolveClickMoveDown(index)"
                                    :state="index < listCourseLesson.length - 1 ? iconStateEnum.NORMAL : iconStateEnum.DISABLED"
                                    :style="{
                                        visibility: index < listCourseLesson.length - 1 ? 'visible' : 'hidden'
                                    }"
                                    />
                            </div>
                        </div>
    
                        <div class="p-course-lesson-card">
                            <CourseLessonCard 
                                :responseCourseLessonModel="courseLesson" 
                                :isShowMenuContext="false" />
                        </div>
    
                        <div class="p-course-lesson-delete-button">
                            <MActionIcon title="Xóa bài giảng khỏi khóa học"
                                fa="trash-can" family="far"
                                :iconStyle="deleteIconStyle" :containerStyle="deleteContainerStyle"
                                :onclick="resolveClickDeleteLesson(index)"
                                :state="iconStateEnum.NORMAL"
                                />
                        </div>
                    </div>
                </transition-group>
            </div>

            <div class="p-devide"></div>

            <!-- Card footer -->
            <div class="p-cus-card-footer card-footer">
                <div class="p-cus-heading card-heading">
                    <!-- Footer has not heading -->
                </div>
                <div class="p-cus-footer-button">
                    <span title="Khôi phục thay đổi">
                        <MActionIcon 
                            fa="rotate-left" family="fas"
                            :iconStyle="{fontSize: '20px'}" :containerStyle="{width: '36px', height: '36px'}"
                            :onclick="resolveClickReset"
                            />
                    </span>
                    <span>
                        <MButton 
                            fa="check" label="Lưu danh sách"
                            :onclick="resolveClickSaveLesson" />
                    </span>
                    <span>
                        <MSecondaryButton 
                            fa="plus" label="Thêm bài giảng vào khóa học"
                            :onclick="resolveClickAddLesson" />
                    </span>
                </div>
            </div>
        </div>

        <SelectLessonPopup ref="popup" :isShow="false" :onOkay="resolveAddLesson"/>
    </div>
</template>



<script>
/* global grecaptcha */
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue';
import MButton from './../../../../../base/buttons/MButton.vue'
import { myEnum } from '@/js/resources/enum';
import SelectLessonPopup from '@/components/base/popup/select-lesson-popup/SelectLessonPopup.vue';
import { DeleteRequest, GetRequest, PatchRequest, PostRequest, Request } from '@/js/services/request';
import ResponseCourseLessonModel from '@/js/models/api-response-models/response-course-lesson-model';
import CourseLessonCard from '@/components/base/cards/CourseLessonCard.vue';
// import CourseLessonCardSkeleton from '@/components/base/cards/CourseLessonCardSkeleton.vue';
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import appConfig from '@/app-config';
// import Common from '@/js/utils/common';


export default {
    name: 'CourseUpdateSubpage',
    components: {
        MSecondaryButton,
        MButton,
        SelectLessonPopup,
        CourseLessonCard,
        // CourseLessonCardSkeleton,
        NotFoundPanel,
    },
    props: {
    },
    data(){
        return {
            siteKey: appConfig.getCaptchaSiteKey(),
            iconStateEnum: myEnum.iconState,
            movingIconStyle: {
                fontSize: '28px'
            },
            movingContainerStyle: {
                width: '32px',
                height: '32px'
            },
            deleteIconStyle: {
                color: 'red',
            },
            deleteContainerStyle: {
                width: '36px',
                height: '36px'
            },
            isWorking: false,
            listCourseLesson: [],
            listCourseOrigin: [],
            isMyCourse: false,
        }
    },
    async created(){
        try {
            this.isMyCourse = await this.getIsMyCourse();
            if (!this.isMyCourse) return;
            await this.loadAllCourseLesson();
        } catch (e){
            console.error(e);
        }
    },
    async mounted(){
        // Common.loadRecaptchaScript();
    },
    methods: {
        async loadAllCourseLesson(){
            if (this.isWorking) {
                this.getToastManager().inform("Vui lòng chờ trong giây lát");
                return;
            }
            try {
                this.isWorking = true;
                let courseId = this.getCourse()?.UserItemId;
                if (courseId == null) return;
                let res = await new GetRequest('CourseLessons/' + courseId).execute();
                let body = await Request.tryGetBody(res);

                // read data
                let totalLesson = body.Total;
                let listLessons = body.Results;

                // load more if not load enough lesson
                if (listLessons.length < totalLesson){
                    let offset = listLessons.length;
                    let limit = totalLesson - listLessons.length;
                    res = await new GetRequest('CourseLessons/' + courseId)
                        .setParams({ limit, offset })
                        .execute();
                    body = await Request.tryGetBody(res);
                    listLessons = listLessons.concat(body.Results);
                }
                
                // order list lesson by offset
                listLessons.sort((a, b) => Number(a.Offset) - Number(b.Offset));
                this.listCourseOrigin = listLessons.map(function(item){
                    return new ResponseCourseLessonModel().copy(item);
                });
                this.listCourseLesson = this.listCourseOrigin.map(function(item){
                    return new ResponseCourseLessonModel().copy(item);
                });
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.isWorking = false;
            }
        },

        async resolveClickSaveLesson(){
            if (this.isWorking) {
                this.getToastManager().inform("Vui lòng chờ trong giây lát");
                return;
            }
            try {
                this.isWorking = true;
                let listIds = this.listCourseLesson.map(function(item){
                    return item.CourseLessonId;
                });
                await new PatchRequest('CourseLessons/update-offsets')
                    .setBody(listIds).execute();
                // success:
                this.getToastManager().success('Lưu danh sách bài giảng thành công');
                this.listCourseOrigin = this.listCourseLesson.map(function(item){
                    return new ResponseCourseLessonModel().copy(item);
                });
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.isWorking = false;
            }
        },

        async resolveClickReset(){
            if (this.isWorking) {
                this.getToastManager().inform("Vui lòng chờ trong giây lát");
                return;
            }
            try {
                this.isWorking = true;
                // check for any change between origin and current:
                let isChange = false;
                if (this.listCourseOrigin.length != this.listCourseLesson.length){
                    isChange = true;
                } else {
                    for (let i = 0; i < this.listCourseOrigin.length; i++){
                        if (this.listCourseOrigin[i].CourseLessonId != this.listCourseLesson[i].CourseLessonId){
                            isChange = true;
                            break;
                        }
                    }
                }
                // reset data if changed:
                if (isChange){
                    this.listCourseLesson = this.listCourseOrigin.map(function(item){
                        return new ResponseCourseLessonModel().copy(item);
                    });
                    this.getToastManager().success('Đã khôi phục danh sách bài giảng');
                } else {
                    this.getToastManager().inform('Không có thay đổi nào');
                }
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.isWorking = false;
            }
        },

        async resolveClickAddLesson(){
            try {
                this.$refs.popup?.show?.();
            } catch (error) {
                console.error(error);
            }
        },

        resolveClickMoveUp(index){
            return async function(){
                if (this.isWorking){
                    this.getToastManager().inform("Vui lòng chờ trong giây lát");
                    return;
                }
                try {
                    this.isWorking = true;
                    if (index <= 0) return;
                    let temp = this.listCourseLesson[index];
                    this.listCourseLesson[index] = this.listCourseLesson[index - 1];
                    this.listCourseLesson[index - 1] = temp;
                    // update offset:
                    for (let i = index - 1; i <= index; i++){
                        this.listCourseLesson[i].Offset = i + 1;
                    }
                } catch (error) {
                    console.error(error);
                } finally {
                    this.isWorking = false;
                }
            }.bind(this);
        },
        

        resolveClickMoveDown(index){
            return async function(){
                if (this.isWorking){
                    this.getToastManager().inform("Vui lòng chờ trong giây lát");
                    return;
                }
                try {
                    this.isWorking = true;
                    if (index >= this.listCourseLesson.length - 1) return;
                    let temp = this.listCourseLesson[index];
                    this.listCourseLesson[index] = this.listCourseLesson[index + 1];
                    this.listCourseLesson[index + 1] = temp;
                    // update offset:
                    for (let i = index; i <= index + 1; i++){
                        this.listCourseLesson[i].Offset = i + 1;
                    }
                } catch (error) {
                    console.error(error);
                } finally {
                    this.isWorking = false;
                }
            }.bind(this);
        },

        async resolveOnClickCaptcha(){
            try {
                console.log(grecaptcha.getResponse());
            } catch (error) {
                console.error(error);
            }
        },

        resolveClickDeleteLesson(index){
            return async function(){
                try {
                    if (index < 0 || index >= this.listCourseLesson.length) return;
                    let lesson = this.listCourseLesson[index];
                    let participantId = lesson.CourseLessonId;
                    let alertMsg = `Bạn có chắc chắn muốn xóa bài giảng số ${lesson.Offset} khỏi khóa học?`;
                    let deleteCallback = this.submitDeleteCourseLesson.bind(this, participantId);
                    this.getPopupManager().warning(alertMsg, deleteCallback);
                } catch (e){
                    console.error(e);
                }
            }.bind(this);
        },

        async submitDeleteCourseLesson(participantId){
            if (this.isWorking){
                this.getToastManager().inform("Vui lòng chờ trong giây lát");
                return;
            }
            try {
                this.isWorking = true;                
                await new DeleteRequest('CourseLessons/lesson/' + participantId).execute();

                // success:
                // update course lesson and offset
                let index = this.listCourseLesson.findIndex(function(item){
                    return item.CourseLessonId == participantId;
                });
                this.listCourseLesson.splice(index, 1);
                for (let i = index; i < this.listCourseLesson.length; i++){
                    this.listCourseLesson[i].Offset = i + 1;
                }

                // update originlist and offset in originlist:
                let originIndex = this.listCourseOrigin.findIndex(function(item){
                    return item.CourseLessonId == participantId;
                });
                if (originIndex >= 0){
                    this.listCourseOrigin.splice(originIndex, 1);
                    for (let i = originIndex; i < this.listCourseOrigin.length; i++){
                        this.listCourseOrigin[i].Offset = i + 1;
                    }
                }

                // toast success:
                this.getToastManager().success('Đã xóa bài giảng khỏi khóa học');
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.isWorking = false;
            }
        },

        async resolveAddLesson(lesson){
            if (this.isWorking){
                this.getToastManager().inform("Vui lòng chờ trong giây lát");
                return;
            }
            try {
                this.isWorking = true;

                // prepare call api
                let lessonId = lesson?.UserItemId;
                let courseId = this.getCourse()?.UserItemId;
                let offset = this.listCourseLesson.length;
                let lessonTitle = lesson?.Title ?? ("Bài giảng số " + offset);
                if (lessonId == null || courseId == null) return;
                
                // call api:
                let res = await new PostRequest('CourseLessons/lesson')
                    .setBody({ 
                        CourseId: courseId, 
                        LessonId: lessonId, 
                        LessonTitle: lessonTitle 
                    })
                    .execute();
                let body = await Request.tryGetBody(res);

                // success:
                // update list course lesson and offset:
                let newLesson = new ResponseCourseLessonModel().copy(body);
                this.listCourseLesson.push(newLesson);
                for (let i = 0; i < this.listCourseLesson.length; i++){
                    this.listCourseLesson[i].Offset = i + 1;
                }
                // update list origin lesson and offset:
                this.listCourseOrigin.push(newLesson);
                for (let i = 0; i < this.listCourseOrigin.length; i++){
                    this.listCourseOrigin[i].Offset = i + 1;
                }

                // toast:
                this.getToastManager().success('Thêm bài giảng thành công');
            } catch (error) {
                Request.resolveAxiosError(error);
            } finally {
                this.$refs.popup?.hide?.();
                this.isWorking = false;
            }
        },
    },
    inject: {
        getCourse: {},
        getIsMyCourse: {},
        getToastManager: {},
        getPopupManager: {},
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-course-update-subpage{
    width: 100%;
}

.p-course-update-subpage{
    min-width: 50%;
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    padding-bottom: 32px;
}

.p-course-update-subpage .p-cus-card{
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    padding: 16px;
    gap: 16px;
}

.p-cus-not-my-course{
    width: 100%;
    height: 250px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.p-course-update-subpage .p-cus-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-course-update-subpage .p-cus-card-toolbar{
    width: 100%;
    height: 50px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}


.p-course-update-subpage .p-cus-card-header .p-cus-header-button{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: center;
    gap: 8px;
}

.p-course-update-subpage .p-cus-content{
    width: 85%;
    max-width: 100%;
    height: 100%;
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    padding: 32px;
    align-items: stretch;
    align-self: center;
    gap: 8px;
}

.p-course-update-subpage .p-cus-course-lesson-item{
    width: 100%;
    height: auto;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
    padding: 16px 16px 32px 16px;
    border-radius: 8px;
    gap: 16px;
}

.p-course-update-subpage .p-cus-course-lesson-item:hover{
    background-color: rgba(var(--primary-color-50-rgb), 0.56);
}

.p-course-update-subpage .p-cus-course-lesson-item > div {
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}


.p-course-update-subpage .p-course-lesson-moving-button{
    width: 50px;
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
}

.p-course-update-subpage .p-course-lesson-card{
    width: 100%;
    height: auto;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 4px;
}

.p-course-update-subpage .p-course-lesson-delete-button{
    width: 50px;
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
}

.p-course-update-subpage .p-cus-card-footer{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-course-update-subpage .p-cus-card-footer .p-cus-footer-button{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-end;
    align-items: center;
    gap: 8px;
}

.p-course-update-subpage .p-cus-not-found{
    width: 100%;
    height: 250px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.list-move {
    transition: transform 0.5s;
}


</style>

