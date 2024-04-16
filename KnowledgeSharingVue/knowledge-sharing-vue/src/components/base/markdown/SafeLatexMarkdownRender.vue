<template>
    <div class="markdown-content" :style="style" v-html="cleanContent"></div>
</template>

<script>
// xss defender
import DOMPurify from "dompurify";
// markdown compile and style
import MarkdownIt from "markdown-it";
import 'github-markdown-css/github-markdown-light.css';
// latex compile and style
import ml from 'markdown-it-latex';
import 'markdown-it-latex/dist/index.css'

export default {
    data() { 
        return {
            cleanContentConfigs: {
                ALLOWED_TAGS: ['b', 'i', 'em', 'strong', 'a'],
                ALLOWED_ATTR: ['href', 'title'], 
                ALLOW_DATA_ATTR: false,
                USE_PROFILES: {html: true} 
            },
            md: MarkdownIt().use(ml),
        } 
    },
    props: {
        markdownContent: {
            type: String,
            required: true,
        },
        style: {}
    },
    computed: {
        cleanContent() {
            // Sanitize trước khi convert markdown để đảm bảo an toàn
            const cleanMarkdown = this.checkXSSAttack(this.markdownContent);
            
            // Convert markdown content sang HTML
            return this.md.render(cleanMarkdown);
        },
    },
    methods: {
        // Hàm này sẽ sanitize đầu vào để tránh các nội dung nguy hiểm
        checkXSSAttack(content) {
            // return DOMPurify.sanitize(content, this.cleanContentConfgs);
            return DOMPurify.sanitize(content);
        },
    },
};
</script>


<style scoped>
.markdown-content{
    width: auto;
}
</style>