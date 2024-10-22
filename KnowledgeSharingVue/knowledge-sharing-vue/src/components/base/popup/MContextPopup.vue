<template>
    <div class="p-popup-context-container" tabindex="1" 
        @:click.stop="togglePopup" @:blur="hidePopup">
        <div class="p-popup-context-mask" ref="popup-context-mask">
            <slot name="popupContextMask"></slot>
        </div>
        <div v-show="isPopupContextVisible" v-if="isPopupCreated" class="p-popup-context-content" 
            :style="popupContextStyle" ref="popup-context-content">
            <!-- Nội dung của popupContext -->
            <slot name="popupContextContent"></slot>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                isPopupCreated: false,
                isPopupContextVisible: false,
                isWaitingToShow: false,
                isWaitingToHide: false,

                popupContextStyle: {},
                popupContextContent: null,
                popupContextMask: null,

                popupPosition: {
                    top: "top",
                    bottom: "bottom",
                },
                barHeight: 56,
                padding_horizontal: 8,
                padding_vertical: 8,
            };
        },
        mounted() {
            this.popupContextContent = this.$refs["popup-context-content"];
            this.popupContextMask = this.$refs["popup-context-mask"];
        },
        props: {
            position: {
                default: null
            },
            delayShowing: {
                default: 100
            },
            delayHiding: {
                default: 500
            },
        },
        methods: {
            async togglePopup(event){
                try {
                    // this.isPopupContextVisible = !this.isPopupContextVisible;
                    if (this.isPopupContextVisible){
                        await this.hidePopup();
                    } else {
                        await this.showPopup();
                    }
                    event.stopPropagation();
                } catch (e){
                    console.error(e);
                }

            },
            async hidePopup(delay) {
                this.isWaitingToHide = true;
                this.isWaitingToShow = false;
                await new Promise((resolve) => {
                    setTimeout(() => {
                        resolve();
                    }, isNaN(delay) ? this.delayHiding : delay);
                });
                if (!this.isWaitingToHide) return;

                this.isPopupContextVisible = false;
            },
            async showPopup(delay) {
                this.isWaitingToShow = true;
                this.isWaitingToHide = false;
                await new Promise((resolve) => {
                    setTimeout(() => {
                        resolve();
                    }, isNaN(delay) ? this.delayShowing : delay);
                });
                if (!this.isWaitingToShow) return;

                this.isPopupContextVisible = true;
                if (!this.isPopupCreated) {
                    this.isPopupCreated = true;
                }
                let that = this;
                this.$nextTick(async () => {
                    try {
                        const tempStyle = {};
                        await that.horizontalAlign(that, tempStyle);
                        await that.verticalAlign(that, tempStyle);
                        that.popupContextStyle = tempStyle;
                    } catch (e) {
                        console.error(e);
                    }     
                });

            },
            /**
             * Hàm xử lý can chinh popup theo chieu doc
             * @param {*} that this pointer
             * @param {*} style style cua popup
             * @returns none
             * @Created PhucTV (15/04/24)
             * @Modified None
            */
            async verticalAlign(that, style){
                const popupRect = that.$refs['popup-context-content'].getBoundingClientRect();
                const maskRect = that.$refs['popup-context-mask'].getBoundingClientRect();
                
                if (that.position == that.popupPosition.top) {
                    style.bottom = `${that.padding_vertical + maskRect.height}px`;
                } else if (that.position == that.popupPosition.bottom) {
                    style.top = `${that.padding_vertical + maskRect.height}px`;
                } else {
                    const minTopContent = maskRect.top - that.padding_vertical - popupRect.height - that.barHeight;
                    const maxBottomContent = maskRect.bottom + that.padding_vertical + popupRect.height;
                    if (minTopContent < 0 && maxBottomContent <= window.innerHeight) {
                        style.top = `${that.padding_vertical + maskRect.height}px`;
                    } else {
                        style.bottom = `${that.padding_vertical + maskRect.height}px`;
                        // alert(style.bottom);
                    }
                }
            },

            /**
             * Hàm xử lý can chinh popup theo chieu ngang
             * @param {*} that this pointer
             * @param {*} style style cua popup
             * @returns none
             * @Created PhucTV (15/04/24)
             * @Modified None
             */
            async horizontalAlign(that, style){
                const popupRect = that.$refs['popup-context-content'].getBoundingClientRect();
                const maskRect = that.$refs['popup-context-mask'].getBoundingClientRect();
                
                // align popup follow horizontally
                const minLeftContent = maskRect.left + maskRect.width/2 - popupRect.width/2;
                const maxRightContent = maskRect.left + maskRect.width/2 + popupRect.width/2;
                if (minLeftContent < 0) {
                    style.left = `${-maskRect.left + that.padding_horizontal}px`;
                } else {
                    if (maxRightContent <= window.innerWidth){
                        style.left = `${maskRect.width/2 - popupRect.width/2}px`;
                    } else {
                        style.right = `${-window.innerWidth + maskRect.right + that.padding_horizontal}px`;
                    }
                }
            },

            
            /**
             * Hàm xử lý can chinh tooltip theo chieu doc
             * @param {*} that this pointer
             * @param {*} style style cua tooltip
             * @returns none
             * @Created PhucTV (15/04/24)
             * @Modified None
            */
            async verticalAlignFixed(that, style){
                const popupRect = that.$refs['popup-context-content']?.getBoundingClientRect?.();
                const maskRect = that.$refs['popup-context-mask']?.getBoundingClientRect?.();
                if (popupRect == null || maskRect == null) return;
                
                if (that.position == that.popupPosition.top) {
                    style.bottom = `${-that.padding_vertical -popupRect.height + maskRect.top}px`;
                } else if (that.position == that.popupPosition.bottom) {
                    style.top = `${that.padding_vertical + maskRect.bottom}px`;
                } else {
                    const minTopContent = maskRect.top - that.padding_vertical - popupRect.height - that.barHeight;
                    const maxBottomContent = maskRect.bottom + that.padding_vertical + popupRect.height;
                    if (minTopContent < 0 && maxBottomContent <= window.innerHeight) {
                        style.top = `${that.padding_vertical + maskRect.bottom}px`;
                    } else {
                        style.top = `${-that.padding_vertical -popupRect.height + maskRect.top}px`;
                        // alert(style.bottom);
                    }
                }
            },

            /**
             * Hàm xử lý can chinh tooltip theo chieu ngang
             * @param {*} that this pointer
             * @param {*} style style cua tooltip
             * @returns none
             * @Created PhucTV (15/04/24)
             * @Modified None
             */
            async horizontalAlignFixed(that, style){
                const popupRect = that.$refs['popup-context-content']?.getBoundingClientRect?.();
                const maskRect = that.$refs['popup-context-mask']?.getBoundingClientRect?.();
                if (popupRect == null || maskRect == null) return;
                
                // align tooltip follow horizontally
                const minLeftContent = maskRect.left + maskRect.width/2 - popupRect.width/2;
                const maxRightContent = maskRect.left + maskRect.width/2 + popupRect.width/2;
                if (minLeftContent < 0) {
                    style.left = `${that.padding_horizontal}px`;
                } else {
                    if (maxRightContent <= window.innerWidth){
                        style.left = `${maskRect.left + maskRect.width/2 - popupRect.width/2}px`;
                    } else {
                        style.right = `${that.padding_horizontal}px`;
                    }
                }
            }
        },
    };
</script>

<style>

.p-popup-context-content *{
    max-width: initial;
}

</style>

<style scoped>
.p-popup-context-container {
    position: relative;
    width: 100%;
    height: 100%;
    max-width: 100%;
    max-height: 100%;
}

.p-popup-context-mask {
    cursor: pointer;
    width: 100%;
    height: 100%;
    max-width: 100%;
    max-height: 100%;
}

.p-popup-context-content {
    max-width: max-content;
    box-sizing: border-box;
    position: absolute;
    overflow: hidden;
    background-color: #fff;
    border-radius: 8px;
    box-shadow: 0 0px 2px rgba(0, 0, 0, 0.56);
    z-index: 9999;
    white-space: nowrap; /* Đảm bảo nội dung không bị wrap nếu dài */
}
</style>