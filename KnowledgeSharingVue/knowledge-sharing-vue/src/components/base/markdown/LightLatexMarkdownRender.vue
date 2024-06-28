<template>
    <div class="markdown-container">
        <div v-html="renderedContent" :style="style" class="markdown-body"></div>
    </div>
</template>

<script>

import MarkdownRenderer from "@/js/utils/markdown-renderer";

export default {
    name: "LightLatexMardownRender",
    props: {
        markdownContent: {
            required: true,
        },
        style: {}
    },
    mounted() {
        this.toc = this.$refs?.toc;
        this.updateContent();
    },
    data() {
        return {
            toc: null,
            renderedContent: "",
            renderer: MarkdownRenderer.getInstance(),
        };
    },
    watch: {
        markdownContent: {
            immediate: true,
            handler() {
                this.updateContent();
            }
        }
    },
    methods: {
        updateContent() {
            this.renderedContent = this.renderer.render(this.markdownContent);
        },
        extractTableOfContents() {
            return this.toc?.innerHTML || "";
        },
    }
};
</script>

<style scoped>
.markdown-container{
    max-width: 100%;
    font-size: inherit;
}
.katex svg {
    overflow: hidden;
}
.toc {
    margin-bottom: 20px;
}
.markdown-body{
    overflow-x: hidden;
    background-color: transparent;
    font-size: inherit;
}
</style>
