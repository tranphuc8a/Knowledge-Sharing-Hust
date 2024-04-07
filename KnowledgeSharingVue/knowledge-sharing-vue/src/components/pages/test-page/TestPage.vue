<template>
    <div :style="{ width: '320px', display: 'flex', flexFlow: 'column', gap: '4px'}">
        <Button label="Thêm mới" />
        <Button fa="user-plus"/>
        <Button fa="user-plus" label="Thêm mới người dùng" state="disabled" />

        <CancelButton label="Hủy bỏ" />
        <CancelButton fa="user-minus"/>
        <CancelButton fa="user-minus" label="Thêm mới người " state="disabled" />

        <DeleteButton label="Xóa" />
        <DeleteButton fa="trash-can"/>
        <DeleteButton fa="trash-can" label="Xóa bỏ người dùng" state="disabled" />

        <LinkButton label="Facebook" href="https://fb.com"/>
        <LinkButton label="Youtube" href="https://youtube.com" state="disabled" />

        <ComboButton label="Facebook" fa="align-center"/>
        <ComboButton label="Youtube" fa="align-left" state="disabled" />
    </div>
</template>


<script>
import Button from '@/components/base/buttons/MButton.vue';
import CancelButton from '@/components/base/buttons/MCancelButton';
import DeleteButton from '@/components/base/buttons/MDeleteButton';
import LinkButton from '@/components/base/buttons/MLinkButton';
import ComboButton from '@/components/base/buttons/MComboButton';

export default {
    name: 'KSTestPage',
    components: { Button, CancelButton, DeleteButton, LinkButton, ComboButton },
    data(){
        return {
        }
    },
    mounted(){
    },
    methods: {
        
    },
    props: {

    },
    provide(){
        return {} 
    }
}
</script>


<style>
</style>
