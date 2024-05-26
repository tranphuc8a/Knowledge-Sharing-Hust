

<template>
    <div class="p-course-credit-fee-content">
        <div class="p-ccf-card card">
            <div class="p-ccf-header">
                <div class="card-heading">
                    Chi Phí Khóa Học - Định Giá Kiệt Tác Của Bạn!
                </div>
                <div class="card-description">
                    Đến phần quan trọng rồi đây! Xác định chi phí cho khóa học của bạn để các sinh viên khác biết được họ cần đầu tư bao nhiêu. 
                    Bạn có thể chọn giữa việc hào phóng cung cấp khóa học miễn phí hoặc kiếm thêm chút "khoản tiêu vặt" từ công sức của mình. 
                </div>
            </div>
            
            <div class="p-ccf-form">
                <div class="p-input-item">
                    <div class="p-input-heading card-subheading">
                        Học phí khóa học
                    </div>
                    <div class="p-input-description card-description">
                        Bạn muốn khóa học của mình miễn phí hay có phí? Nếu miễn phí, hãy chọn "Miễn phí" và thể hiện "lòng nhân ái" của mình. 
                        Nếu có phí, chọn "Có phí" và nhập số tiền (đơn vị VNĐ). <br />
                        Hãy cân nhắc giữa giá trị khóa học và khả năng chi trả của các bạn sinh viên để tìm ra con số hợp lý. 
                    </div>
                    <div class="p-ccg-input-content p-input-fee">
                        <div class="p-radio-frame">
                            <MRadio
                                :is-show-title="false" :is-show-error="false" :is-obligate="false"
                                :items="isFreeItems" direction="column" group="isfree-2"
                                ref="IsFree" :validator="null"
                                :onchange="onIsFreeChange" :value="true"
                            />
                        </div>
                        <div class="p-textfield-frame" :style="{
                            visibility: isFree ? 'hidden' : 'visible'
                        }">
                            <MTextfield 
                                placeholder="Nhập học phí khóa học" type="number"
                                :is-show-icon="false" :is-show-title="false" :is-show-error="true" :is-obligate="false"
                                :validator="validators.Fee" :value="0"
                                state="normal" ref="Fee"
                            />
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
</template>



<script>
import { RangeNumberValidator, Validator } from '@/js/utils/validator';
import MTextfield from './../../../../../base/inputs/MTextfield.vue';
import MRadio from './../../../../../base/inputs/MRadio.vue'
import courseCreditContent from './course-credit-content';

export default {
    name: 'CourseCreditfeetContent',
    components: {
        MTextfield, MRadio
    },
    props: {
    },
    data(){
        return {
            keys: ["Fee"],
            validators: {
                Fee: new RangeNumberValidator("Học phí không hợp lệ").setBoundary(0, null)
            },
            components: {
            },
            isFreeItems: [
                { value: true, label: "Miễn phí" },
                { value: false, label: "Có phí" }
            ],
            isFree: true
        }
    },
    async mounted(){
        try {
            this.refresh();
        } catch (error){
            console.error(error);
        }
    },
    methods: {
        ...courseCreditContent.methods,

        async getValue(){
            try {
                let isFree = await this.components.IsFree.getValue();
                let fee = await this.components.Fee.getValue();
                if (isFree) return { Fee: 0, };
                return { Fee: fee ?? 0 };
            } catch (error){
                console.error(error);
            }
        },

        async setValue(obj){
            try {
                if (Validator.isEmpty(obj?.Fee) || obj.Fee == 0){
                    await this.components.IsFree.setValue(true);
                    await this.components.Fee.setValue(0);
                    this.isFree = true;
                } else {
                    await this.components.IsFree.setValue(false);
                    await this.components.Fee.setValue(Number(obj.Fee));
                    this.isFree = false;
                }
            } catch (error){
                console.error(error);
            }
        },
        
        async onIsFreeChange(value){
            try {
                this.isFree = value;
            } catch (error){
                console.error(error);
            }
        },

        async refresh(){
            try {
                this.components = {
                    IsFree: this.$refs.IsFree,
                    Fee: this.$refs.Fee,
                }
            } catch (error){
                console.error(error);
            }
        }, 

    },
    inject: {
    },
    provide(){
        return{}
    }
}

</script>


<style scoped>

.p-course-credit-information-content{
    width: 100%;
}

.p-ccf-card{
    width: 100%;
    padding: 16px;
    padding-bottom: 96px;

    display: flex;
    flex-flow: column nowrap;
    align-items: stretch;
    justify-content: flex-start;

    gap: 64px;
}

.p-ccf-card > * {
    text-align: left;
}

.p-ccf-header{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;
    gap: 16px;
}


.p-ccf-form{
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 64px;
}

.p-ccf-form .p-input-item{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    align-items: flex-start;
    justify-content: flex-start;

    gap: 8px;
}

.p-ccg-input-content{
    width: 50%;
}

.p-ccg-input-content.p-input-fee{
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    gap: 16px;
}

.p-ccg-input-content.p-input-fee > * {
    width: 100%;
}

</style>

