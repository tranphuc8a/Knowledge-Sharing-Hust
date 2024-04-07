<template>
    <div>
        <Textfield placeholder="Nhập mã sinh viên" ref="textfield" label="Mã số sinh viên" />
        <MButton label="Tìm kiếm" :onclick="searchImage" />
        <img :src="`data:image/png;base64,${imageData}`" alt="StudentImage">
    </div>
</template>

<script>

import MButton from '@/components/base/buttons/MNormalButton.vue';
import Textfield from '@/components/base/inputs/MTextfield.vue';
import { GetRequest } from '@/js/services/request';
export default {
    data() {
        return {
            imageData: ''
        };
    },
    mounted(){
        this.input = this.$refs.textfield;
    },
    components: {
        MButton, Textfield
    },
    methods: {
        trimQuotes(str) {
            if (str.startsWith('"') && str.endsWith('"')) {
                return str.slice(1, -1);
            }
            return str;
        },

        async searchImage() {
            try {
                let code = await this.input.getValue();
                if (code.length <= 10){
                    let apiUrl = `https://ctsv.hust.edu.vn/ctsv-img/getimagebystudentid?mssv=${code}`;
                    let response = await new GetRequest(apiUrl).execute();
                    this.imageData = response;
                } else {
                    this.imageData = this.trimQuotes(code);
                }
            } catch (error) {
                console.error(error);
            }
        }
    }
};
</script>