<template>
    <DesktopHomeFrame>
        <div class="d-content">
            <MarkdownEditor ref="editor" />
        </div>
    </DesktopHomeFrame>

</template>


<script> 
import DesktopHomeFrame from '../home/DesktopHomeFrame.vue';
import MarkdownEditor from './components/MarkdownEditor.vue';


export default {
    name: 'CreateLessonPage',
    components: {
        DesktopHomeFrame, MarkdownEditor
    },
    data(){
        return {

        }
    },
    methods: {

    },
    props: {

    },
    inject: {

    }
}

</script>


<style>

</style>


