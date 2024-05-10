

<template>
    <div class="p-profile-subpage">
        <div class="pps-resume-card card">
            <div class="pps-resume-label">
                Giới thiệu
            </div>
            <ResumeBio />
            <div class="pps-resume-info">
                <div class="pps-resume-info-item"
                    v-for="(item, index) in filteredItems"
                    :key="item.key ?? index"
                >
                    <div class="pps-item-icon">
                        <MIcon :fa="item.icon" />
                    </div>
                    <div class="pps-item-present" :title="item.present">
                        <a :href="item.present" 
                            v-if="item.key == 'SocialLink'">
                            Mạng xã hội
                        </a>
                        <span v-else>
                            {{ item.present }}
                        </span>
                    </div>
                </div>
            </div>
            <MSecondaryButton 
                label="Chỉnh sửa chi tiết"
                :onclick="resolveEditResume"
                v-show="isMySelf"
            />
        </div>
    </div>
</template>



<script>
import ResumeBio from '../../components/resume-subpage/ResumeBio.vue';
import { myEnum } from '@/js/resources/enum';
import { MyDate } from '@/js/utils/mydate';
import { Validator } from '@/js/utils/validator';
import MSecondaryButton from '@/components/base/buttons/MSecondaryButton';
import CurrentUser from '@/js/models/entities/current-user';
import { useRouter } from 'vue-router';

export default {
    name: 'ProfileHomeResumeSubpage',
    components: {
        ResumeBio,
        MSecondaryButton,
    },
    props: {
    },
    data(){
        return {
            title: '',
            isLessonExisted: false,
            lesson: null,
            keys: [],
            items: {},
            filteredItems: [],
            buttonStyle: {

            },
            isMySelf: false,
            currentUser: null,
            router: useRouter(),
        }
    },
    mounted(){
        this.initItems();
        this.refresh();
    },
    methods: {
        initItems(){
            try {
                this.keys = [
                    "Username", "Role", "Nickname", "Gender", "DateOfBirth", "PhoneNumber",
                    "ContactEmail", "Country", "Address", "SocialLink", "School", "Profession", 
                    "Cpa", "Grade", "Class", "Job"
                ];
                let listIcons = [
                    "user", "users-cog", "signature", "venus-mars", "birthday-cake", "phone", 
                    "envelope", "house-user", "map-marker-alt", "link", "school", "chalkboard-teacher",
                    "copyright", "user-graduate", "users", "user-md"
                ];
                let listPresentsCallback = [
                    // username:
                    (value) => `@${value}`,
                    // role:
                    (value) => {
                        if (value == myEnum.EUserRole.Admin)
                            return "Quản trị viên";
                        return "Người dùng hệ thống";
                    },
                    // nickname:
                    (value) => `Biệt danh ${value}`,
                    // gender:
                    (value) => {
                        if (value == myEnum.EGender.Male)
                            return "Nam";
                        if (value == myEnum.EGender.Female)
                            return "Nữ";
                        return "Khác";
                    },
                    // date of birth:
                    (value) => new MyDate(value).toFullyDate(),
                    // phone number:
                    (value) => value,
                    // contact email:
                    (value) => value,
                    // country:
                    (value) => `Đến từ ${value}`,
                    // address:
                    (value) => `Sống tại ${value}`,
                    // social link:
                    (value) => value,
                    // school:
                    (value) => `Học tại ${value}`,
                    // profession:
                    (value) => `Chuyên ngành ${value}`,
                    // cpa:
                    (value) => `CPA ${value}`,
                    // grade:
                    (value) => `Khóa K${value}`,
                    // class:
                    (value) => `Lớp ${value}`,
                    // job:
                    (value) => `Làm việc tại ${value}`,
                ];
                let totalItems = 16;
                for (let i = 0; i < totalItems; i++){
                    this.items[this.keys[i]] = {
                        key: this.keys[i],
                        icon: listIcons[i],
                        present: listPresentsCallback[i]
                    }
                }
            } catch (e) {
                console.error(e);
            }
        },

        async filterItems(){
            try {
                this.filteredItems = [];
                for (let key of this.keys){
                    let value = this.getUser()?.[key];
                    if (Validator.isNotEmpty(value)){
                        if (key == "Cpa" && ! (value > 0)) continue;
                        this.filteredItems.push({
                            key: key,
                            icon: this.items[key].icon,
                            present: this.items[key].present(value)
                        });
                    }
                }
                this.currentUser = await CurrentUser.getInstance();
                if (this.currentUser != null){
                    this.isMySelf = this.currentUser.UserId == this.getUser()?.UserId;
                }
            } catch (e) {
                console.error(e);
            }
        },

        async refresh(){
            try {
                await this.filterItems();
            } catch (e) {
                console.error(e);
            }
        },

        async resolveEditResume(){
            try {
                this.currentUser = await CurrentUser.getInstance();
                let username = this.currentUser?.Username;
                if (username == null){
                    this.getPopupManager().requiredLogin();
                    return;
                }
                this.router.push(`/profile/${username}/profile-edit`);
            } catch (e) {
                console.error(e);
            }
        }
    },
    inject: {
        getUser: {},
        getPopupManager: {},
    }
}

</script>

<style>
.p-profile-subpage{
    width: 100%;
}
</style>

<style scoped>
.pps-resume-card{
    padding: 16px;
    gap: 16px;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
}
.pps-resume-label{
    width: 100%;
    text-align: left;
    font-family: 'ks-font-semibold';
    font-size: 24px;
}

.pps-resume-info{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 12px;
}

.pps-resume-info-item{
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    gap: 16px;
    line-height: 24px;
}

.pps-item-icon{
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    align-items: center;
}

.pps-item-icon,
.pps-item-icon svg {
    width: 24px;
}

.pps-item-present{
    text-decoration: none;
    display: block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    font-family: 'ks-font-regular';
}

.pps-item-present a {
    text-decoration: none;
}

</style>

