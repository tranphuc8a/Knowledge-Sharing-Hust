<template>
    <div>
        <MCheckbox 
            label="Giới tính" title="Chọn một giới tính phù hợp"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :items="getCheckboxItems()" direction="row" group="gender"
            />
        <MCombobox 
            label="Chọn tỉnh" title="Chọn một tỉnh của VN" placeholder="--Chọn 1 tỉnh--"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :listItemLoader="getComboboxItems" :isDynamicValidate="true"
            state="normal"
        /> 
        <MCombobox 
            label="Chọn tỉnh" title="Chọn một tỉnh của VN" placeholder="--Chọn 1 tỉnh--"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :listItemLoader="getComboboxItems" :isDynamicValidate="true"
            state="validated"
        />  
        <MCombobox 
            label="Chọn tỉnh" title="Chọn một tỉnh của VN" placeholder="--Chọn 1 tỉnh--"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :listItemLoader="getComboboxItems" :isDynamicValidate="true"
            state="error"
        />  
        <MSelectOption 
            label="Chọn tỉnh" title="Chọn một tỉnh của VN" placeholder="--Chọn 1 tỉnh--"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :listItemLoader="getComboboxItems" :isDynamicValidate="true"
            state="normal"
        /> 
        <MSelectOption 
            label="Chọn tỉnh" title="Chọn một tỉnh của VN" placeholder="--Chọn 1 tỉnh--"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :listItemLoader="getComboboxItems" :isDynamicValidate="true"
            state="validated"
        />  
        <MSelectOption 
            label="Chọn tỉnh" title="Chọn một tỉnh của VN" placeholder="--Chọn 1 tỉnh--"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :listItemLoader="getComboboxItems" :isDynamicValidate="true"
            state="error"
        />   
        <MDatePicker 
            label="Ngày sinh" title="Chọn ngày sinh nhật của bạn" placeholder="<--dd/mm/yy-->"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
        />
        <MRadio 
            label="Giới tính" title="Chọn một giới tính phù hợp"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :items="getCheckboxItems()" direction="row" group="gender-2"
        />
        <MTextfield 
            label="Nhập TTK" title="Nhập tên tài khoản" placeholder="Tên tài khoản" autocomplete="email"
            :is-show-icon="true" :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :onclick-icon="()=>{}" 
            :validator="null"
            state="normal"/>
        <MTextfield 
            label="Nhập TTK" title="Nhập tên tài khoản" placeholder="Tên tài khoản" autocomplete="email"
            :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :onclick-icon="()=>{}" 
            :validator="null" value="tranphuc8a9a@gmail.com"
            state="read-only"/>
        <MTextfield 
            label="Nhập TTK" title="Nhập tên tài khoản" placeholder="Tên tài khoản" autocomplete="email"
            :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :onclick-icon="()=>{}" 
            :validator="null" value="tranphuc8a9a@gmail.com"
            state="validating"/>
        <MTextfield 
            label="Nhập TTK" title="Nhập tên tài khoản" placeholder="Tên tài khoản" autocomplete="email"
            :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :onclick-icon="()=>{}" 
            :validator="null" value="tranphuc8a9a@gmail.com"
            state="validated"/>
        <MTextfield 
            label="Nhập TTK" title="Nhập tên tài khoản" placeholder="Tên tài khoản" autocomplete="email"
            :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :onclick-icon="()=>{}" 
            :validator="null" value="tranphuc8a9a@gmail.com" error-message="Tên tài khoản không hợp lệ"
            state="error"/>
        <MPasswordTextfield 
            label="Nhập MK" title="Nhập mật khẩu tương ứng" placeholder="Mật khẩu của bạn" autocomplete="password"
            :is-show-icon="false" :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :onclick-icon="()=>{}" 
            :validator="null" value="tranphuc8a9a@gmail.com" error-message="Tên tài khoản không hợp lệ"
            state="error"/>
        
        <CategoryInput 
            label="Chọn danh mục" 
            title="Chọn một danh mục phù hợp" 
            placeholder="--Chọn 1 danh mục--" 
            :validator="null"
        />

        <MTextfieldButton 
            title="Nhập từ khóa tìm kiếm" placeholder="Tìm kiếm"
            :is-show-icon="true" 
            :is-show-title="false" 
            :is-show-error="false" 
            :is-obligate="false"
            :onclick-icon="resolveClickSearch" 
            :validator="null"
            :oninput="resolveInputSearch"
            state="normal"
        />

        <MImageInput 
            :style="{ width: '200px', height: '200px' }"
            label="Chọn ảnh" title="Chọn một ảnh phù hợp"
            :is-show-title="true" :is-show-error="true" :is-obligate="true"
            :validator="null"
            state="normal"
        />

        <MTextArea
            ref="textarea"
            :placeholder="placeholder"
            :is-show-title="false" :is-show-error="true" 
            :validator="commentValidator"
            max-height="150px" rows="3"
            />
    </div>
</template>


<script>
import MCheckbox from '@/components/base/inputs/MCheckbox';
import MCombobox from '@/components/base/inputs/MCombobox';
import MDatePicker from '@/components/base/inputs/MDatePicker';
import MRadio from '@/components/base/inputs/MRadio';
import MTextfield from '@/components/base/inputs/MTextfield';
import MPasswordTextfield from '@/components/base/inputs/MPasswordTextfield.vue';
import MSelectOption from './MSelectOption.vue';
import CategoryInput from '../category/CategoryInput.vue';
import MTextfieldButton from './MTextfieldButton.vue';
import MImageInput from './MImageInput.vue';
import MTextArea from './MTextArea.vue';


export default {
    name: 'TestInputsPage',
    components: { 
        MCheckbox, MCombobox, MDatePicker, MRadio, MTextfield,
        MPasswordTextfield, MSelectOption, CategoryInput,
        MTextfieldButton, MImageInput, MTextArea
    },
    methods: {
        async getComboboxItems(){
            await new Promise(e => setTimeout(e, 1000));
            return Array.from([{
                    label: 'Hưng Yên',
                    value: 1
                }, {
                    label: 'Hà Nội',
                    value: 2
                }, {
                    label: 'Nam Định',
                    value: 3
                }, ]);
        },
        getCheckboxItems(){
            return [{
                label: "Nam",
                value: 1
            }, {
                label: "Nữ",
                value: 2
            }, {
                label: "Khác",
                value: 0
            }, {
                label: "LGBT",
                value: -1
            }]
        }
    }
}
</script>
