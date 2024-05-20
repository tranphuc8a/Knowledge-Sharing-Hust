

<template>
    <div style="display: none">
        <a-image-preview-group 
            :preview="{ 
                visible, 
                onVisibleChange: vis => (visible = vis),
                current
            }"
        >
            <a-image
                v-for="(src, index) in dSrcs"
                :key="index"
                :src="src"
            />
        </a-image-preview-group>
    </div>
</template>



<script>
import Common from '@/js/utils/common';

export default {
    name: 'PreviewImageGroup',
    props: {
        srcs: {
            required: true,
            type: Array
        },
    },
    data(){
        return {
            visible: false,
            current: 0,
            dSrcs: []
        }
    },
    mounted(){
        this.refresh();
    },
    methods: {
        async show(index = 0){
            if (!isNaN(index) && this.srcs.length > 0){
                index = Math.floor(Number(index));
                index = index % this.srcs.length;
                this.current = index;
            }
            this.visible = true;
        },

        async refresh(){
            try {
                this.dSrcs = [];
                let awaitAll = await Promise.all(this.srcs.map(src => Common.isValidImage(src)));
                this.dSrcs = this.srcs.map((src, index) => {
                    return awaitAll[index] ? src : null;
                    // return awaitAll[index] ? null : null;
                }).filter(src => src !== null);
            } catch (e){
                console.error(e);
            }
        }
    },
    watch: {
        srcs(){
            this.refresh();
        }
    },
}

</script>

<style scoped>

</style>

