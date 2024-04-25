<template>
    <div class="toc-container">
        <div class="toc" ref="toc" v-show="tocTree.length > 0">
            <ul>
                <li v-for="entry in tocTree" :key="entry.id">
                    <a :href="'#' + entry.id">{{ entry.title }}</a>
                    <ul v-if="entry.children.length > 0">
                        <li v-for="child in entry.children" :key="child.id">
                            <a :href="'#' + child.id">{{ child.title }}</a>
                        </li>
                    </ul>
                </li>
            </ul>
        </div>
        <div class="not-toc" v-show="tocTree.length <= 0">
            Không có mục lục
        </div>
    </div>
</template>

<script>


export default {
    name: "MarkdownToc",
    props: {
        markdownContent: {
            required: true,
        }
    },
    mounted() {
        this.tocTree = this.createTOCTree(this.markdownContent);
    },
    data() {
        return {
            tocTree: []
        };
    },
    methods: {
        createTOCTree(markdownText) {
            try {
                if (markdownText == null) {
                    throw new Error("Markdown content is null");
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
            this.tocTree = this.createTOCTree(this.markdownContent);
        }
    }
}
</script>

<style>

.toc-container{
    width: 100%;
}

.not-toc{
    font-size: 16px;
    font-family: 'ks-font-semibold';
    color: var(--primary-color);
}

</style>