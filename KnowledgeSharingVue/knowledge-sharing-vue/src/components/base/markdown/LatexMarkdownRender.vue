<template>
    <div v-html="renderedContent" :style="style" class="markdown-body"></div>
</template>

<script>
import MarkdownIt from "markdown-it";
import 'github-markdown-css/github-markdown-light.css';

import ml from 'markdown-it-latex';
import 'markdown-it-latex/dist/index.css';

export default {
    name: "LatexMardownRender",
    props: {
        markdownContent: {
            type: String,
            required: true,
        },
        style: {}
    },
    data() {
        return {
            md: new MarkdownIt({
                html: true
            }).use(ml),
        };
    },
    computed: {
        renderedContent() {
            try {
                return this.md.render(this.markdownContent);
            }
            catch (error){
                console.error(error);
                return "";
            }
        },
    },
};
</script>

<style>
/* @import url(@/css/base/latex/latex.css); */
.katex svg{
    overflow: hidden;
}
</style>
