<template>
    <div class="pl-option pl-login-google" @click="resolveClickIcon">
    </div>
</template>

<script>
export default {
    name: 'LoginWithGoogleIcon',
    methods: {
        async resolveClickIcon() {
            try {
                const url = 'https://accounts.google.com/o/oauth2/v2/auth?client_id=17403549430-no5m3svllo81fcnj5suedcg42meb204u.apps.googleusercontent.com&redirect_uri=http%3A%2F%2Flocalhost%3A8080%2Flogin%2Fgoogle&scope=openid+profile+email&response_type=token&state=123321123321123321';
                window.location.href = url;
            } catch (error) {
                console.error(error);
            }
        }
    }
}
</script>


<style scoped>

.pa-form .pl-login-options{
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

.pl-login-google{
    background-image: url(@/assets/image-icons/icon-google.svg);
}
.pl-login-apple{
    background-image: url(@/assets/image-icons/icon-apple.svg);
}
.pl-login-microsoft{
    background-image: url(@/assets/image-icons/icon-microsoft.svg);
}

</style>
