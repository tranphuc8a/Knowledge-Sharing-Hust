<template>
    <div>
        <div class="toc" ref="toc"></div>
        <div v-html="renderedContent" :style="style" class="markdown-body"></div>
    </div>
</template>

<script>
import MarkdownIt from "markdown-it";
import 'github-markdown-css/github-markdown-light.css';

import ml from 'markdown-it-latex';
import 'markdown-it-latex/dist/index.css';

import anchor from 'markdown-it-anchor';
import TOC from 'markdown-it-toc-done-right';

export default {
    name: "LatexMardownRender",
    props: {
        markdownContent: {
            type: String,
            required: true,
        },
        style: {}
    },
    mounted() {
        this.toc = this.$refs?.toc;
    },
    data() {
        return {
            md: new MarkdownIt({
                html: true
            })
                .use(anchor)
                .use(TOC, { containerClass: 'toc' })
                .use(ml),
            tableOfContents: "",
            toc: null,
            renderedContent: ""
        };
    },
    watch: {
        markdownContent: {
            immediate: true,
            handler(newValue) {
                if (newValue) {
                    this.updateContent();
                }
            }
        }
    },
    methods: {
        updateContent() {
            try {
                this.renderedContent = this.md.render(this.markdownContent);
                this.$nextTick(() => {
                    this.tableOfContents = this.extractTableOfContents();
                });
            } catch (error) {
                console.error(error);
                this.renderedContent = "";
            }
        },
        extractTableOfContents() {
            return this.toc?.innerHTML || "";
        }
    }
};
</script>

<style>
.katex svg {
    overflow: hidden;
}
.toc {
    margin-bottom: 20px;
}
.markdown-body{
    overflow-x: hidden;
}
</style>
