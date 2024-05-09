

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
                v-for="(src, index) in srcs"
                :key="index"
                :src="src"
            />
        </a-image-preview-group>
    </div>
</template>



<script>


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
            current: 0
        }
    },
    mounted(){
    },
    methods: {
        async show(index = 0){
            if (!isNaN(index) && this.srcs.length > 0){
                index = Math.floor(Number(index));
                index = index % this.srcs.length;
                this.current = index;
            }
            this.visible = true;
        }
    }
}

</script>

<style scoped>

</style>

