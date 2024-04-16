<template>
    <div class="p-comment">

    </div>

</template>

<script>


export default {
    name: "p-comment",
    data() {
        return {
            label: null,
            listComments: [null, null],
        }
    },
    components: {
    },
    methods: {
    },
    props: {
        comment: {}
    },
    mounted() {
        this.listComments = this.comments;
    },
    inject: {
        inject: {}
    },
    provider(){
        return {
            comment: this.comment
        }
    }
}

</script>

<style scoped>

</style>