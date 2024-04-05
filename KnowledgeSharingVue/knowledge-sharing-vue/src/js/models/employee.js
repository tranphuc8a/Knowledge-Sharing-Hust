/* eslint-disable */

import { Human } from "./human";
import { Position } from "./position";
import { Department } from "./department";
import { Validator } from "@/js/utils/validator";
import { MyDate } from "@/js/utils/mydate";
import axios from "axios";
import { GetRequest, Request } from "../services/request";

class Employee extends Human{
    constructor(employeeApi){
        try {
            super(employeeApi);
        } catch (error){
            console.error(error);
        }
    }


    /**
     * Hàm thực hiện lấy danh sách Position từ API và đổ vào danh sách employee
     * @param {*} listEmployees 
     * @returns none
     * @Created PhucTV (28/1/24)
     * @Modified None
     */
    static async fillPosition(listEmployees){
        try {
            let response = await new GetRequest('Positions').execute();
            let listPositions = {};
            // console.log(response);
            let body = Request.tryGetBody(response);
            if (Validator.isEmpty(body)){
                return;
            }
            body.forEach(function(pos){
                listPositions[pos.PositionId] = pos.PositionName;
            });
            listEmployees.forEach(function(employee){
                employee.PositionName = listPositions[employee.PositionId];
            });
        } catch (error){
            console.error(error);
        }
    }


    /**
     * Hàm thực hiện lấy danh sách Departmenttừ API và đổ vào danh sách employee
     * @param {*} listEmployees 
     * @returns none
     * @Created PhucTV (28/1/24)
     * @Modified None
     */
    static async fillDepartment(listEmployees){
        try {
            let response = await new GetRequest('Departments').execute();
            let listDepartments = {};
            let body = Request.tryGetBody(response);
            if (Validator.isEmpty(body)) {
                return;
            }
            body.forEach(function(pos){
                listDepartments[pos.DepartmentId] = pos.DepartmentName;
            });
            listEmployees.forEach(function(employee){
                employee.DepartmentName = listDepartments[employee.DepartmentId];
            });
        } catch (error){
            console.error(error);
        }
    }

    initProperties(){
        super.initProperties();
        this.keys = [...this.keys, "EmployeeId", "EmployeeCode", "JoinDate", "MaterialStatus", "WorkStatus", "PersonalTaxCode", 
                                    "Salary", "BankAccount", "BankName", "BankBranch", 
                                    "NationalityId", "QualificationId", "PositionId", "DepartmentId",
                                    "Position", "Department", "PositionName", "DepartmentName"];
    }


    toTableObject(){
        let obj = super.toTableObject();
        // if (Validator.isNotEmpty(this.Position)){
        //     obj.tPositionName = this.Position.PositionName;
        // }
        // if (Validator.isNotEmpty(this.Department)){
        //     obj.tDepartmentName = this.Department.DepartmentName;
        // }
        return obj;
    }

    fromEntityApi(apiObj){
        super.fromEntityApi(apiObj);
        // this.Position = new Position(apiObj.Position);
        // this.Department = new Department(apiObj.Department);
    }

    formatEntityTable(tableObj){
        super.formatEntityTable(tableObj);
        tableObj.tJoinDate = this.formatDate(tableObj.tJoinDate);        
    }

    formatEntityVisual(obj){
        super.formatEntityVisual(obj);
        obj.JoinDate = this.formatDate(obj.JoinDate);
    }

    formatEntityForm(formObj){
        super.formatEntityForm(formObj);
        formObj.JoinDate = this.formatDate(formObj.JoinDate, false);
    }

    formatEntityApi(apiObj){
        super.formatEntityApi(apiObj);
        apiObj.JoinDate = this.formatDate(apiObj.JoinDate, false);
        delete apiObj.Position;
        delete apiObj.Department;
        for (let key of this.keys){
            if (Validator.isEmpty(apiObj[key])){
                delete apiObj[key];
            }
        }
    }
}

export { Employee };