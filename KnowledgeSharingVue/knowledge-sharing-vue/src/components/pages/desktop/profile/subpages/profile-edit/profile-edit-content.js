
import { Validator } from "@/js/utils/validator";

export default {
    methods: {
        async startDynamicValidate(){
            try {
                for (let key of this.keys){
                    this.components[key].startDynamicValidate?.();
                }
            } catch (error){
                console.error(error);
            }
        },

        async stopDynamicValidate(){
            try {
                for (let key of this.keys){
                    this.components[key].stopDynamicValidate?.();
                }
            } catch (error){
                console.error(error);
            }
        },

        async getValue(){
            try {
                let result = {};
                for (let key of this.keys){
                    let value = await this.components[key].getValue?.();
                    if (value != null && key == "Grade"){
                        value = String(value);
                    }
                    result[key] = value;
                }
                return result;
            } catch (error){
                console.error(error);
            }
        },

        async setValue(user){
            try {
                if (Validator.isEmpty(user)) return;
                for (let key of this.keys){
                    let value = user[key];
                    if (["Grade", "Cpa"].includes(key)){
                        if (Validator.isEmpty(value)) {
                            value = '';
                        } else {
                            value = Number(value);
                        }
                    }
                    await this.components[key].setValue?.(value);
                }
            } catch (error){
                console.error(error);
            }
        },

        async focusError(){
            try {
                for (let key of this.keys){
                    if (!await this.components[key].validate()) 
                        return await this.components[key].focus();
                }
            } catch (error){
                console.error(error);
            }
        },

        async validate(){
            try {
                for (let key of this.keys){
                    if (!await this.components[key].validate()) return false;
                }
                return true;
            } catch (error){
                console.error(error);
                return false;
            }
        }
    }
}

