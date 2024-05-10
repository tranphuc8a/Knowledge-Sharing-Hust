<template>
    <div>
        <a-switch :checked="!loading" @change="onChange" />
        <a-list item-layout="vertical" size="large" :data-source="listData">
            <template #renderItem="{ item }">
                <a-list-item key="item.title">
                    
                    
                    <a-skeleton :loading="loading" active>
                        <!-- <a-list-item-meta :description="item.description">
                            <template #title>
                                <a :href="item.href">{{ item.title }}</a>
                            </template>
                            <template #avatar><a-avatar :src="item.avatar" /></template>
                        </a-list-item-meta> -->
                        {{ item.content }}
                    </a-skeleton>

                </a-list-item>
            </template>
        </a-list>

        <div class="skeleton p2"></div>
    </div>
</template>


<script setup>
import { ref } from 'vue';

let listData = [];
for (let i = 0; i < 3; i++) {
    listData.push(
        {
            href: 'https://www.antdv.com/',
            title: `ant design vue part ${i}`,
            avatar: 'https://joeschmoe.io/api/v1/random',
            description:
            'Ant Design, a design language for background applications, is refined by Ant UED Team.',
            content:
            'We supply a series of design principles, practical patterns and high quality design resources (Sketch and Axure), to help people create their product prototypes beautifully and efficiently.',
        }
    );
}

const loading = ref(true);

const onChange = (checked) => {
    loading.value = !checked;
};
</script>

<style scoped>


.p2{
    width: 600px;
    height: 60px;
    border-radius: 8px;
    background-color: white !important;
}


</style>