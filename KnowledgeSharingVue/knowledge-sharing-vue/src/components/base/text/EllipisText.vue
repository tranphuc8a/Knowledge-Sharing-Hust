

<template>

    <div class="p-ellipsis-text" :style="
        {
            '-webkit-line-clamp': maxline,
            ...style
        }">
        {{ text }}
    </div>

</template>



<script>
export default {
    name: 'EllipsisText',
    components: {
    },
    props: {
        text: {
            required: true,
        },
        maxline: {},
        style: {}
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
.p-ellipsis-text{
    width: 100%;
    text-align: justify;

    overflow: hidden;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
}

</style>

