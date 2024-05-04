

<template>
    <DesktopHomeFrame>
        <div class="d-contentCO">
            
        </div>
    </DesktopHomeFrame>
</template>



<script>
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';

export default {
    name: 'ProfilePage',
    components: {
        DesktopHomeFrame
    },
    props: {
    },
    data(){
        return {
            title: '',
            isLessonExisted: false,
            lesson: null
        }
    },
    mounted(){

    },
    methods: {
    }
}

</script>

<style scoped>

</style>

