<template>
    <div>
        <Textfield placeholder="Nhập mã sinh viên" ref="textfield" label="Mã số sinh viên" />
        <Textfield placeholder="Nhập mật khẩu" ref="password" label="Mật khẩu" />
        <MButton label="Tìm kiếm" :onclick="searchImage" />
        <MButton label="Login qldt" :onclick="loginQldt" />
        <img :src="`data:image/png;base64,${imageData}`" alt="StudentImage">
    </div>
</template>

<script>

import MButton from '@/components/base/buttons/MButton.vue';
import Textfield from '@/components/base/inputs/MTextfield.vue';
import { GetRequest, PostRequest } from '@/js/services/request';
import { myEnum } from '@/js/resources/enum';
export default {
    data() {
        return {
            imageData: '',
            mssv: null,
            password: null
        };
    },
    mounted(){
        this.mssv = this.$refs.textfield;
        this.password = this.$refs.password;
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
                let code = await this.mssv.getValue();
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
        },

        async loginQldt(){
            try {
                // let mssv = await this.mssv.getValue();
                // let password = await this.password.getValue();
                let body = '7|0|8|https://qldt.hust.edu.vn/soicteducation/|874A341421F9ED9DCFEECAEAE669B269|com.soict.edu.core.client.service.AuthenticationService|login|java.lang.String/2004016611|20192345|g99yR8QvPqITg2CK2m+NUg==|U5uRIIkxBIbkUGHCWJwkbA==|1|2|3|4|4|5|5|5|5|0|6|7|8|';

                let response = await new PostRequest('https://qldt.hust.edu.vn/soicteducation/authentication')
                    .setContentType(myEnum.contentType.GWT)
                    .setBody(body)
                    .execute();
                console.log(response);
            } catch (error){
                console.error(error);
            }
        }
    }
};
</script>