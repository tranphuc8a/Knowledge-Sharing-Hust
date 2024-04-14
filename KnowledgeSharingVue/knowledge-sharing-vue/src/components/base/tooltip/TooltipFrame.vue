<template>
    <div class="p-tooltip-container" @mouseenter="showTooltip" @mouseleave="hideTooltip">
        <div class="p-tooltip-mask" ref="tooltip-mask">
            <slot name="tooltipMask"></slot>
        </div>
        <div v-show="isTooltipVisible" class="p-tooltip-content" :style="tooltipStyle" ref="tooltip-content">
            <!-- Nội dung của tooltip -->
            <slot name="tooltipContent"></slot>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                isTooltipVisible: false,
                tooltipStyle: {},
                hoverCount: 0,
                tooltipContent: null,
                tooktipMask: null,
            };
        },
        mounted() {
            this.tooltipContent = this.$refs["tooltip-content"];
            this.tooktipMask = this.$refs["tooltip-mask"];
        },
        watch: {
            async hoverCount(value) {
                this.isTooltipVisible = value > 0;
                if (value > 0){
                    await this.showTooltip();
                }
            },
        },
        methods: {
            async hideTooltip() {
                this.isTooltipVisible = false;
            },
            async showTooltip() {
                this.isTooltipVisible = true;
                let that = this;
                this.$nextTick(() => {
                    try {
                        console.log("window viewport: " + window.innerWidth + "x" + window.innerHeight);
                        const tooltipEl = that.tooltipContent;
                        const tooltipMaskEl = that.tooktipMask;
                        const barHeight = 56; // Chiều cao của thanh tiêu đề
                        const padding_vertical = 0; // Khoảng cách giữa tooltip và mask
                        const padding_horizontal = 0; // Khoảng cách giữa tooltip và mask
                        
                        const toolTipRect = tooltipEl.getBoundingClientRect();
                        const maskRect = tooltipMaskEl.getBoundingClientRect();
                        const tempStyle = {};

                        // align tooltip follow vertically
                        const minTopContent = maskRect.top - padding_vertical - toolTipRect.height - barHeight;
                        const maxBottomContent = maskRect.bottom + padding_vertical + toolTipRect.height;
                        if (minTopContent < 0 && maxBottomContent <= window.innerHeight) {
                            tempStyle.top = `${padding_vertical + maskRect.height}px`;
                        } else {
                            tempStyle.bottom = `${padding_vertical + maskRect.height}px`;
                        }

                        // align tooltip follow horizontally
                        const minLeftContent = maskRect.left + maskRect.width/2 - toolTipRect.width/2;
                        const maxRightContent = maskRect.left + maskRect.width/2 + toolTipRect.width/2;
                        if (minLeftContent < 0) {
                            tempStyle.left = `${-maskRect.left + padding_horizontal}px`;
                        } else {
                            if (maxRightContent <= window.innerWidth){
                                tempStyle.left = `${maskRect.width/2 - toolTipRect.width/2}px`;
                            } else {
                                tempStyle.right = `${-window.innerWidth + maskRect.right + padding_horizontal}px`;
                            }
                        }

                        that.tooltipStyle = tempStyle;
                    } catch (e) {
                        console.error(e);
                    }     
                });
            },
        },
    };
</script>


<style scoped>
.p-tooltip-container {
    position: relative;
    width: fit-content;
}

.p-tooltip-mask {
    cursor: pointer;
    width: fit-content;
}

.p-tooltip-content {
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