<template>
    <div class="p-popup-context-container" tabindex="1" 
        @:click="togglePopup" @:blur="hidepopupContext">
        <div class="p-popup-context-mask" ref="popup-context-mask">
            <slot name="popupContextMask"></slot>
        </div>
        <div v-show="ispopupContextVisible" class="p-popup-context-content" :style="popupContextStyle" ref="popup-context-content">
            <!-- Nội dung của popupContext -->
            <slot name="popupContextContent"></slot>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                ispopupContextVisible: false,
                popupContextStyle: {},

                popupContextContent: null,
                popupContextMask: null,
            };
        },
        mounted() {
            this.popupContextContent = this.$refs["popup-context-content"];
            this.popupContextMask = this.$refs["popup-context-mask"];
        },
        watch: {
            async hoverCount(value) {
                this.ispopupContextVisible = value > 0;
                if (value > 0){
                    await this.showpopupContext();
                }
            },
        },
        methods: {
            async togglePopup(){
                this.ispopupContextVisible = !this.ispopupContextVisible;
                if (this.ispopupContextVisible){
                    await this.showpopupContext();
                } else {
                    await this.hidepopupContext();
                }
            },
            async hidepopupContext() {
                this.ispopupContextVisible = false;
            },
            async showpopupContext() {
                this.ispopupContextVisible = true;
                let that = this;
                this.$nextTick(() => {
                    try {
                        // console.log("window viewport: " + window.innerWidth + "x" + window.innerHeight);
                        const popupContextEl = that.popupContextContent;
                        const popupContextMaskEl = that.popupContextMask;
                        const barHeight = 56; // Chiều cao của thanh tiêu đề
                        const padding_vertical = 0; // Khoảng cách giữa popupContext và mask
                        const padding_horizontal = 0; // Khoảng cách giữa popupContext và mask
                        
                        const popupContextRect = popupContextEl.getBoundingClientRect();
                        const maskRect = popupContextMaskEl.getBoundingClientRect();
                        const tempStyle = {};

                        // align popupContext follow vertically
                        const minTopContent = maskRect.top - padding_vertical - popupContextRect.height - barHeight;
                        const maxBottomContent = maskRect.bottom + padding_vertical + popupContextRect.height;
                        if (minTopContent < 0 && maxBottomContent <= window.innerHeight) {
                            tempStyle.top = `${padding_vertical + maskRect.height}px`;
                        } else {
                            tempStyle.bottom = `${padding_vertical + maskRect.height}px`;
                        }

                        // align popupContext follow horizontally
                        const minLeftContent = maskRect.left + maskRect.width/2 - popupContextRect.width/2;
                        const maxRightContent = maskRect.left + maskRect.width/2 + popupContextRect.width/2;
                        if (minLeftContent < 0) {
                            tempStyle.left = `${-maskRect.left + padding_horizontal}px`;
                        } else {
                            if (maxRightContent <= window.innerWidth){
                                tempStyle.left = `${maskRect.width/2 - popupContextRect.width/2}px`;
                            } else {
                                tempStyle.right = `${-window.innerWidth + maskRect.right + padding_horizontal}px`;
                            }
                        }

                        that.popupContextStyle = tempStyle;
                    } catch (e) {
                        console.error(e);
                    }     
                });
            },
        },
    };
</script>


<style scoped>
.p-popup-context-container {
    position: relative;
    width: fit-content;
}

.p-popup-context-mask {
    cursor: pointer;
    width: fit-content;
}

.p-popup-context-content {
    box-sizing: border-box;
    position: absolute;
    background-color: #fff;
    border-radius: 4px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    padding: 10px;
    z-index: 9999;
    white-space: nowrap; /* Đảm bảo nội dung không bị wrap nếu dài */
}
</style>