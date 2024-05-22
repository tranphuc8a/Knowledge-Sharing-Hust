

<template>
    <a-image
        :style="{ display: 'none', width: 0 }"
        :preview="{
            visible,
            onVisibleChange: (value) => {
                visible = value;
            }
        }"
        :src="dSrc"
    />
</template>



<script>
// import Common from '@/js/utils/common';

export default {
    name: 'PreviewImage',
    props: {
        src: {
            required: true
        },
    },
    data(){
        return {
            visible: false,
            dSrc: null
        }
    },
    mounted(){
        this.refresh();
    },
    methods: {
        async show(){
            this.visible = true;
        },
        async refresh(){
            try {
                // this.dSrc = null;
                // if (await Common.isValidImage(this.src)) {
                //     this.dSrc = this.src;
                //     return;
                // }
                this.dSrc = this.src;
            } catch (e){
                console.error(e);
            }
        }
    },
    watch: {
        src(){
            this.refresh();
        }
    }
}

</script>

<style scoped>

</style>

