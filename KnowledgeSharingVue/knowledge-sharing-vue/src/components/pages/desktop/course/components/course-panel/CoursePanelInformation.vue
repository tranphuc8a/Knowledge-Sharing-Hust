

<template>
    <div class="p-course-information" v-if="!isLoaded">
        <div class="p-course-information__left">
            
            <div class="p-course-infor">
                <div class="p-course-title">
                    <div class="skeleton"
                        style="width: 448px; height: 32px;"
                    >
                    </div>
                </div>
                <div class="p-course-user">
                    <div class="p-course-owner">
                        <div class="skeleton"
                            style="width: 36px; height: 36px; border-radius: 50%;"
                        >
                        </div>
                        <div class="skeleton"
                            style="width: 96px; height: 24px;"
                        >
                        </div>
                    </div>
                    <div class="p-course-member">
                        <!-- Visualized Privacy of course -->
                        <div class="p-course-privacy skeleton"
                            style="width: 150px; height: 24px;"
                        >
                        </div>
                        
                        <!-- CoursePanelMember -->
                        <CoursePanelMember />
                    </div>
                </div>
            </div>
        </div>
        
        <div class="p-course-information__right">
            <!-- CoursePanelButtons -->
            <CoursePanelButton />
        </div>
    </div>

    <div class="p-course-information" v-if="isLoaded">
        <div class="p-course-information__left">
            
            <div class="p-course-infor">
                <div class="p-course-title">
                    <EllipisText 
                        :text="dCourse?.Title"  
                        :max-line="2"
                        :style="titleStyle"
                    />
                </div>
                <div class="p-course-user">
                    <div class="p-course-owner">
                        <TooltipUserAvatar :user="owner" :size="36" />
                        <TooltipUsername :user="owner" />
                    </div>
                    <div class="p-course-member">
                        <!-- Visualized Privacy of course -->
                        <div class="p-course-privacy">
                            <VisualizedPrivacy :privacy="dCourse?.Privacy" :iconStyle="{fontSize: '16px'}"/>
                            <span> {{ privacyText }}</span>
                        </div>

                        <MIcon fa="circle" :style="dotIconStyle"/>
                        
                        <!-- CoursePanelMember -->
                        <CoursePanelMember />
                    </div>
                </div>
            </div>
        </div>
        
        <div class="p-course-information__right">
            <!-- CoursePanelButtons -->
            <CoursePanelButton />
        </div>
    </div>
</template>



<script>
import EllipisText from '@/components/base/text/EllipisText.vue';
import VisualizedPrivacy from '@/components/base/visualized-components/VisualizedPrivacy.vue';
import TooltipUserAvatar from '@/components/base/avatar/TooltipUserAvatar.vue';
import TooltipUsername from '@/components/base/avatar/TooltipUsername.vue';
import CoursePanelMember from './CoursePanelMember.vue';
import CoursePanelButton from './CoursePanelButton';
import { myEnum } from '@/js/resources/enum';
// import CoursePanelThumbnail from './CoursePanelThumbnail.vue';

export default {
    name: 'CoursePanelInformation',
    components: {
        EllipisText,
        TooltipUserAvatar,
        TooltipUsername,
        CoursePanelMember,
        CoursePanelButton,
        VisualizedPrivacy,
        // CoursePanelThumbnail,
    },
    props: {
    },
    data(){
        return {
            dCourse: this.getCourse(),
            owner: null,
            titleStyle: {
                fontSize: '28px',
                fontFamily: "'ks-font-bold'",
            },
            isLoaded: false,
            privacyText: '',
            dotIconStyle: {fontSize: '4.5px', color: 'var(--grey-color-600)'}
        }
    },
    mounted(){
        try {
            this.dCourse = this.getCourse() ?? {};
            this.owner = this.dCourse?.getUser?.();
            this.isLoaded = this.getCourse() != null;
            let privacy = this.dCourse?.Privacy;
            if (privacy == myEnum.EPrivacy.Private){
                this.privacyText = 'Khóa học riêng tư';
            } else {
                this.privacyText = 'Khóa học công khai';
            }
        } catch (e) {
            console.error(e);
        }
    },
    methods: {
    },
    inject: {
        getCourse: {},
    }
}

</script>

<style scoped>
.p-course-information{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: flex-end;
    margin-top: 8px;
    height: 132px;
}

.p-course-information > * {
    height: 100%;
}

.p-course-information__left{
    width: 0;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: flex-end;
    height: 100%;
    flex-grow: 7;
    flex-shrink: 0;
    gap: 24px;
}

.p-course-information__right{
    width: 0;
    flex-grow: 3;
    flex-shrink: 0;
}

.p-course-infor{
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: flex-start;
    height: auto;
    align-self: stretch;
    gap: 8px;
}

.p-course-infor > *{
    flex-grow: 1;
    flex-shrink: 0;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
}

.p-course-information__left > *,
.p-course-information > *{
    flex-shrink: 0;
}

.p-course-title{
    font-size: 28px;
    font-family: 'ks-font-bold';
    color: #000000;
    max-width: 100%;

    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
}

.p-course-user{
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
    align-items: flex-start;
    height: auto;
    align-self: stretch;
    gap: 8px;
}

.p-course-user > *{
    flex-grow: 1;
    flex-shrink: 0;
    display: flex;
    flex-flow: column nowrap;
    justify-content: center;
}

.p-course-owner{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    height: auto;
    align-self: stretch;
    gap: 8px;
}

.p-course-member{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    height: auto;
    align-self: stretch;
    gap: 8px;
}

.p-course-privacy{
    flex-shrink: 0;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    height: auto;
    gap: 8px;
    font-size: 14.5px;
    color: var(--grey-color-600);
    font-family: 'ks-font-semibold';
}

.p-course-privacy > * {
    flex-shrink: 0;
    flex-grow: 0;
}
</style>


