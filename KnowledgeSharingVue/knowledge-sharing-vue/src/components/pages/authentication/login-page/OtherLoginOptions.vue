<template>
    <div>
        <div class="pl-divide">
            <div class="pl-segment-frame">
                <div class="pl-segment"></div>
            </div>
            <div class="pl-text-divide">
                {{ label }}
            </div>
        </div>
        <div class="pl-login-options">
            <div class="pl-option pl-login-google" @:click="resolveClickGoogleIcon">
            </div>
            <div class="pl-option pl-login-apple" @:click="resolveClickAppleIcon">
            </div>
            <div class="pl-option pl-login-microsoft" @:click="resolveClickMicrosoftIcon">
            </div>
        </div>
    </div>
    
</template>

<script>
import appConfig from '@/app-config';
import Common from '@/js/utils/common';
import { useRouter } from 'vue-router';

export default {
    name: 'OtherLoginOptions',
    data() {
        return {
            router: useRouter()
        }
    },
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
                const redirect_uri = encodeURIComponent(`${baseUrl}/login/google`);
                const scope = 'openid+profile+email';
                const response_type = 'token';
                const state = 'knowledgesharinghusttoken22042001';
                const url = `${googleOAuthUrl}?client_id=${client_id}&redirect_uri=${redirect_uri}&scope=${scope}&response_type=${response_type}&state=${state}`;
                // window.location.href = url;
                this.router.push(url);
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
                const redirect_uri = encodeURIComponent(`${baseUrl}/login/google`);
                const scope = 'openid+profile+email';
                const response_type = 'token';
                const state = 'knowledgesharinghusttoken22042001';
                const url = `${googleOAuthUrl}?client_id=${client_id}&redirect_uri=${redirect_uri}&scope=${scope}&response_type=${response_type}&state=${state}`;
                // window.location.href = url;
                this.router.push(url);
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
                const redirect_uri = encodeURIComponent(`${baseUrl}/login/google`);
                const scope = 'openid+profile+email';
                const response_type = 'token';
                const state = 'knowledgesharinghusttoken22042001';
                const url = `${googleOAuthUrl}?client_id=${client_id}&redirect_uri=${redirect_uri}&scope=${scope}&response_type=${response_type}&state=${state}`;
                // window.location.href = url;
                this.router.push(url);
            } catch (error) {
                console.error(error);
            }
        }
    },
    props: {
        label: {
            default: 'Or register with'
        }
    }
}

</script>

<style scoped>

.pl-divide{
    width: 100%;
    position: relative;
    display: flex;
    flex-flow: row nowrap;
    padding-bottom: 16px;
    justify-content: center;
    align-items: center;
}

.pl-divide .pl-segment-frame{
    height: 100%;
    width: 100%;
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
}
.pl-divide .pl-segment-frame .pl-segment{
    width: 100%;
    height: 1px;
    background-color: #9ea1a5;
}

.pl-divide .pl-text-divide{
    width: fit-content;
    color: #9ea1a5;
    background-color: #fff;
    padding: 5px 10px;
    z-index: 1;
}

.pl-login-options{
    width: 100%;
    height: fit-content;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
    gap: 8px;
}

.pl-login-options .pl-option{
    width: 40px;
    height: 40px;
    background-size: 100% 100%;
    background-position: center;
    
    border-radius: 100%;
    cursor: pointer;
    box-sizing: border-box;
}

.pl-login-options .pl-option:hover{
    box-shadow: 0 1px 4px rgba(0,0,0,.16);
}

.pl-login-options .pl-login-google{
    background-image: url(@/assets/image-icons/icon-google.svg);
}
.pl-login-options .pl-login-apple{
    background-image: url(@/assets/image-icons/icon-apple.svg);
}
.pl-login-options .pl-login-microsoft{
    background-image: url(@/assets/image-icons/icon-microsoft.svg);
}

</style>