

<template>
    <div class="p-post-short-card">
        <div class="card">
            <div class="p-psc-thumbnail">
                <div class="p-psc-thumbnail-image">
                    <div class="skeleton" style="width: 100%; height: 100%; padding: 16px;">
                    </div>
                </div>
            </div>

            <div class="p-psc-information">
                <div class="p-psc-infor__left">
                    <div class="p-psc-lesson-title">
                        <div class="skeleton" style="width: 150px; height: 24px;">
                        </div>
                    </div>
                    <div class="p-psc-lesson-owner">
                        <TooltipUserAvatarAndUsername :user="null" :size="32" />
                    </div>
                    <div class="p-psc-lesson-stars">
                        <div class="skeleton" style="width: 175px; height: 24px;">
                        </div>
                    </div>
                </div>
                <div class="p-psc-infor__right">
                    <div class="skeleton"
                        style="width: 24px; height: 32px; border-radius: 20%;"
                    >
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import TooltipUserAvatarAndUsername from '../avatar/TooltipUserAvatarAndUsername.vue';


export default {
    name: 'PostShortCardSkeleton',
    components: {
        TooltipUserAvatarAndUsername,
    },
}

</script>


<style scoped>

.p-post-short-card{
    width: 100%;
    max-width: 100%;
}

.p-post-short-card > .card{
    height: 120px;
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: stretch;
}

.p-psc-thumbnail{
    width: 200px;
    height: 100%;
    flex-grow: 0;
    flex-shrink: 0;
    cursor: pointer;
    border-bottom-left-radius: 8px;
    border-top-left-radius: 8px;
}

.p-psc-thumbnail-image{
    width: 100%;
    height: 100%;
    background-size: cover;
    background-position: center;
    position: relative;
    cursor: pointer;
    border-bottom-left-radius: 8px;
    border-top-left-radius: 8px;
}

.p-psc-thumbnail-overlay{
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    opacity: 0;
    transition: opacity 0.3s;
    border-bottom-left-radius: 8px;
    border-top-left-radius: 8px;
}

.p-psc-thumbnail-image:hover .p-psc-thumbnail-overlay{
    opacity: 1;
}

.p-psc-information{
    min-width: 300px;
    width: 100%;
    max-width: calc(100% - 200px);
    height: 100%;
    padding: 16px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: stretch;
    gap: 16px;
}

.p-psc-infor__left{
    width: 0;
    flex-grow: 1;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: stretch;
}

.p-psc-infor__left > *{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 8px;
}

.p-psc-lesson-title{
    width: 100%;
    height: fit-content;
    font-size: 24px;
    font-family: 'ks-font-semibold';
    cursor: pointer;
}

.p-psc-lesson-owner{
    width: 100%;
    height: fit-content;
}

.p-psc-lesson-stars{
    width: 100%;
    height: fit-content;
}

.p-psc-infor__right{
    width: fit-content;
    height: 100%;
    flex-grow: 0;
    flex-shrink: 0;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-end;
    align-items: flex-end;
    gap: 16px;
}

.p-lcs-menu-context{
    width: fit-content;
    height: fit-content;
}



</style>

