

<template>
    <MContextPopup position="bottom">

        <template #popupContextMask>
            <MSecondaryButton
                :label="null"
                :onclick="()=>{}"
                :buttonStyle="buttonStyle"
                fa="ellipsis-h" family="fas" :iconStyle="iconStyle"
            />
        </template>

        <template #popupContextContent>
            <div class="p-popup-more-context-content">
                <template v-for="(option, index) in items" :key="option.key ?? index">
                    <router-link :to="option.link" class="router-link"> 
                        <div class="p-popup-more-context-option" >
                            <span> {{ option.label }} </span>
                        </div>
                    </router-link>
                </template>
            </div>
        </template>

    </MContextPopup>
</template>



<script>

import MContextPopup from '@/components/base/popup/MContextPopup.vue';
import MSecondaryButton from '@/components/base/buttons/MSecondaryButton';

export default {
    name: 'ProfilePanelMoreContextButton',
    components: {
        MContextPopup, MSecondaryButton
    },
    props: {
        items: {}
    },
    data(){
        return {
            buttonStyle: {
                padding: '16px'
            }
        }
    },
    mounted(){

    },
    methods: {
    }
}

</script>

<style scoped>

.p-popup-more-context-content{
    padding: 12px 0px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: flex-start;
    gap: 2px;
}

.p-popup-more-context-option{
    width: 100%;
    padding: 8px 24px 8px 24px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    font-family: 'ks-font-semibold';
    cursor: pointer;
    min-width: 200px;
}

.router-link{
    text-decoration: none;
    color: var(--grey-color-600);
    font-family: 'ks-font-semibold';
    width: 100%;
}


.p-popup-more-context-option:hover{
    background-color: var(--primary-color-100);
}

.p-popup-more-context-option:active{
    background-color: var(--primary-color-200);
}

</style>

