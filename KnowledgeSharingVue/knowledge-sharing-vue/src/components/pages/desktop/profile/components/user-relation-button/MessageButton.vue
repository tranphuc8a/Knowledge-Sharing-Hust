

<template>
    <div>
        <MSecondaryButton 
            label="Nhắn tin"
            :onclick="resolveClickButton"
            :buttonStyle="buttonStyle"
            fa="comment" family="fas" :iconStyle="iconStyle"
        />
    </div>
</template>



<script>
import MSecondaryButton from '@/components/base/buttons/MSecondaryButton'

export default {
    name: 'MessageButton',
    components: {
        MSecondaryButton
    },
    props: {
    },
    data(){
        return {
            iconStyle: {
                fontSize: '20px'
            },
            buttonStyle: {
            
            },
        }
    },
    mounted(){

    },
    methods: {
        async resolveClickButton(){
            try {
                console.log("Click message button");
            } catch (e) {
                console.error(e);
            }
        }
    }
}

</script>

<style scoped>

</style>

