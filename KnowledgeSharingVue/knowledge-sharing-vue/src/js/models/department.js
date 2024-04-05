/* eslint-disable */
import { Entity } from "./entity";

class Department extends Entity{

    constructor(apiObj){
        super(apiObj)
    }

   
    initProperties(){
        super.initProperties();
        this.keys = [...this.keys, "DepartmentId", "DepartmentCode", "DepartmentName", "Description"];
    }

}

export { Department };