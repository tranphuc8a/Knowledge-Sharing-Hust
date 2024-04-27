<template>
    <div class="markdown-container">
        <div v-html="renderedContent" :style="style" class="markdown-body"></div>
    </div>
</template>

<script>
import Common from "@/js/utils/common";

import MarkdownIt from "markdown-it";
import 'github-markdown-css/github-markdown-light.css';

import ml from 'markdown-it-latex';
import 'markdown-it-latex/dist/index.css';

import anchor from 'markdown-it-anchor';
import TOC from 'markdown-it-toc-done-right';
import { Validator } from "@/js/utils/validator";

export default {
    name: "LatexMardownRender",
    props: {
        markdownContent: {
            required: true,
        },
        style: {}
    },
    mounted() {
        this.toc = this.$refs?.toc;
    },
    data() {
        return {
            anchorOptions: { permalink: true, permalinkBefore: true, permalinkSymbol: '§' },
            tocOptions: {
                level: [2],
                containerClass: "toc",
                listClass: "tdesign-toc_list",
                itemClass: "tdesign-toc_list_item",
                linkClass: "tdesign-toc_list_item_a",
                listType: "ul"
                // format: (x, htmlencode) => {
                //   console.log(x, htmlencode);
                //   return `<span>${htmlencode(x)}</span>`;
                // },
                // callback: (res) => {
                //   console.log(res);
                // }
            },
            md: new MarkdownIt({
                html: true,
                xhtmlOut: true,
                typographer: true
            })
                .use(anchor, this.anchorOptions)
                .use(TOC, { containerClass: 'toc' })
                .use(ml),
            tableOfContents: "",
            toc: null,
            renderedContent: "",
            mdContent: this.markdownContent,
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
                if (Validator.isEmpty(this.markdownContent)){
                    this.renderedContent = "";
                    return;
                }
                this.mdContent = Common.unescapeSpecialCharacters(this.markdownContent);
                // console.log(this.mdContent);
                this.renderedContent = this.md.render(this.mdContent) ?? "";
            } catch (error) {
                console.error(error);
                this.renderedContent = "";
            }
        },
        extractTableOfContents() {
            return this.toc?.innerHTML || "";
        },

        

    }
};
</script>

<style scoped>
.markdown-container{
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
