<template>
    <div class="markdown-body" :style="style" v-html="renderedMarkdown"></div>
 </template>

 <script>
// Import thư viện marked
import marked from 'marked'

export default {
    name: "MarkdownRender",
    props: {
        markdownText: {
            type: String,
            default: ''
        },
        style: {}
    },
    computed: {

        renderedMarkdown() {
            return marked(this.markdownText, { sanitize: true })
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