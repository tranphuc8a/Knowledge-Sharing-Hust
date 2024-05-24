

<template>
    <div class="p-course-user-card-skeleton">
        <div class="p-pfc-card-skeleton card"
            style="width: 100%; padding: 16px; display: flex; justify-content: space-between; align-items: center;">
            <div class="p-pfc__left"
                style="display: flex; gap: 16px;"
            >
                <div class="p-pfc__avatar skeleton"
                    style="width: 50px; height: 50px; border-radius: 50%;"
                ></div>

                <div class="p-pfc__userinfo"
                    style="display: flex; flex-direction: column; gap: 8px; justify-content: center;"
                >
                    <div class="p-pfc__fullname skeleton"
                        style="height: 18px; width: 150px;"
                    ></div>
                    <div class="p-pfc__username skeleton"
                        style="height: 18px; width: 100px;"
                    ></div>
                </div>

            </div>
            <div class="p-pfc__right">
                <div class="p-pfc__button skeleton"
                    style="width: 150px; height: 32px; border-radius: 8px;"
                >
                </div>
            </div>

        </div>
    </div>
</template>



<script>



export default {
    name: 'CourseUserCardSkeleton',
    components: {
    },
    props: {
    },
    data(){
        return {
            
        }
    },
    async mounted(){
    },
    methods: {

    },
    inject: {
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-course-user-card-skeleton{
    width: 100%;
}

</style>

