import { Validator } from "./validator";


class PaginationQueryParsing{
    /**
     * The function check if the string is null, undefined, empty or contain only white space
     * @param {*} str - The string to check
     * @returns {Boolean} - True if the string is null, undefined, empty or contain only white space, otherwise return false
     * @Created PhucTV (29/5/24)
     * @Modified None
     */
    static isNullOrEmptyOrWhiteSpace(str){
        if (str === null || str === undefined) return true;
        str = String(str);
        return str.trim() === "";
    }

    /**
     * The function to parse the pagination order query string
     * @param {*} orderString - The order string to parse, ex: "name:asc,age:desc,date,,," 
     * @returns {Array} - The array of order object, ex: [{field: "name", order: "asc"}, {field: "age", order: "desc"}]
     * @Created PhucTV (29/5/24)
     * @Modified None
     */
    static parseOrderString(orderString){
        try {
            if (!orderString) return [];
            let orderArray = orderString.split(",");
            // filter the empty string or contain only space
            orderArray = orderArray.filter(item => !this.isNullOrEmptyOrWhiteSpace(item));
            
            let result = [];
            orderArray.forEach(item => {
                let [field, order] = item.split(":");
                if (this.isNullOrEmptyOrWhiteSpace(field)) return;
                if (this.isNullOrEmptyOrWhiteSpace(order))
                    order = "asc"; // default order is "asc"
                result.push({
                    field: String(field).trim(), 
                    order: String(order).trim().toLowerCase(),
                });
            });

            return result;
        } catch (e){
            console.error(e);
        }
    }

    /**
     * The function to parse the pagination filter query string
     * @param {*} filterString - The filter string to parse, ex: "name:phuc,old,age:25:desc,date,privacy:private:ne,,,"
     * @returns {Array} - The array of filter object, ex: [{field: "name", value: "phuc", operator: "eq"}, {field: "age", value: 25, operator: "desc"}, {field: "privacy", value: "private", operator: "ne"}]
     * @Created PhucTV (29/5/24)
     * @Modified None
     */
    static parseFilterString(filterString){
        try {
            if (!filterString) return [];
            let filterArray = filterString.split(",");
            // filter the empty string or contain only space
            filterArray = filterArray.filter(item => !this.isNullOrEmptyOrWhiteSpace(item));
            
            let result = [];
            filterArray.forEach(item => {
                let [field, value, operator] = item.split(":");
                if (this.isNullOrEmptyOrWhiteSpace(field)) return;
                if (this.isNullOrEmptyOrWhiteSpace(value)) return;
                if (this.isNullOrEmptyOrWhiteSpace(operator))
                    operator = "eq"; // default operator is "eq"
                result.push({
                    field: String(field).trim(), 
                    value: value, 
                    operator: String(operator).trim().toLowerCase(),
                });
            });

            return result;
        } catch (e){
            console.error(e);
        }
    }
    
}


export default PaginationQueryParsing;

