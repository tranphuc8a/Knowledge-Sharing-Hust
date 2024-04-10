

<template>
    <div class="pa-background">
        <div class="pa-form-container">
            <div class="pa-form">
                <div class="pa-logo-frame">
                    <div class="pa-logo"></div>
                </div>
                <slot />
            </div>
            <div class="pa-copyright">
                Copyright © 2024 by tranphuc8a
            </div>
        </div>
        <div class="pa-change-language-button">
            <ChangeLanguageButton />
        </div>
    </div>
</template>


<script>
import ChangeLanguageButton from '@/components/base/buttons/MChangeLanguageButton.vue';

export default {
    name: 'KSBaseAuthenticationPage',
    data(){
        return {
            label: null
        }
    },
    async mounted(){
    },
    components: {
        ChangeLanguageButton
    },
    methods: {}
}
</script>


<style scoped>
@import url(@/css/pages/authentication/base-page/base-authentication-page.css);
</style>

