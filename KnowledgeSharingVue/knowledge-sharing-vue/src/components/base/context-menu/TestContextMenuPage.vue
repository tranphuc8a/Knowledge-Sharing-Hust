<template>
    <div :style="{ width: '320px', display: 'flex', flexFlow: 'column', gap: '4px'}">
        <MContextMenu>
            <MIcon fa="book" />
        </MContextMenu>
    </div>
</template>


<script>
import MContextMenu from './MContextMenu.vue';
export default {
    name: 'TestContextMenuPage',
    components: { MContextMenu },
}
</script>
