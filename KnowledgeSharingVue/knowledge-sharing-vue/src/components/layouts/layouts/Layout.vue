<template>
    <div>
        
        <Content> 
            <slot>
                This is page content
            </slot>
        </Content>
        <Header />
        <Sidebar />
    </div>
</template>

<script>
import Content from '@/components/layouts/content/KSContent.vue';
import Header from '@/components/layouts/header/KSHeader.vue';
import Sidebar from '@/components/layouts/sidebar/KSSidebar.vue';

export default {
    name: 'KSLayout',
    components: {
        Content, Sidebar, Header
    },
    data(){  
        return {}   
    },
    mounted() {},
    methods: { 
        
        /**
         * Xử lý sự kiện click bất kỳ
         * @param {*} value 
         * @returns
         * @Created PhucTV (22/3/24)
         * @Modified None
         */
        async resolveOnclick(value){
            this.getPopupManager().error("HEllo World" + value);
        },   
    }
}
</script>

<style>
@import url(@/css/layouts/position-layout.css);
.block{
    width: fit-content;
}
</style>
