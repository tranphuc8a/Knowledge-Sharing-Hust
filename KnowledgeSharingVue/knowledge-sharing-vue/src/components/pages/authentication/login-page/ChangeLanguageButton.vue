

<template>
    <div class="pl-button-frame" tabindex="1" v-on:blur="isFocus=false">
        <div class="pl-button" v-on:click="isFocus = !isFocus">
            <div class="pl-button-left">
                <div class="pl-button-flag vietnam" v-show="!isEnglish">
                </div>
                <div class="pl-button-flag england" v-show="isEnglish">
                </div>
            </div>
            <div class="pl-button-right">
                {{ isEnglish ? getLabel()?.english : getLabel()?.vietnamese }}
            </div>
        </div>
        <div class="pl-menu-context" v-show="isFocus">
            <div class="pl-menu-item pl-menu-vietnamese"
                @:click="resolveChangeLanguage(languageEnum.VIETNAMESE)"
            >
                {{ getLabel()?.vietnamese }}
            </div>
            <div class="pl-menu-item pl-menu-vietnamese"
                @:click="resolveChangeLanguage(languageEnum.ENGLISH)"
            >
                {{ getLabel()?.english }}
            </div>
        </div>
    </div>
</template>


<script>
export default {
    name: 'KSChangeLanguageButton',
    data(){
        return {
            global: this.globalData,
            label: null,
            isEnglish: false,
            languageEnum: {
                VIETNAMESE: 0,
                ENGLISH: 1
            },
            isFocus: false
        }
    },
    mounted(){
        this.getLabel();
    },
    updated() {
        // console.log("Is Update: " + this.isUpdate);
    },
    methods: {
        /**
         * Hàm thực hiện lấy về nhãn của button đổi ngôn ngữ
         * @param none
         * @return nhãn cần lấy
         * @Created PhucTV (19/1/24)
         * @Modified None
        */
        getLabel(){
            if (this.getLanguage != null){
                this.label = this.getLanguage().pages.login;
            }
            return this.label;
        },

        /**
         * Hàm xử lý sự kiện nhấn thay đổi ngôn ngữ
         * @param {*} lang - ngôn ngữ cần thay đổi
         * @returns none
         * @Created PhucTV (19/1/24)
         * @Modified None 
        */
        resolveChangeLanguage(lang){
            try {
                if (lang === this.languageEnum.VIETNAMESE){
                    this.changeLanguage(this.globalData.language.VIETNAMESE);
                    this.isEnglish = false;
                } else if (lang === this.languageEnum.ENGLISH){
                    this.changeLanguage(this.globalData.language.ENGLISH);
                    this.isEnglish = true;
                }
            } catch (error){
                console.error(error);
            }
        }
    },
    inject: {
        getLanguage: {}, 
        changeLanguage: {}, 
        getPopupManager: {}, 
        getToastManager: {},
        isUpdate : {},
    }
}
</script>


<style scoped>

@import url(@/css/pages/login-page/change-language-button.css);

</style>
