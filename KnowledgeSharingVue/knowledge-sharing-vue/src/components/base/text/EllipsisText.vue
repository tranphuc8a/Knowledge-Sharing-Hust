

<template>

    <div class="p-ellipsis-text" :title="text"
        :style="
        {
            '-webkit-line-clamp': maxline,
            ...style
        }">
        <span>
            {{ normalizeWhitespace(text) }}
        </span>
    </div>

</template>



<script>
import { Validator } from '@/js/utils/validator';

export default {
    name: 'EllipsisText',
    components: {
    },
    props: {
        text: {
            required: true,
        },
        maxline: {},
        style: {}
    },
    data(){
        return {
            
        }
    },
    async mounted(){
    },
    methods: {
        normalizeWhitespace(str) {
            if (Validator.isEmpty(str)) return "";
            str = String(str);
            
            // Thay thế tất cả các ký tự tab, xuống dòng, breakline thành một khoảng trắng
            str = str.replace(/\s+/g, ' ');

            // Loại bỏ khoảng trắng ở đầu và cuối chuỗi
            str = str.trim();

            return str;
        }
    },
    inject: {
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>
.p-ellipsis-text{
    width: 100%;
    text-align: left;

    overflow: hidden;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;

    white-space: break-spaces;
}

</style>

