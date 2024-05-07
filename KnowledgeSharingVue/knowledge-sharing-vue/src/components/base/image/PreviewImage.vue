

<template>
    <a-image
        :style="{ display: 'none', width: 0 }"
        :preview="{
            visible,
            onVisibleChange: (value) => {
                visible = value;
            }
        }"
        :src="src"
    />
</template>



<script>


export default {
    name: 'PreviewImage',
    props: {
        src: {
            required: true
        },
    },
    data(){
        return {
            visible: false
        }
    },
    mounted(){
    },
    methods: {
        async show(){
            this.visible = true;
        }
    }
}

</script>

<style scoped>

</style>

