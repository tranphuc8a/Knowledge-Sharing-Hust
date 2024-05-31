

<template>
    <div class="p-course-lesson-card">
        <div class="p-clc-header">
            <div class="p-clc-offset skeleton"
                style="width: 200px; height: 32px;"
            >
            </div>
            <div class="p-clc-button">
                <span>
                    <MEmbeddedButton 
                        label="Xem bài giảng"
                    />
                </span>
            </div>
        </div>
        <div class="p-clc-content">
            <LessonShortCardSkeleton />
        </div>
    </div>
</template>



<script>
import LessonShortCardSkeleton from './LessonShortCardSkeleton.vue';
import MEmbeddedButton from './../buttons/MEmbeddedButton.vue';

export default {
    name: 'CourseLessonCardSkeleton',
    components: {
        LessonShortCardSkeleton,
        MEmbeddedButton,
    },
}

</script>


<style scoped>

.p-course-lesson-card{
    max-width: 100%;
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    gap: 4px;
}

.p-clc-header{
    width: 100%;
    max-width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: center;
}

.p-clc-header > * {
    width: fit-content;
    max-width: 100%;
    flex-shrink: 0;
    flex-grow: 0;
}

.p-clc-content{
    width: 100%;
    max-width: 100%;
}

</style>

