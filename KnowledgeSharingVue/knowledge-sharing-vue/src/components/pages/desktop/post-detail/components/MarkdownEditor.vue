<template>

    <div class="p-editor-frame">
        <div class="p-editor-toolbar">
            <MarkdownEditorToolbar :resolveCommand="resolveCommand" />
        </div>
        <div class="p-enter-frame">
            <div class="p-edit-frame" v-show="editorViewMode != editorViewModeEnums.Preview">
                <textarea 
                    ref="textarea" 
                    class="p-editor-textarea" 
                    v-model="dText"
                    :placeholder="placehoder" 
                    @:input="resolveOnInput"  
                    ></textarea>
            </div>
            <div class="p-preview-frame" v-show="editorViewMode != editorViewModeEnums.Normal">
                <LatexMarkdownRender :markdownContent="dText" />
            </div>
        </div>
    </div>

</template>


<script>

import MarkdownEditorToolbar from './MarkdownEditorToolbar.vue';
import { myEnum } from '@/js/resources/enum';
import LatexMarkdownRender from '@/components/base/markdown/LatexMarkdownRender.vue';
import markdownSyntax from '@/js/resources/markdown/markdown-syntax';
import { Validator } from '@/js/utils/validator';

class EditorState{
    constructor(content, cursorPointer){
        this.markdownText = content;
        this.cursorPointer = cursorPointer;
    }
}

class EditorHistory{
    constructor(){
        this.clear();

    }

    clear(){
        this.history = [];
        this.redoStack = [];
    }

    addChange(editorState){
        this.history.push(editorState);
        this.redoStack = [];
    }

    undo(){
        if(this.history.length > 1){
            const last = this.history.pop();
            this.redoStack.push(last);
            return this.history[this.history.length - 1];
        }
        return this.history[0];
    }

    redo(){
        if(this.redoStack.length > 0){
            const last = this.redoStack.pop();
            this.history.push(last);
            return this.history[this.history.length - 1];
        }
        return this.history[this.history.length - 1];
    }

    currentState(){
        return this.history.back();
    }

}

export default {
    name: 'MarkdownEditor',
    components: {
        MarkdownEditorToolbar, LatexMarkdownRender
    },
    data() {
        return {
            dText: '',
            pointerPotition: 0,
            history: null,
            placehoder: 'Có hỗ trợ văn bản markdown',
            editorViewMode: myEnum.editorViewMode.Normal,
            editorViewModeEnums: myEnum.editorViewMode,
            editorCommandEnums: myEnum.editorCommand,
        }
    },
    methods: {
        initialEditor(){
            this.dText = this.initialText;
            this.pointerPotition = 0;
            this.history = new EditorHistory();
            this.history.addChange(new EditorState(this.dText, this.pointerPotition));
        },

        async changeEditorViewMode(mode){
            try {
                this.editorViewMode = mode;
            } catch (e) {
                console.error(e);
            }
        },

        async resolveOnInput(){
            try {
                const textarea = this.$refs['textarea'];
                const cursorPosition = textarea.selectionStart;
                const text = this.dText;
                this.pointerPotition = cursorPosition;
                this.history.addChange(new EditorState(text, cursorPosition));
            } catch (e) {
                console.error(e);
            }
        },

        async resolveCommand(command){
            try {
                let textarea, cursorPosition, text, syntax, deltaCursor;
                switch (command) {
                    case this.editorCommandEnums.Undo:
                        this.dText = this.history.undo().markdownText;
                        this.pointerPotition = this.history.undo().cursorPointer;
                        break;
                    case this.editorCommandEnums.Redo:
                        this.dText = this.history.redo().markdownText;
                        this.pointerPotition = this.history.redo().cursorPointer;
                        break;
                    case this.editorCommandEnums.Preview:
                        if (this.editorViewMode != this.editorViewModeEnums.Preview) {
                            this.changeEditorViewMode(this.editorViewModeEnums.Preview);
                        } else {
                            this.changeEditorViewMode(this.editorViewModeEnums.Normal);
                        }
                        break;
                    case this.editorCommandEnums.Split:
                        if (this.editorViewMode != this.editorViewModeEnums.Split) {
                            this.changeEditorViewMode(this.editorViewModeEnums.Split);
                        } else {
                            this.changeEditorViewMode(this.editorViewModeEnums.Normal);
                        }
                        break;
                    default:
                        textarea = this.$refs['textarea'];
                        cursorPosition = textarea.selectionStart;
                        text = this.dText;
                        syntax = markdownSyntax[command].syntax;
                        deltaCursor = markdownSyntax[command].delta;

                        this.dText = text.slice(0, cursorPosition) + syntax + text.slice(cursorPosition);
                        this.pointerPotition = cursorPosition + deltaCursor;
                        textarea.setSelectionRange(this.pointerPotition, this.pointerPotition);
                        this.history.addChange(new EditorState(this.dText, this.pointerPotition));
                        textarea.focus();
                        break;
                }
            } catch (e) {
                console.error(e);
            }
        },


        async getValue(){
            try {
                return this.dText;
            } catch (e) {
                console.error(e);
            }
        },

        async setValue(value){
            try {
                this.history.clear();
                this.dText = value;
            } catch (e) {
                console.error(e);
            }
        },

        async validate(){
            try {
                return Validator.isNotEmpty(this.dText);
            } catch (e) {
                console.error(e);
            }
        }
    },
    props: {
        initialText: {
            type: String,
            default: ''
        }
    },
    mounted() {
        this.initialEditor();
    },
    watch: {
        initialText() {
            this.initialEditor();
        }
    },
    provide(){
        return {
            getEditorViewMode: () => this.editorViewMode,
            changeEditorViewMode: this.changeEditorViewMode,
        }
    }
}

</script>

<style scoped>

.p-editor-frame{
    width: 100%;
    height: 100%;
    min-height: 500px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    padding: 12px;
    gap: 4px;
    background-color: transparent;
}

.p-editor-toolbar{
    width: 100%;
    background-color: white;
}

.p-enter-frame{
    width: 100%;
    height: 50%;
    flex-grow: 1;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
    background-color: transparent;
}

.p-enter-frame > div{
    height: 100%;
    flex-shrink: 1;
    flex-grow: 1;
    width: 50%;
    background-color: #fff;
    text-align: start;
    font-size: 16px;
    box-sizing: border-box;
}

.p-editor-textarea{
    width: 100%;
    height: 100%;
    padding: 8px;
    font-size: 16px;
    font-family: 'ks-font-regular';
    border: none;
    outline: none;
    resize: none;
}

.p-preview-frame{
    box-sizing: border-box;
    padding: 12px;
    overflow: auto;
}

</style>

