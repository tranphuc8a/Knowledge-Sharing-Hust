

<template>
    <div class="p-administrator-one-lesson-content">
        <div class="p-aolc-result-card card">
            <div class="p-aolc-card-header">
                <div class="card-heading">
                    <span> Kết quả tìm kiếm </span>
                </div>
            </div>
            <div class="p-aolc-result">
                <div class="p-aolc-result-list" v-if="!isLoaded">
                    <PostShortCardSkeleton />
                </div>
                <div class="p-aolc-result-list" v-if="isLoaded && isPostExisted">
                    <PostAdministatorShortCard :post="post"/>
                </div>
                <div class="p-aolc-result-notfound" v-if="isLoaded && !isPostExisted" >
                    <NotFoundPanel 
                        :text="errorText" 
                    />
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import NotFoundPanel from '@/components/base/popup/NotFoundPanel.vue';
import PostAdministatorShortCard from '../../components/administrator/admin-useritem/PostAdministatorShortCard.vue';
import PostShortCardSkeleton from '@/components/base/cards/PostShortCardSkeleton.vue';
import { useRoute } from 'vue-router';
import { GetRequest, Request } from '@/js/services/request';
import ViewLesson from '@/js/models/views/view-lesson';


export default {
    name: 'AdministratorOneLessonContent',
    components: {
        NotFoundPanel,
        PostAdministatorShortCard,
        PostShortCardSkeleton,
    },
    props: {
    },
    data(){
        return {
            route: useRoute(),
            post: null,
            filter: '',
            isLoaded: false,
            isPostExisted: false,
            errorText: 'Không tìm thấy bài giảng',
        }
    },
    async created(){
    },
    async mounted(){
        this.createdPage();
    },
    methods: {

        async createdPage(){
            try {
                this.isLoaded = false;
                this.filter = this.route.query['filter'];
                let url = 'Lessons/admin/' + this.filter;
                let res = await new GetRequest(url).execute();
                let body = await Request.tryGetBody(res);

                if (body == null){
                    this.isPostExisted = false;
                    this.post = null;
                    return;
                }

                this.post = new ViewLesson().copy(body);
                this.isPostExisted = true;
            } catch (e){
                let userMsg = await Request.tryGetUserMessage(e);
                if (userMsg != null){
                    this.errorText = userMsg;
                } else {
                    console.error(e);
                }
                this.isPostExisted = false;
                this.post = null;
            } finally {
                this.isLoaded = true;
            }
        },


        resolveOnDeletedLesson(){
            this.createdPage();
        }
    },
    watch: {
        '$route.query.filter': {
            handler(){
                this.createdPage();
            },
            deep: true,
        }
    },
    inject: {
        getToastManager: {},
        getPopupManager: {},
        getCurrentUser: {},
    },
    provide(){
        return {
            onPostDeleted: this.resolveOnDeletedLesson,
        }
    }
}

</script>


<style scoped>

.p-administrator-one-lesson-content{
    max-width: 100%;
    width: 100%;
    padding-bottom: 32px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    position: relative;
    gap: 16px;
}

.p-aolc-result-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-aolc-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}


.p-aolc-result{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-aolc-result-notfound{
    width: 100%;
    height: 300px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-aolc-result-list{
    width: 100%;
    display: flex;
    flex-flow: column wrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

