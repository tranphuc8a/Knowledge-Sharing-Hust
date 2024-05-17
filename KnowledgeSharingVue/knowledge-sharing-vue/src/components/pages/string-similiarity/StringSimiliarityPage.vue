

<template>
    <div class="p-page">
        <div class="p-page-two-string">
            <div class="p-page-two-string__form">
                <div>
                    <MTextfield 
                        label="Chuỗi A" placeholder="Chuỗi A"
                        :is-show-icon="false" :is-show-title="true" :is-show-error="false" :is-obligate="false"
                        :onclick-icon="()=>{}" 
                        :validator="null"
                        state="normal"
                        ref="a-textfield"
                        :oninput="resolveChangeFormOne"
                    />
                </div>
                <div>
                    <MTextfield 
                        label="Chuỗi B" placeholder="Chuỗi B"
                        :is-show-icon="false" :is-show-title="true" :is-show-error="false" :is-obligate="false"
                        :onclick-icon="()=>{}" 
                        :validator="null"
                        state="normal"
                        ref="b-textfield"
                        :oninput="resolveChangeFormOne"
                    />
                </div>
            </div>
            <div class="p-page-two-string__form">

            </div>
        </div>

        <div class="p-page-combine">

        </div>
    </div>
</template>



<script>
import MTextfield from './../../base/inputs/MTextfield.vue';
import StringAlgorithm from '@/js/utils/string-algorithm';

export default {
    name: 'StringSimiliarityPage',
    components: {
        MTextfield
    },
    props: {
    },
    data(){
        return {
            formOne: {

            }
        }
    },
    mounted(){

    },
    methods: {
        async resolveChangeFormOne(){
            try {
                let atextfield = this.$refs['a-textfield'];
                let btextfield = this.$refs['b-textfield'];
                let atext = await atextfield.getValue();
                let btext = await btextfield.getValue();
                
            } catch (e){
                console.error(e);
            }
        }
    }
}

</script>

<style scoped>

</style>

