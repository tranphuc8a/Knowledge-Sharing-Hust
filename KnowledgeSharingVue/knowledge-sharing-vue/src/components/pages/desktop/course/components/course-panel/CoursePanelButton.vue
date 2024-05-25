

<template>
    <div class="p-course-panel-button" v-if="!isLoaded">
        <div class="p-cpb-cost">
            <div class="skeleton"
                style="width: 200px; height: 36px;"
            >
            </div>
        </div>

        <div class="p-cpb-button">
            <div class="skeleton"
                style="width: 200px; height: 36px;"
            >
            </div>
        </div>

    </div>

    <div class="p-course-panel-button" v-if="isLoaded">
        <div class="p-cpb-cost">
            <VisualizedCurrency :money="getCourse()?.Fee" />
        </div>

        <div class="p-cpb-button">
            <CourseRelationButton />
        </div>

    </div>
</template>



<script>
import VisualizedCurrency from './../course-cost/VisualizedCurrency.vue';
import CourseRelationButton from './../course-relation-button/CourseRelationButton.vue';
// import { useRouter } from 'vue-router';

export default {
    name: 'CoursePanelButton',
    components: {
        VisualizedCurrency,
        CourseRelationButton,
    },
    props: {
    },
    data(){
        return {
            isLoaded: false,
            iconStyle: {
                fontSize: '18px'
            },
            buttonStyle: {
                padding: '16px'
            },
            // currentUser: null,
            // isMyCourse: true,
            // router: useRouter(),
        }
    },
    mounted(){
        try {
            this.refresh();
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
        
        async refresh(){
            if (this.getCourse() != null){
                this.isLoaded = true;
            }
        }
    },
    inject: {
        getCourse: {}
    }
}

</script>

<style scoped>

.p-course-panel-button{
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-end;
    align-items: flex-end;
    gap: 12px;
}

.p-cpb-button{
    width: auto;
}

</style>

