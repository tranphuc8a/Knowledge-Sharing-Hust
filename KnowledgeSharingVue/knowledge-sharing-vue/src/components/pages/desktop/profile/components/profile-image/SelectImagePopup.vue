

<template>
    <div class="p-select-image-popup">
        <div class="p-popup-background">
            <div class="p-popup-content card">

            </div>
        </div>
    </div>
</template>



<script>

import CurrentUser from '@/js/models/entities/current-user';


export default {
    name: 'SelectImagePopup',
    components: {
    },
    props: {
    },
    data(){
        return {
            currentUser: null,
        }
    },
    async mounted(){
        await this.initPopup();
    },
    methods: {
        async initPopup(){
            try {
                this.currentUser = await CurrentUser.getInstance();
            } catch (e){
                console.error(e);
                return;
            }
        },

        async show(){

        },
        async hide(){

        },
    },
    inject: {
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

</style>

