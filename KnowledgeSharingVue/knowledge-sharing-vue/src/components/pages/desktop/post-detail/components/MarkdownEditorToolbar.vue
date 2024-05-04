<template>
    <div class="p-editor-toolbar">
        <div class="p-editor-group">
            <div title="Bold" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Bold)">
                <MIcon fa="bold" family="fas" :style="iconStyle"/>
            </div>
            <div title="Italic" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Italic)">
                <MIcon fa="italic" family="fas" :style="iconStyle"/>
            </div>
            <div title="Underline" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Underline)">
                <MIcon fa="underline" family="fas" :style="iconStyle"/>
            </div>
            <div title="Strike through" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Strikethrough)">
                <MIcon fa="strikethrough" family="fas" :style="iconStyle"/>
            </div>
            <div title="Subscript" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Subscript)">
                <MIcon fa="subscript" family="fas" :style="iconStyle"/>
            </div>
            <div title="Superscript" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Superscript)">
                <MIcon fa="superscript" family="fas" :style="iconStyle"/>
            </div>
        </div>        

        <div class="p-editor-toolbar-devide"></div>

        <div class="p-editor-group">
            <div title="Heading 1" class="p-editor-toolbar-item p-manual-icon"
                @:click="emitCommand(commandType.Heading1)">
                <span> H<sub>1</sub> </span>
            </div>
            <div title="Heading 2" class="p-editor-toolbar-item p-manual-icon"
                @:click="emitCommand(commandType.Heading2)">
                <span> H<sub>2</sub> </span>
            </div>
            <div title="Heading 3" class="p-editor-toolbar-item p-manual-icon"
                @:click="emitCommand(commandType.Heading3)">
                <span> H<sub>3</sub> </span>
            </div> 
            <div title="Heading 4" class="p-editor-toolbar-item p-manual-icon"
                @:click="emitCommand(commandType.Heading4)">
                <span> H<sub>4</sub> </span>
            </div>
            <div title="Heading 5" class="p-editor-toolbar-item p-manual-icon"
                @:click="emitCommand(commandType.Heading5)">
                <span> H<sub>5</sub> </span>
            </div>
            <div title="Heading 6" class="p-editor-toolbar-item p-manual-icon"
                @:click="emitCommand(commandType.Heading6)">
                <span> H<sub>6</sub> </span>
            </div>  
        </div>
        

        <div class="p-editor-toolbar-devide"></div>

        <div class="p-editor-group">
            <div title="Code" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Code)">
                <MIcon fa="code" family="fas" :style="iconStyle"/>
            </div>
            <div title="Quote Right" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Blockquote)">
                <MIcon fa="quote-right" family="fas" :style="iconStyle"/>
            </div>
            <div title="Paragraph" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Paragraph)">
                <MIcon fa="paragraph" family="fas" :style="iconStyle"/>
            </div>
            <div title="New line" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Newline)">
                <MIcon fa="turn-down" family="fas" :style="iconStyle"/>
            </div>
        </div>

        <div class="p-editor-toolbar-devide"></div>

        <div class="p-editor-group">
            <div title="Unordered list" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.UnorderedList)">
                <MIcon fa="list-ul" family="fas" :style="iconStyle"/>
            </div>
            <div title="Ordered list" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.OrderedList)">
                <MIcon fa="list-ol" family="fas" :style="iconStyle"/>
            </div>
            <div title="Indent" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Indent)">
                <MIcon fa="indent" family="fas" :style="iconStyle"/>
            </div>
            <div title="Outdent" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Outdent)">
                <MIcon fa="outdent" family="fas" :style="iconStyle"/>
            </div>
            
        </div>
        

        <div class="p-editor-toolbar-devide"></div>

        <div class="p-editor-group">
            <div title="Link" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Link)">
                <MIcon fa="link" family="fas" :style="iconStyle"/>
            </div>
            <div title="Image" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Image)">
                <MIcon fa="image" family="fas" :style="iconStyle"/>
            </div>
            <div title="Table" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Table)">
                <MIcon fa="table" family="fas" :style="iconStyle"/>
            </div>
        </div>
        

        <div class="p-editor-toolbar-devide"></div>
        <div class="p-editor-group">
            <div title="Align left" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.AlignLeft)">
                <MIcon fa="align-left" family="fas" :style="iconStyle"/>
            </div>
            <div title="Align right" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.AlignRight)">
                <MIcon fa="align-right" family="fas" :style="iconStyle"/>
            </div>
            <div title="Align center" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.AlignCenter)">
                <MIcon fa="align-center" family="fas" :style="iconStyle"/>
            </div>
            <div title="Align justify" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.AlignJustify)">
                <MIcon fa="align-justify" family="fas" :style="iconStyle"/>
            </div>
        </div>

        <div class="p-editor-toolbar-devide"></div>
        <div class="p-editor-group">
            <div title="Split view" class="p-editor-toolbar-item p-item-active"
                v-show="getEditorViewMode?.() == editorViewMode.Both"
                    @:click="emitCommand(commandType.Split)">
                <MIcon fa="table-columns" family="fas" :style="iconStyle"/>
            </div>
            <div title="Split views" class="p-editor-toolbar-item"
                v-show="getEditorViewMode?.() != editorViewMode.Both"
                    @:click="emitCommand(commandType.Split)">
                <MIcon fa="table-columns" family="fas" :style="iconStyle"/>
            </div>
            <div title="Preview" class="p-editor-toolbar-item p-item-active"
                v-show="getEditorViewMode?.() == editorViewMode.Preview"
                    @:click="emitCommand(commandType.Preview)">
                <MIcon fa="eye" family="fas" :style="iconStyle"/>
            </div>
            <div title="Preview" class="p-editor-toolbar-item"
                v-show="getEditorViewMode?.() != editorViewMode.Preview"
                    @:click="emitCommand(commandType.Preview)">
                <MIcon fa="eye" family="fas" :style="iconStyle"/>
            </div>
            <div title="Undo" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Undo)">
                <MIcon fa="rotate-left" family="fas" :style="iconStyle"/>
            </div>
            <div title="Redo" class="p-editor-toolbar-item"
                @:click="emitCommand(commandType.Redo)">
                <MIcon fa="rotate-right" family="fas" :style="iconStyle"/>
            </div>
        </div>

        <div class="p-editor-toolbar-devide"></div>

        <div class="p-editor-group">
            <div title="Mathemetic formula" class="p-editor-toolbar-item p-manual-icon"
                @:click="emitCommand(commandType.LaTeX)">
                <span> LaTeX </span>
            </div> 
        </div>
        
    </div>
</template>


<script>

import { myEnum } from '@/js/resources/enum';

export default{
    name: 'MarkdownEditorToolbar',
    data(){
        return {
            iconStyle: {
                width: '18px',
                height: '18px'
            },
            editorViewMode: myEnum.editorViewMode,
            commandType: myEnum.editorCommand
        }
    },
    methods: {
        emitCommand(command){
            // console.log(command);
            this.resolveCommand?.(command);
        }
    },
    props: {
        resolveCommand: {}
    },
    inject: {
        getEditorViewMode: {},
        changeEditorViewMode: {}
    }
}

</script>


<style scoped>

.p-editor-toolbar{
    display: flex;
    flex-flow: row wrap;
    justify-content: flex-start;
    align-items: center;
    gap: 4px;
    padding: 8px;
    border-radius: 4px;
}

.p-editor-group{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 1px;
} 

.p-editor-toolbar-item{
    padding: 0 5px;
    height: 32px;
    border-radius: 4px;
    cursor: pointer;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    box-sizing: border-box;
    border: solid 1px transparent;
}

.p-editor-toolbar-devide{
    width: 1px;
    height: 24px;
    background-color: var(--primary-color-300);
    margin: 0 4px;
}

.p-manual-icon{
    color: var(--primary-color);
    font-size: 18px;
    font-weight: 600;
} 

.p-editor-toolbar-item:hover{
    background-color: var(--primary-color-200);
} 

.p-editor-toolbar-item:active{
    background-color: var(--primary-color-300);
}

.p-item-active{
    border: solid 1px var(--primary-color);
}


</style>
