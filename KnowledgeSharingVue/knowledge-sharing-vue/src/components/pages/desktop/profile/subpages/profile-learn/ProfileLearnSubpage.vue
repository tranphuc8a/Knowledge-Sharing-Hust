

<template>
    <div class="profile-learn-subpage p-profile-main-subpage">
        
    </div>
</template>



<script>


export default {
    name: 'ProfileLearnSubpage',
    components: {
        
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


<style scoped>

.profile-learn-subpage{
    display: flex;
    flex-flow: row nowrap;
    gap: 16px;
    justify-content: space-between;
    align-items: flex-start;
    padding-bottom: 32px;
}

</style>

