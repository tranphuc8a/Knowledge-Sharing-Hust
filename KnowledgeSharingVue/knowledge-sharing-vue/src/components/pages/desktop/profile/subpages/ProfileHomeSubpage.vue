

<template>
    <div class="profile-home-subpage p-profile-main-subpage">
        <div class="profile-home-subpage__left">
            <!-- ProfileHomeResumeSubpage -->
            <ProfileHomeResumeSubpage />

            <!-- ProfileHomeImageSubpage -->
            <ProfileHomeImageSubpage v-if="isMySelf"/>

            <!-- ProfileHomeFriendSubpage -->
            <ProfileHomeFriendSubpage />
        </div>

        <div class="profile-home-subpage__right">
            <!-- ProfileHomeFeedSubpage -->
            <ProfileHomeFeedSubpage />
        </div>
    </div>
</template>



<script>
import ProfileHomeResumeSubpage from './ProfileHomeResumeSubpage.vue';
import ProfileHomeImageSubpage from './ProfileHomeImageSubpage.vue';
import ProfileHomeFriendSubpage from './ProfileHomeFriendSubpage.vue';
import ProfileHomeFeedSubpage from './ProfileHomeFeedSubpage.vue';

export default {
    name: 'ProfileHomeSubpage',
    components: {
        ProfileHomeResumeSubpage,
        ProfileHomeImageSubpage,
        ProfileHomeFriendSubpage,
        ProfileHomeFeedSubpage
    },
    props: {
    },
    data(){
        return {
            isMySelf: false,
        }
    },
    async mounted(){
        try {
            this.isMySelf = await this.getIsMySelf();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
    },
    inject: {
        getIsMySelf: {}
    }
}

</script>

<style>
.p-profile-main-subpage{
    max-width: var(--profile-page-max-width);
    width: 100%;
    height: fit-content;
}
</style>

<style scoped>

.profile-home-subpage{
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
}

.profile-home-subpage__left{
    width: 0;
    flex-shrink: 1;
    flex-grow: 2;

    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.profile-home-subpage__right{
    width: 0;
    flex-shrink: 1;
    flex-grow: 3;
}

</style>

