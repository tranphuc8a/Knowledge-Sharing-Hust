

<template>

    <div class="p-profile-learn-toolbar-subpage">
        <div class="p-plts-card card">
            <div class="p-plts-card__header">
                <div class="p-plts-header">
                    Các khóa học bạn đang học
                </div>
                <div class="p-plts-button">
                    <MSecondaryButton 
                        label="Khám phá các khóa học khác"
                        :onclick="resolveClickOtherCourse"
                        :buttonStyle="buttonStyle"
                    />
                </div>
            </div>
            <div class="p-plts-card__descrition">
                {{ description }}
            </div>
            <div class="p-plts-card__control">
                <div class="p-control-textfield">
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
                        ref="search"
                    />
            
                </div>
                <div class="p-control-order">
                    <MSelectOption 
                        placeholder="-- Chọn thứ tự --"
                        :is-show-title="false" :is-show-error="false" :is-obligate="false"
                        :listItemLoader="getOptions"
                        state="normal"
                        :onchange="resolveChangeOrder"
                        ref="option"
                    /> 
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import MSecondaryButton from './../../../../../base/buttons/MSecondaryButton.vue'
import MTextfieldButton from './../../../../../base/inputs/MTextfieldButton.vue'
import MSelectOption from './../../../../../base/inputs/MSelectOption.vue'
import Debounce from '@/js/utils/debounce'

export default {
    name: 'ProfileLearnToolbarSubpage',
    components: {
        MSecondaryButton,
        MTextfieldButton,
        MSelectOption
    },
    props: {
        onChangeConfig: {},
        listCourses: {}
    },
    data(){
        return {
            buttonStyle: {

            },
            description: 'Bạn đang học 2 khóa học. Bạn có thể khám phá các khóa học khác tại đây.',
            debouncedFunction: null
        }
    },
    mounted(){
        this.refresh();
    },
    methods: {

        async refresh(){
            try {
                let numberCourse = this.listCourses?.length ?? 0;
                if (numberCourse > 0){
                    this.description = `Bạn đang học ${numberCourse} khóa học. Bạn có thể khám phá các khóa học khác tại đây.`;
                } else {
                    this.description = 'Bạn chưa học khóa học nào. Bạn có thể khám phá các khóa học khác tại đây.';
                }
                this.debouncedFunction = Debounce.throttle(this.resolveChangeConfig.bind(this), 1000);
                
            } catch (error){
                console.error(error);
            }
        },

        async resolveClickOtherCourse(){
            try {
                await this.$store.dispatch('course/getOtherCourse')
                this.$router.push({
                    name: 'course.other-course'
                })
            } catch (error){
                console.error(error)
            }
        },

        getOptions(){
            return [
                {
                    label: 'Mới nhất',
                    value: 'newest'
                },
                {
                    label: 'Cũ nhất',
                    value: 'oldest'
                }
            ];
        },

        async resolveChangeConfig(){
            try {
                if (this.onChangeConfig){
                    let searchText = await this.$refs.search.getValue();
                    let order = await this.$refs.option.getValue();
                    let config = {
                        text: searchText,
                        isLatest: (order === null ? null : order === 'newest' ? true : false)
                    }
                    await this.onChangeConfig(config);
                }
            } catch (e) {
                console.error(e);
            }
        },

        async resolveInputSearch(){
            return this.debouncedFunction?.();
        },

        async resolveChangeOrder(){
            return this.debouncedFunction?.();
        },

        async resolveClickSearch(){
            return this.debouncedFunction?.();
        }
    },
    watch: {
        listCourses: {
            handler: function(){
                this.refresh();
            },
            deep: true
        }
    }
}

</script>

<style scoped>

.p-profile-learn-toolbar-subpage{
    width: 100%;
}

.p-plts-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
}

.p-plts-card__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: flex-start;
    width: 100%;
    margin-bottom: 16px;
}
.p-plts-header{
    font-size: 24px;
    font-weight: 600;
    font-family: 'ks-font-semibold';
}
.p-plts-button{
    width: fit-content;
}
.p-plts-card__descrition{
    font-size: 16px;
    margin-bottom: 16px;
}

.p-plts-card__control{
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    width: 100%;
    gap: 16px;
}

.p-control-order{
    width: 200px;
}

</style>

