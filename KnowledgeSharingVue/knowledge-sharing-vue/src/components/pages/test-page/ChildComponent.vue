<template>
    <div>
        Dữ liệu inject là: {{ this.inject.number }}
        Dữ liệu props là: {{  this.propNumber }}
    </div>
</template>

<script>
export default {
    name: 'ChildComponent',
    data(){
        return {

        }
    },
    mounted(){

    },
    methods: {

    },
    props: {
        propNumber: {}
    },
    inject: ['inject']
}
</script>

<style>

</style>
