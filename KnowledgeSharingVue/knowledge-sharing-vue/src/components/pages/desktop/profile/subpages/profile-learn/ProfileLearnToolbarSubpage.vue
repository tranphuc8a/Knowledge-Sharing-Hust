

<template>

    <div class="p-profile-learn-toolbar-subpage">
        <div class="p-plts-card card">
            <div class="p-plts-card__header">
                <div class="p-header">
                    Các khóa học bạn đang học
                </div>
                <div class="p-button">
                    <MButton 
                        label="Khám phá các khóa học khác"
                        :onclick="resolveClickOtherCourse"
                        :buttonStyle="buttonStyle"
                    />
                </div>
            </div>
            <div class="p-plts-card__descrition">
                
            </div>
            <div class="p-plts-card__control">
                <div class="p-control-textfield">

                </div>
                <div class="p-control-order">

                </div>
            </div>
        </div>
    </div>
</template>



<script>
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue'


export default {
    name: 'ProfileLearnToolbarSubpage',
    components: {
        MSecondaryButton
    },
    props: {
    },
    data(){
        return {
            buttonStyle: {

            }
        }
    },
    mounted(){

    },
    methods: {
        async resolveClickOtherCourse(){
            try {
                await this.$store.dispatch('course/getOtherCourse')
                this.$router.push({
                    name: 'course.other-course'
                })
            } cactch (error){
                console.error(error)
            }
        }
    }
}

</script>

<style scoped>

</style>

