<template>
    <div class="markdown-body" :style="style" v-html="renderedMarkdown"></div>
</template>

<script>
// Import thư viện marked
import marked from 'marked'

export default {
    name: "MarkdownRender",
    props: {
        markdownContent: {
            type: String,
            required: true
        },
        style: {}
    },
    computed: {

        renderedMarkdown() {
            return marked(this.markdownContent, { sanitize: true })
        }
    }
}
</script>
<style scoped>
.markdown-body {
    line-height: 1.6;
    font-size: 16px;
}
</style>