
// import { MyRandom } from "./myrandom";
import MarkdownRenderer from "./markdown-renderer";

class TocCreator{
    
    /**
     * The function to create the table of contents from the markdown string
     * @param {*} markdownString - The markdown string
     * @returns - The table of contents
     * @Created PhucTV (15/6/24)
     * @Modified None
     * @Example let toc = TocCreator.createTocFromMarkdownString(markdownString);
     */
    static createTocFromMarkdownString(markdownString){
        try {
            const toc = [];
            const lines = markdownString.split('\n');
            for (let i = 0; i < lines.length; i++) {
                const line = lines[i];
                if (line.startsWith('#')) {
                    const level = line.split(' ')[0].length;
                    const title = line.substring(level).trim();
                    toc.push({level, title});
                }
            }
            return toc;
        } catch (e){
            console.error(e);
            return [];
        }
    }


    /**
     * The function to create the table of contents from the html string
     * @param {*} htmlString - The html string
     * @returns - The table of contents
     * @Created PhucTV (15/6/24)
     * @Modified None
     * @Example let toc = TocCreator.createTocFromHtmlString(htmlString);
     */
    static createTocFromHtmlString(htmlString){
        try {
            const parser = new DOMParser();
            const doc = parser.parseFromString(htmlString, 'text/html');
            const headers = doc.querySelectorAll('h1, h2, h3, h4, h5, h6');
            console.log(headers);
            let tocStack = [{
                level: 0,
                title: 'Table of Contents',
                id: 'toc',
                children: [],
            }];
            let stackTop = tocStack[0];
            for (let header of headers) {
                const level = parseInt(header.tagName[1]);
                const title = header.textContent.trim();
                const id = header.id;
                while (stackTop.level >= level) {
                    tocStack.pop();
                    stackTop = tocStack[tocStack.length - 1];
                }
                for (let i = stackTop.level + 1; i <= level; i++) {
                    const newStackTop = {
                        level: i,
                        title: title,
                        id: id, 
                        children: []
                    };
                    stackTop.children.push(newStackTop);
                    tocStack.push(newStackTop);
                    stackTop = newStackTop;
                }
            }
            return tocStack[0].children;
        } catch (e){
            console.error(e);
            return [];
        }
    }

    /**
     * The function to create the table of contents from the markdown string by html syntax
     * @param {*} markdownString - The markdown string
     * @returns - The table of contents
     * @Created PhucTV (15/6/24)
     * @Modified None
     * @Example let toc = TocCreator.createTocFromMarkdownRenderByHtmlSyntax(markdownString);
     */
    static createTocFromMarkdownRenderByHtmlSyntax(markdownString){
        let htmlParsered = MarkdownRenderer.getInstance().render(markdownString);
        return TocCreator.createTocFromHtmlString(htmlParsered);
    }
}

export default TocCreator;