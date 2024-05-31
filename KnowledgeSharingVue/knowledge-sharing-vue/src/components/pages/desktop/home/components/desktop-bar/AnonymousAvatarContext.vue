

<template>
    <div class="p-anonymous-avatar-context">
        <MMenuContextPopup :options="getOptions()">
            <Avatar 
                :src="defaultThumbnail"
                :size="40"
                title="Ẩn danh"
            />
        </MMenuContextPopup>
    </div>
</template>



<script>
import MMenuContextPopup from '@/components/base/popup/MMenuContextPopup.vue';
import Avatar from '@/components/base/avatar/Avatar.vue';

export default {
    name: 'AnonymousAvatarContext',
    components: {
        MMenuContextPopup,
        Avatar
    },
    props: {
    },
    data(){
        return {
            defaultThumbnail: require('@/assets/default-thumbnail/student-image-icon.png')
        }
    },
    async mounted(){
    },
    methods: {

        getOptions(){
            return [
                {
                    fa: 'right-to-bracket',
                    onclick: this.resolveClickLogin.bind(this),
                    label: 'Đăng nhập'
                },
                {
                    fa: 'address-card',
                    onclick: this.resolveClickSignup.bind(this),
                    label: 'Đăng ký'
                }
            ]
        },

        resolveClickLogin(){
            this.$router.push('/login');
        },

        resolveClickSignup(){
            this.$router.push('/register');
        }
    },
    inject: {
    },
    provide(){
        return {}
    }
}

</script>


<style scoped>

.p-anonymous-avatar-context{
    width: 100%;
    height: 100%;
}

</style>

