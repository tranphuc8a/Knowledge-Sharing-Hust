<template>
    <div>
        <div class="pr-divide">
            <div class="pr-segment-frame">
                <div class="pr-segment"></div>
            </div>
            <div class="pr-text-divide">
                {{ label }}
            </div>
        </div>
        <div class="pr-register-options">
            <div class="pr-option pr-register-google" @:click="resolveClickGoogleIcon">
            </div>
            <div class="pr-option pr-register-apple" @:click="resolveClickAppleIcon">
            </div>
            <div class="pr-option pr-register-microsoft" @:click="resolveClickMicrosoftIcon">
            </div>
        </div>
    </div>
    
</template>

<script>
import appConfig from '@/app-config';
import Common from '@/js/utils/common';

export default {
    name: 'OtherRegisterOptions',
    methods: {
        /**
         * Handle click google icon
         * 
         * @Param None
         * @Return None
         * @Created PhucTV (12/4/24)
         * @Modified None
         */
        async resolveClickGoogleIcon() {
            try {
                const googleOAuthUrl = 'https://accounts.google.com/o/oauth2/v2/auth';
                const client_id = '17403549430-no5m3svllo81fcnj5suedcg42meb204u.apps.googleusercontent.com';
                const baseUrl = Common.removeTrailingSlash(appConfig.getHomePageUrl());
                const redirect_uri = encodeURIComponent(`${baseUrl}/register/google`);
                const scope = 'openid+profile+email';
                const response_type = 'token';
                const state = 'knowledgesharinghusttoken22042001';
                const url = `${googleOAuthUrl}?client_id=${client_id}&redirect_uri=${redirect_uri}&scope=${scope}&response_type=${response_type}&state=${state}`;
                window.location.href = url;
            } catch (error) {
                console.error(error);
            }
        },


        /**
         * Handle click apple icon
         * 
         * @Param None
         * @Return None
         * @Created PhucTV (12/4/24)
         * @Modified None
         */
         async resolveClickAppleIcon() {
            try {
                const googleOAuthUrl = 'https://accounts.google.com/o/oauth2/v2/auth';
                const client_id = '17403549430-no5m3svllo81fcnj5suedcg42meb204u.apps.googleusercontent.com';
                const baseUrl = Common.removeTrailingSlash(appConfig.getHomePageUrl());
                const redirect_uri = encodeURIComponent(`${baseUrl}/register/google`);
                const scope = 'openid+profile+email';
                const response_type = 'token';
                const state = 'knowledgesharinghusttoken22042001';
                const url = `${googleOAuthUrl}?client_id=${client_id}&redirect_uri=${redirect_uri}&scope=${scope}&response_type=${response_type}&state=${state}`;
                window.location.href = url;
            } catch (error) {
                console.error(error);
            }
        },

        /**
         * Handle click microsoft icon
         * 
         * @Param None
         * @Return None
         * @Created PhucTV (12/4/24)
         * @Modified None
         */
         async resolveClickMicrosoftIcon() {
            try {
                const googleOAuthUrl = 'https://accounts.google.com/o/oauth2/v2/auth';
                const client_id = '17403549430-no5m3svllo81fcnj5suedcg42meb204u.apps.googleusercontent.com';
                const baseUrl = Common.removeTrailingSlash(appConfig.getHomePageUrl());
                const redirect_uri = encodeURIComponent(`${baseUrl}/register/google`);
                const scope = 'openid+profile+email';
                const response_type = 'token';
                const state = 'knowledgesharinghusttoken22042001';
                const url = `${googleOAuthUrl}?client_id=${client_id}&redirect_uri=${redirect_uri}&scope=${scope}&response_type=${response_type}&state=${state}`;
                window.location.href = url;
            } catch (error) {
                console.error(error);
            }
        }
    },
    props: {
        label: {
            default: 'Or login with'
        }
    }
}

</script>

<style scoped>

.pr-divide{
    width: 100%;
    position: relative;
    display: flex;
    flex-flow: row nowrap;
    padding-bottom: 16px;
    justify-content: center;
    align-items: center;
}

.pr-divide .pr-segment-frame{
    height: 100%;
    width: 100%;
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
}
.pr-divide .pr-segment-frame .pr-segment{
    width: 100%;
    height: 1px;
    background-color: #9ea1a5;
}

.pr-divide .pr-text-divide{
    width: fit-content;
    color: #9ea1a5;
    background-color: #fff;
    padding: 5px 10px;
    z-index: 1;
}

.pr-register-options{
    width: 100%;
    height: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    gap: 8px;
}

.pr-register-options .pr-option{
    width: 40px;
    height: 40px;
    background-size: 100% 100%;
    background-position: center;
    
    border-radius: 100%;
    cursor: pointer;
    box-sizing: border-box;
}

.pr-register-options .pr-option:hover{
    box-shadow: 0 1px 4px rgba(0,0,0,.16);
}

.pr-register-options .pr-register-google{
    background-image: url(@/assets/image-icons/icon-google.svg);
}
.pr-register-options .pr-register-apple{
    background-image: url(@/assets/image-icons/icon-apple.svg);
}
.pr-register-options .pr-register-microsoft{
    background-image: url(@/assets/image-icons/icon-microsoft.svg);
}

</style>