

<template>
    <div class="p-scroll-to-top-button">
        <div class="p-sttb-frame" @:click="resolveOnClick">
            <MIcon fa="arrow-up" :style="iconStyle"/>
        </div>
    </div>
</template>



<script>

export default {
    name: 'ScrollToTopPopupButton',
    components: {
    },
    props: {
        onclick: {
            default: null
        }
    },
    data(){
        return {
            iconStyle: {
                fontSize: '20px',
            }
        }
    },
    async mounted(){
    },
    methods: {
        async resolveOnClick(){
            try {
                if (this.onclick != null){
                    await this.onclick();
                }
            } catch (e){
                console.error(e);
            }
        }
    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-scroll-to-top-button{
    width: fit-content;
    height: fit-content;
    position: fixed;
    bottom: 20px;
    right: 20px;
    z-index: 1000;
}

.p-sttb-frame{
    width: 50px;
    height: 50px;
    background-color: #f1f1f1;
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;
}

.p-sttb-frame:hover{
    background-color: #e1e1e1;
}

.p-sttb-frame:active{
    background-color: #d1d1d1;
}

</style>

