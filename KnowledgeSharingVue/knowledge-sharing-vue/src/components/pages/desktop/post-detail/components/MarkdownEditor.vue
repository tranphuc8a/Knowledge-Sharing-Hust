<template>

    <div class="p-editor-container">
        <div class="p-editor-error-message">
            <span v-if="currentState == stateEnum.Error" class="p-error-text">{{errorMsg}}</span>
        </div>
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
                        @:input="resolveOnInputThrottle"  
                        ></textarea>
                </div>
                <div class="p-preview-frame" v-show="editorViewMode != editorViewModeEnums.Normal">
                    <LatexMarkdownRender :markdownContent="dText" />
                </div>
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
import Debounce from '@/js/utils/debounce';

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

            // for validate:
            isDynamicValidate: false,
            errorMsg: '',
            stateEnum: {
                Normal: 0,
                Error: 1,
                Validated: 2
            },
            currentState: 0,

            resolveOnInputThrottle: null,
        }
    },
    methods: {
        initialEditor(){
            this.resolveOnInputThrottle = Debounce.throttle(this.resolveOnInput.bind(this), 500);
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
                await this.resolveDynamicValidate();
            } catch (e) {
                console.error(e);
            }
        },

        async resolveDynamicValidate(){
            try {
                if (this.isDynamicValidate) {
                    let isValid = await this.validate();
                    if (isValid) {
                        this.currentState = this.stateEnum.Validated;
                    } else {
                        this.currentState = this.stateEnum.Error;
                    }
                }
            } catch (e) {
                console.error(e);
            }
        },

        async resolveCommand(command){
            try {
                let textarea, cursorPosition, text, syntax, deltaCursor;
                switch (command) {
                    case this.editorCommandEnums.Undo:
                        textarea = this.$refs['textarea'];
                        this.dText = this.history.undo().markdownText;
                        this.pointerPotition = this.history.undo().cursorPointer;
                        this.setCursorPosition(this.pointerPotition);
                        break;
                    case this.editorCommandEnums.Redo:
                        textarea = this.$refs['textarea'];
                        this.dText = this.history.redo().markdownText;
                        this.pointerPotition = this.history.redo().cursorPointer;
                        this.setCursorPosition(this.pointerPotition);
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
                        this.setCursorPosition(this.pointerPotition);
                        this.history.addChange(new EditorState(this.dText, this.pointerPotition + 0));
                        textarea.focus();
                        break;
                }
            } catch (e) {
                console.error(e);
            }
        },


        async setCursorPosition(position){
            try {
                position = Number(position);
                this.pointerPotition = position;
                let textarea = this.$refs['textarea'];
                this.$nextTick(() => {
                    textarea.setSelectionRange(position, position);
                    textarea.focus();
                });
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
                if (Validator.isEmpty(value)){
                    value = '';
                }
                this.dText = value;
            } catch (e) {
                console.error(e);
            }
        },

        async focus(){
            try {
                this.$refs['textarea'].focus();
            } catch (e) {
                console.error(e);
            }
        },

        async validate(){
            try {
                if (this.validator != null) {
                    let res = await this.validator.validate(this.dText);
                    if (!res.isValid) {
                        this.errorMsg = res.msg;
                        return false;
                    }
                }
                return true;
            } catch (e) {
                console.error(e);
            }
        },

        async startDynamicValidate(){
            this.isDynamicValidate = true;
        },
        async stopDynamicValidate(){
            this.state = this.stateEnum.Normal;
            this.isDynamicValidate = false;
        }
    },
    props: {
        initialText: {
            type: String,
            default: ''
        },
        validator: {
            default: {
                async validate(value){
                    let isValid = Validator.isNotEmpty(value);
                    if (isValid) return {
                        isValid: true,
                        msg: ''
                    }
                    return {
                        isValid: false,
                        msg: 'Nội dung không được để trống'
                    }
                }
            }
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

.p-editor-container{
    width: 100%;
    height: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 4px;
}

.p-editor-error-message{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 4px;
    background-color: transparent;
}

.p-error-text{
    color: red;
    font-size: 16px;
}

textarea {
    height: 100%;
}

.p-editor-frame{
    width: 100%;
    height: 100%;
    min-height: 500px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 4px;
    border-radius: 8px;
    box-shadow: 0 0 2px 2px rgba(var(--primary-color-200-rgb), 1);
    background-color: var(--primary-color-200);
    border: solid 1px var(--primary-color-200);
}

.p-editor-frame * {
    border-radius: 8px;
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
    overflow: hidden;
    justify-content: space-between;
    align-items: center;
    gap: 4px;
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

.p-editor-frame .p-enter-frame .p-edit-frame,
.p-editor-frame .p-enter-frame .p-preview-frame{
    height: auto;
    align-self: stretch;
}

</style>

