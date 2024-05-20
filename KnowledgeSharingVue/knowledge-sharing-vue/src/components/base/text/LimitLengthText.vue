

<template>
    <div class="p-limit-text" :style="style">
        <span class="p-limit-text-content">
            <!-- <span v-show="!isCollapsed"> {{ dText }} </span> 
            <span v-show="isCollapsed">{{ collapsedText }} </span> -->

            <span v-show="!isCollapsed"> 
                <LatexMarkdownRender :markdownContent="dText" />
            </span> 
            <span v-show="isCollapsed">
                <LatexMarkdownRender :markdownContent="collapsedText" />    
            </span>
        </span>

        <span class="p-collapse-button" v-show="isEnableCollapse">
            <span> &nbsp; </span>
            <span v-show="!isCollapsed"
                @:click="toggleCollapse">Ẩn bớt</span>
            <span v-show="isCollapsed"
                @:click="toggleCollapse">Xem thêm</span>
        </span>
    </div>
</template>

<script>
import { Validator } from '@/js/utils/validator';
import LatexMarkdownRender from '../markdown/LatexMarkdownRender.vue';

export default {
    name: "LimitLengthText",
    data() {
        return {
            dText: null,
            collapsedText: null,
            draftData: "Slay quá slay `$x^2 - 3x + 2 = 0$` \n\n ## This is label label label label label label  \n\n Thôi được rồi",
            isEnableCollapse: false,
            isCollapsed: false
        }
    },
    components: {
        LatexMarkdownRender
    },
    mounted() {
        this.updateText();
    },
    methods: {

        /**
         * collapse text
         * @param {text} text
         * @returns {void}
         * @Created PhucTV (18/04/24)
         * @Modified None
         */
        collapseText(text, maxLength){
            try {
                text = String(text);
                if (text.length <= maxLength) {
                    return text;
                }
                return text.slice(0, maxLength) + '...';
            } catch (error){
                console.error(error);
                return "Error";
            }
        },

        /**
         * toggle collapse
         * @returns {void}
         * @Created PhucTV (18/04/24)
         * @Modified None
         */
        async toggleCollapse(){
            this.isCollapsed = !this.isCollapsed;
            if (this.onCollapse){
                this.onCollapse();
            }
        },

        async updateText(){
            try {
                this.isCollapsed = false;
                this.isEnableCollapse = false;

                this.dText = this.text;
                if (Validator.isEmpty(this.dText))
                    this.dText = this.draftData;
                if (this.dText?.length > this.length) {
                    this.isCollapsed = true;
                    this.isEnableCollapse = true;
                } 
                this.collapsedText = this.collapseText(this.dText, this.length);
            } catch (error){
                console.error(error);
            }
        }
    },
    watch: {
        text(){
            this.updateText();
        }
    },
    props: {
        text: {
            required: true
        },
        length: {
            default: 50
        },
        style: {},
        onCollapse: {}
    },
}

</script>

<style>

.p-limit-text{
    font-size: 13.5px;
    text-align: justify;
    height: fit-content;
    margin: 0;
    padding: 0;
    max-width: 100%;
}

.p-limit-text-content{
    max-width: 100%;
}

.p-collapse-button{
    font-family: 'ks-font-semibold';
    cursor: pointer;
    color: var(--primary-color);
    font-size: 13.5px
}

.p-collapse-button:hover{
    color: var(--primary-color-600);
}


</style>

