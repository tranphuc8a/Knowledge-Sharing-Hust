

<template>
    <div class="p-home-right-subpage" ref="scroll-container"
        @scroll="throttleResolveOnScroll"
    >
        <div class="p-his-content">
            <CourseSubpage 
                :get-course="getCourseCallback"
                :row-count="1"
            />
        </div>
    </div>
</template>



<script>
import CourseSubpage from '../components/course-subpage/CourseSubpage.vue';
import { GetRequest } from '@/js/services/request';
import Debounce from '@/js/utils/debounce';

export default {
    name: 'HomeRightSubpage',
    components: {
        CourseSubpage
    },
    props: {
    },
    data(){
        return {
            getCourseCallback: null,
            handlers: [],
            throttleResolveOnScroll: Debounce.throttle(this.resolveOnScroll.bind(this), 1000),
        }
    },
    async mounted(){
        await this.initSubpage();
    },
    methods: {
        async initSubpage(){
            try {
                let url = 'Courses';
                let currentUser = await this.getCurrentUser();
                if (currentUser == null){
                    url = 'Courses/anonymous';
                }
                this.getCourseCallback = async function(limit, offset){
                    let res = await new GetRequest(url)
                        .setParams({
                            limit: limit,
                            offset: offset
                        })
                        .execute();
                    return res;
                }.bind(this);
            } catch (e){
                console.error(e);
            }
        },

        async resolveOnScroll(){
            try {
                await Promise.all(this.handlers.map(handler => handler(this.$refs['scroll-container'])));
            } catch (error) {
                console.error(error);
            }
        },

        registerScrollHandler(handler){
            this.handlers.push(handler);
        }
    },
    provide(){
        return {
            registerScrollHandler: this.registerScrollHandler
        }
    },
    inject: {
        getCurrentUser: {},
    },
}

</script>


<style scoped>

.p-home-right-subpage{
    width: 100%;
    height: 100%;
    overflow-y: auto;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 16px;
}

</style>

