<template>
    <div class="p-tooltip-container" @mouseenter="showTooltip" @mouseleave="hideTooltip">
        <div class="p-tooltip-mask" ref="tooltip-mask">
            <slot name="tooltipMask"></slot>
        </div>
        <div v-show="isTooltipVisible" class="p-tooltip-content" :style="{ ...tooltipStyle, ...(style ?? {}) }" ref="tooltip-content">
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
            isWaitingToShow: false,
            isWaitingToHide: false,

            tooltipStyle: {},
            tooltipContent: null,
            tooltipMask: null,

            tooltipPosition: {
                top: "top",
                bottom: "bottom",
            },
            barHeight: 56,
            padding_horizontal: 0,
            padding_vertical: 8,
        };
    },
    mounted() {
        this.tooltipContent = this.$refs["tooltip-content"];
        this.tooltipMask = this.$refs["tooltip-mask"];
    },
    methods: {
        async hideTooltip(delay) {
            this.isWaitingToHide = true;
            this.isWaitingToShow = false;
            await new Promise((resolve) => {
                setTimeout(() => {
                    resolve();
                }, isNaN(delay) ? this.delayHiding : delay);
            });
            if (!this.isWaitingToHide) return;
            
            this.isTooltipVisible = false;
        },
        async showTooltip(delay) {
            this.isWaitingToShow = true;
            this.isWaitingToHide = false;
            await new Promise((resolve) => {
                setTimeout(() => {
                    resolve();
                }, isNaN(delay) ? this.delayShowing : delay);
            });
            if (!this.isWaitingToShow) return;
            
            this.isTooltipVisible = true;
            let that = this;
            this.$nextTick(async () => {
                try {
                    const tempStyle = {};
                    await that.horizontalAlign(that, tempStyle);
                    await that.verticalAlign(that, tempStyle);
                    that.tooltipStyle = tempStyle;
                } catch (e) {
                    console.error(e);
                }     
            });
        },

        /**
         * Hàm xử lý can chinh tooltip theo chieu doc
         * @param {*} that this pointer
         * @param {*} style style cua tooltip
         * @returns none
         * @Created PhucTV (15/04/24)
         * @Modified None
        */
        async verticalAlign(that, style){
            const toolTipRect = that.tooltipContent.getBoundingClientRect();
            const maskRect = that.tooltipMask.getBoundingClientRect();
            
            if (that.position == that.tooltipPosition.top) {
                style.bottom = `${that.padding_vertical + maskRect.height}px`;
            } else if (that.position == that.tooltipPosition.bottom) {
                style.top = `${that.padding_vertical + maskRect.height}px`;
            } else {
                const minTopContent = maskRect.top - that.padding_vertical - toolTipRect.height - that.barHeight;
                const maxBottomContent = maskRect.bottom + that.padding_vertical + toolTipRect.height;
                if (minTopContent < 0 && maxBottomContent <= window.innerHeight) {
                    style.top = `${that.padding_vertical + maskRect.height}px`;
                } else {
                    style.bottom = `${that.padding_vertical + maskRect.height}px`;
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
        async horizontalAlign(that, style){
            const toolTipRect = that.tooltipContent.getBoundingClientRect();
            const maskRect = that.tooltipMask.getBoundingClientRect();
            
            // align tooltip follow horizontally
            const minLeftContent = maskRect.left + maskRect.width/2 - toolTipRect.width/2;
            const maxRightContent = maskRect.left + maskRect.width/2 + toolTipRect.width/2;
            if (minLeftContent < 0) {
                style.left = `${-maskRect.left + that.padding_horizontal}px`;
            } else {
                if (maxRightContent <= window.innerWidth){
                    style.left = `${maskRect.width/2 - toolTipRect.width/2}px`;
                } else {
                    style.right = `${-window.innerWidth + maskRect.right + that.padding_horizontal}px`;
                }
            }
        }
    },
    props: {
        position: {
            default: null
        },
        delayShowing: {
            default: 500
        },
        delayHiding: {
            default: 500
        },
        style: {}
    }
};
</script>


<style scoped>
.p-tooltip-container {
    position: relative;
    width: 100%;
}

.p-tooltip-mask {
    cursor: pointer;
    width: 100%;
}

.p-tooltip-content {
    width: fit-content;
    box-sizing: border-box;
    position: absolute;
    overflow: hidden;
    background-color: #fff;
    border-radius: 8px;
    box-shadow: 0 0px 2px rgba(0, 0, 0, 0.56);
    /* padding: 10px; */
    z-index: 9999;
    white-space: nowrap; /* Đảm bảo nội dung không bị wrap nếu dài */
}
</style>