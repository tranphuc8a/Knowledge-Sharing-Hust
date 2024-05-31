

<template>

    <div class="p-profile-course-toolbar-subpage">
        <div class="p-pcts-card card">
            <div class="p-pcts-card__header">
                <div class="p-pcts-header">
                    Các khóa học của {{ isMySelf ? 'bạn' : 'người dùng'}}
                </div>
                <div class="p-pcts-button">
                    <MButton v-show="isMySelf"
                        label="Tạo khóa học mới"
                        :onclick="resolveClickCreateCourse"
                        :buttonStyle="buttonStyle"
                    />
                </div>
            </div>
            <div class="p-pcts-card__descrition">
                {{ description }}
            </div>
            <div class="p-pcts-card__control">
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
import MButton from './../../../../../base/buttons/MButton.vue'
import MTextfieldButton from './../../../../../base/inputs/MTextfieldButton.vue'
import MSelectOption from './../../../../../base/inputs/MSelectOption.vue'
import Debounce from '@/js/utils/debounce'
import { useRouter } from 'vue-router'

export default {
    name: 'ProfileCourseToolbarSubpage',
    components: {
        MButton,
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
            description: 'Bạn đang có 2 khóa học. Bạn có thể tạo hoặc khám phá các khóa học khác tại đây.',
            debouncedFunction: null,
            router: useRouter(),
            isMySelf: false,
        }
    },
    mounted(){
        this.refresh();
    },
    inject: {
        getIsMySelf: {},
    },
    methods: {

        async refresh(){
            try {
                this.isMySelf = await this.getIsMySelf();
                let numberCourse = this.listCourses?.length ?? 0;
                if (numberCourse > 0){
                    if (this.isMySelf){
                        this.description = `Bạn đang có ${numberCourse} khóa học. Bạn có thể tạo một khóa học khác tại đây.`;
                    } else {
                        this.description = `Có ${numberCourse} khóa học.`;
                    }
                } else {
                    if (this.isMySelf){
                        this.description = 'Bạn chưa có khóa học nào. Hãy tạo một khóa học mới.';
                    } else {
                        this.description = 'Chưa có khóa học nào.';
                    }
                }
                this.debouncedFunction = Debounce.throttle(this.resolveChangeConfig.bind(this), 1000);
                
            } catch (error){
                console.error(error);
            }
        },

        async resolveClickCreateCourse(){
            try {
                this.router.push('/course-create/');
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

.p-profile-course-toolbar-subpage{
    width: 100%;
}

.p-pcts-card{
    width: 100%;
    padding: 16px;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
}

.p-pcts-card__header{
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: flex-start;
    width: 100%;
    margin-bottom: 16px;
}
.p-pcts-header{
    font-size: 24px;
    font-weight: 600;
    font-family: 'ks-font-semibold';
}
.p-pcts-button{
    width: fit-content;
}
.p-pcts-card__descrition{
    font-size: 16px;
    margin-bottom: 16px;
}

.p-pcts-card__control{
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

