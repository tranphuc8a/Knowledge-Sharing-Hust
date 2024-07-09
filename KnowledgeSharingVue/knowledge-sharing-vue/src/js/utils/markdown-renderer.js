
import Common from "@/js/utils/common";

import MarkdownIt from "markdown-it";
import 'github-markdown-css/github-markdown-light.css';

import ml from 'markdown-it-latex';
import 'markdown-it-latex/dist/index.css';

import mc from 'markdown-it-icons';
import 'markdown-it-icons/dist/index.css';

import mv from '@vrcd-community/markdown-it-video';
import me from 'markdown-it-emoji';
// import me from '@gerhobbelt/markdown-it-emoji';

// import mel from 'markdown-it-emoji/light';

import anchor from 'markdown-it-anchor';
// import TOC from 'markdown-it-toc-done-right';
import { Validator } from "@/js/utils/validator";


// import mis from 'markdown-it-sanitizer'

import sanitizeHtml from "sanitize-html";

class MarkdownRenderer{
    static _instance = null;
    static getInstance(){
        if (this._instance == null){
            this._instance = new MarkdownRenderer();
        }
        return this._instance;
    }

    constructor(){
        // this.anchorOptions = { permalink: true, permalinkBefore: true, permalinkSymbol: '§' };
        // this.tocOptions = {
        //     level: [2],
        //     containerClass: "toc",
        //     listClass: "tdesign-toc_list",
        //     itemClass: "tdesign-toc_list_item",
        //     linkClass: "tdesign-toc_list_item_a",
        //     listType: "ul"
        //     // format: (x, htmlencode) => {
        //     //   console.log(x, htmlencode);
        //     //   return `<span>${htmlencode(x)}</span>`;
        //     // },
        //     // callback: (res) => {
        //     //   console.log(res);
        //     // }
        // };
        this.misOptions = {
            allowedTags: [
                'a', 'abbr', 'b', 'blockquote', 'br', 'code', 'del', 'em', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6',
                'hr', 'i', 'img', 'kbd', 'li', 'ol', 'p', 'pre', 's', 'sup', 'sub', 'strong', 'table', 'tbody',
                'td', 'th', 'thead', 'tr', 'ul', 'div', 'span', 'iframe'
            ],
            allowedAttributes: {
                '*': ['class', 'id', 'style', 'align'],
                'a': ['href', 'title', 'target'],
                'img': ['src', 'alt', 'title', 'width', 'height'],
                'div': ['class', 'id', 'style', 'align'],
                'span': ['class', 'id', 'style', 'align'],
                'p': ['class', 'id', 'style', 'align'],
                'iframe': ['src', 'width', 'height', 'frameborder', 'allowfullscreen', 'allow', 'style']
            },
            removeUnbalanced: false,
            removeUnknown: false
        };
        this.mdOptions = {
            youtube: { width: 640, height: 390 },
            vimeo: { width: 500, height: 281 },
            vine: { width: 600, height: 600, embed: 'simple' },
            prezi: { width: 550, height: 400 },
            bilibili: { width: 640, height: 360 },
            osf: { width: 640, height: 360 },
            spotify: { width: 300, height: 380 },
        },
        this.mdRenderer = new MarkdownIt(
            {
                html: true,
                xhtmlOut: true,
                typographer: true,
                // linkify: true,
            }
        )
        // .use(mis, this.misOptions)
        .use(anchor, this.anchorOptions)
        // .use(TOC, this.tocOptions)
        .use(ml)
        .use(mc, 'emoji')
        .use(mc, 'font-awesome')
        .use(me)
        .use(mv, this.mdOptions);
    }

    /**
     * The function to render the markdown content
     * @param {*} content - The markdown content
     * @returns - The rendered content by html syntax
     * @Created PhucTV (15/6/24)
     * @Modified None
     * @Example let renderedContent = MarkdownRenderer.render(content);
     */
    render(content){
        try {
            if (Validator.isEmpty(content)){
                return "";
            }
            let temp = Common.unescapeSpecialCharacters(content);
            let renderedContent = this.mdRenderer.render(temp) ?? "";
            renderedContent = sanitizeHtml(renderedContent, this.misOptions);
            // console.log(renderedContent);
            return renderedContent;
        } catch (error) {
            console.error(error);
            return "";
        }
    }
}


export default MarkdownRenderer;
