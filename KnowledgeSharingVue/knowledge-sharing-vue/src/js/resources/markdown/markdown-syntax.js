/* eslint-disable */
import { myEnum } from "../enum";
const commandType = myEnum.editorCommand;

export default {
    [commandType.Bold]: {
        syntax: '****',
        delta: 2
    },
    [commandType.Italic]: {
        syntax: '**',
        delta: 1
    },
    [commandType.Strikethrough]: {
        syntax: '~~~~',
        delta: 2
    },
    [commandType.Underline]: {
        syntax: '____',
        delta: 2
    },
    [commandType.Blockquote]: {
        syntax: '>  ',
        delta: 2
    },
    [commandType.Heading1]: {
        syntax: '#  ',
        delta: 2
    },
    [commandType.Heading2]: {
        syntax: '##  ',
        delta: 3
    },
    [commandType.Heading3]: {
        syntax: '###  ',
        delta: 4
    },
    [commandType.Heading4]: {
        syntax: '####  ',
        delta: 5
    },
    [commandType.Heading5]: {
        syntax: '#####  ',
        delta: 6
    },
    [commandType.Heading6]: {
        syntax: '######  ',
        delta: 7
    },
    [commandType.OrderedList]: {
        syntax: '1.  ',
        delta: 3
    },
    [commandType.UnorderedList]: {
        syntax: '*  ',
        delta: 2
    },
    [commandType.Link]: {
        syntax: '[text](url)',
        delta: 1
    },
    [commandType.Image]: {
        syntax: '![alt](url)',
        delta: 2
    },
    [commandType.Code]: {
        syntax: '```\n\n```',
        delta: 4
    },
    [commandType.Table]: {
        syntax: '| Column 1 | Column 2 |\n| -------- | -------- |\n| Row 1    | Row 1    |',
        delta: 3
    },
    [commandType.Subscript]: {
        syntax: '<sub>  </sub>',
        delta: 6
    },
    [commandType.Superscript]: {
        syntax: '<sup>  </sup>',
        delta: 6
    },
    [commandType.AlignLeft]: {
        syntax: '<div style="text-align: left;">\n\n</div>',
        delta: 31
    },
    [commandType.AlignCenter]: {
        syntax: '<div style="text-align: center;">\n\n</div>',
        delta: 34
    },
    [commandType.AlignRight]: {
        syntax: '<div style="text-align: right;">\n\n</div>',
        delta: 33
    },
    [commandType.AlignJustify]: {
        syntax: '<div style="text-align: justify;">\n\n</div>',
        delta: 35
    },
    [commandType.LaTeX]: {
        syntax: '\`$  $\`',
        delta: 3
    },
    [commandType.Paragraph]: {
        syntax: '\n\n',
        delta: 2
    },
    [commandType.Newline]: {
        syntax: '  \n',
        delta: 3
    },
    [commandType.Indent]: {
        syntax: '    ',
        delta: 4
    },
    [commandType.Outdent]: {
        syntax: '\b\b\b\b',
        delta: 4
    },

}
