

<template>
    <div class="p-container"> 

        <div>
            <MIcon fa="frown" family="far" :style="iconStyle"/>
        </div>
        <span>
            {{ text }}
        </span>

    </div>
</template>

<script>

export default {
    name: 'NotFound',
    data() {
        return {
            iconStyle: {
                fontSize: '50px',
                margin: '18px'
            }
        }
    },
    props: {
        text: {
            type: String,
            default: 'Bài giảng hiện không tồn tại hoặc đã bị xóa'
        }
    }
}


</script>


<style scoped>

.p-container{
    font-size: 16px;
    color: var(--primary-color);
    font-weight: 600;
}

</style>