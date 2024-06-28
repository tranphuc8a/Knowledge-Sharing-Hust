

<template>
    <div class="p-administrator-one-post-content">
        <div class="p-aopc-result-card card">
            <div class="p-aopc-card-header">
                <div class="card-heading">
                    <span> Kết quả tìm kiếm </span>
                </div>
            </div>
            <div class="p-aopc-result">
                <div class="p-aopc-result-list" v-if="!isLoaded">
                    <PostShortCardSkeleton />
                </div>
                <div class="p-aopc-result-list" v-if="isLoaded && isPostExisted">
                    <PostAdministatorShortCard :post="post"/>
                </div>
                <div class="p-aopc-result-notfound" v-if="isLoaded && !isPostExisted" >
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
import { myEnum } from '@/js/resources/enum';
import ViewLesson from '@/js/models/views/view-lesson';
import ViewQuestion from '@/js/models/views/view-question';


export default {
    name: 'AdministratorOnePostContent',
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
            errorText: 'Không tìm thấy bài viết',
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
                let url = 'Posts/admin?filter=UserItemId:' + this.filter;
                let res = await new GetRequest(url).execute();
                let body = await Request.tryGetBody(res);
                body = body[0];

                if (body == null){
                    this.isPostExisted = false;
                    this.post = null;
                    return;
                }

                if (body.PostType == myEnum.EPostType.Lesson){
                    this.post = new ViewLesson().copy(body);
                } else {
                    this.post = new ViewQuestion().copy(body);
                }
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

        async resolveDeletedPost(){
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
            onPostDeleted: this.resolveDeletedPost,
        }
    }
}

</script>


<style scoped>

.p-administrator-one-post-content{
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

.p-aopc-result-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-aopc-card-header{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}


.p-aopc-result{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

.p-aopc-result-notfound{
    width: 100%;
    height: 300px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: center;
}

.p-aopc-result-list{
    width: 100%;
    display: flex;
    flex-flow: column wrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
}

</style>

