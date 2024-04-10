

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
                <p> {{ isEnglish ? getLabel()?.english : getLabel()?.vietnamese }} </p>
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
    name: 'MChangeLanguageButton',
    data(){
        return {
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
    methods: {
        /**
         * Hàm thực hiện lấy về nhãn của button đổi ngôn ngữ
         * @param none
         * @return nhãn cần lấy
         * @Created PhucTV (19/1/24)
         * @Modified None
        */
        getLabel(){
            if (this.inject?.language != null){
                this.label = this.inject?.language?.pages?.login;
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
                    this.inject.changeLanguage(this.globalData.enum.language.VIETNAMESE);
                    this.isEnglish = false;
                } else if (lang === this.languageEnum.ENGLISH){
                    this.inject.changeLanguage(this.globalData.enum.language.ENGLISH);
                    this.isEnglish = true;
                }
                this.isFocus = false;
            } catch (error){
                console.error(error);
            }
        }
    },
    inject: {
        inject: {},
        getPopupManager: {}, 
        getToastManager: {}
    }
}
</script>


<style scoped>

@import url(@/css/base/button/change-language-button.css);

</style>
