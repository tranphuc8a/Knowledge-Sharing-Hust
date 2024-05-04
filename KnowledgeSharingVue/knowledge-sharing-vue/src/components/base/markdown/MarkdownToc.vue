<template>
    <div class="toc-container">

        <div class="toc" ref="toc" v-show="tocTree?.length > 0">
            <div class="toc-title">
                MỤC LỤC
            </div>
            <div class="toc-items">
                <div v-for="entry in tocTree" :key="entry.id" class="p-item p-item-level-1">
                    <a :href="'#' + entry.id">{{ entry.title }}</a>
                    <div v-for="child in entry.children ?? []" :key="child.id" class="p-item p-item-level-2">
                        <a :href="'#' + child.id">{{ child.title }}</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="not-toc" v-show="tocTree?.length <= 0">
            Không có mục lục
        </div>
    </div>
</template>

<script>

import Common from '@/js/utils/common';

export default {
    name: "MarkdownToc",
    props: {
        markdownContent: {
            required: true,
        }
    },
    mounted() {
        this.updateTocTree();
    },
    data() {
        return {
            tocTree: [],
            mdContent: this.markdownContent,
        };
    },
    methods: {
        async updateTocTree(){
            this.mdContent = Common.unescapeSpecialCharacters(this.markdownContent);
            this.mdContent = Common.normalizeMarkdownText(this.mdContent);
            this.tocTree = this.createTOCTree(this.mdContent);
            // console.log(this.tocTree);
        },

        createTOCTree(markdownText) {
            try {
                if (markdownText == null) {
                    return null;
                }
                const lines = markdownText.split('\n');
                const tocTree = [];
                const stack = [{
                    level: 0,
                    children: tocTree
                }];
    
                lines.forEach(line => {
                    const result = line.match(/^(#+)\s?(.+)/);
                    if (result) {
                        const level = result[1].length;
                        const title = result[2].trim();
                        const id = title.toLowerCase().split(' ').join('-');
                        const tocEntry = {
                            title,
                            id,
                            level,
                            children: []
                        };
    
                        if (level > stack[stack.length - 1].level) {
                            // This entry is a sub-section of the previous one.
                            stack[stack.length - 1].children.push(tocEntry);
                        } else {
                            // Pop the stack to find the parent level
                            while (level <= stack[stack.length - 1].level) {
                                stack.pop();
                            }
                            stack[stack.length - 1].children.push(tocEntry);
                        }
                        stack.push(tocEntry); // Push the current entry onto the stack.
                    }
                });
    
                return tocTree;
            } catch (e) {
                console.error(e);
                return [];
            }
        }
    },
    watch: {
        markdownContent(){
            this.updateTocTree();
        }
    }
}
</script>

<style scoped>

.toc-container{
    width: 100%;
}


.toc, .toc .toc-items *{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    text-decoration: none;
    color: var(--primary-color);
    font-family: 'ks-font-semibold';
    width: 100%;
}

.toc-container .toc .toc-title{
    width: 100%;
    text-align: center;
    font-size: 22px;
    font-weight: 700;
    padding: 0 0 12px 0;
}

.toc-items{
    width: 100%;
}

.p-item-level-1{
    font-size: 15px;
}
.p-item-level-2{
    font-size: 14px;
}

.p-item{
    padding-left: 18px;
    text-align: left;
}

.p-item a {
    width: 100%;
    padding: 8px 10px;
    border-radius: 4px;

    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
}

.p-item a.active {
    background-color: var(--primary-color-100);
    color: var(--primary-color-600);

    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
}

.p-item-level-1 > a{
    padding: 10px 10px;
}

.p-item a:hover{
    background-color: var(--primary-color-100);
}

.not-toc{
    font-size: 16px;
    font-family: 'ks-font-semibold';
    color: var(--primary-color);
}

</style>