

<template>
    <div class="p-profile-learn-content-subpage">
        
    </div>
</template>



<script>


export default {
    name: 'ProfileLearnContentSubpage',
    props: {
        listCourses: {
        }
    },
    data(){
        return {
            pageSize: 9,
            numberPages: 0,
            currentPage: 1,
        }
    },
    mounted(){
        this.refresh();
    },
    methods: {
        async refresh(){
            try {
                console.log(this.listCourses);
            } catch (e) {
                console.error(e);
            }    
        }
    },
    watch: {
        listCourses: {
            handler: function () {
                this.refresh();
            },
            deep: true
        }
    }
}

</script>

<style scoped>
.p-profile-learn-content-subpage {
    width: 100%;
}

</style>

