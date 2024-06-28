

<template>
    <div class="p-user-card-skeleton">
        <div class="p-ucs-card-skeleton card"
            style="width: 100%; padding: 16px; display: flex; justify-content: space-between; align-items: center;">
            <div class="p-ucs__left"
                style="display: flex; gap: 16px;"
            >
                <div class="p-ucs__avatar skeleton"
                    style="width: 50px; height: 50px; border-radius: 50%;"
                ></div>

                <div class="p-ucs__userinfo"
                    style="display: flex; flex-direction: column; gap: 8px; justify-content: center;"
                >
                    <div class="p-ucs__fullname skeleton"
                        style="height: 18px; width: 150px;"
                    ></div>
                    <div class="p-ucs__username skeleton"
                        style="height: 18px; width: 100px;"
                    ></div>
                </div>

            </div>
            <div class="p-ucs__right">
                <div class="p-ucs__button skeleton"
                    style="width: 150px; height: 32px; border-radius: 8px;"
                >
                </div>
            </div>

        </div>
    </div>
</template>



<script>



export default {
    name: 'UserCardSkeleton',
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

.p-user-card-skeleton{
    width: 100%;
}

</style>

