

<template>
    
    <UserCard :user="dUser">
        <UserAdministratorButton :is-call-api-when-create="false"/>
    </UserCard>
    
    
</template>



<script>
import UserCard from './UserCard.vue';
import UserAdministratorButton from './UserAdministratorButton.vue';

export default {
    name: 'UserRelationCard',
    components: {
        UserAdministratorButton,
        UserCard,
    },
    props: {
        user: {
            type: Object,
            required: true,
        }
    },
    watch: {
        user: {
            handler(){
                this.refresh();
            },
            deep: true,
        }
    },
    data(){
        return {
            dUser: this.user
        }
    },
    async mounted(){
        this.dUser = this.user;
    },
    methods: {
        async refresh(){
            try {
                this.dUser = this.user;
            } catch (e){
                console.error(e);
            }
        
        }
    },
    inject: {
    },
    provide(){
        return {
            getUser: () => this.dUser,
        }
    }
}

</script>

