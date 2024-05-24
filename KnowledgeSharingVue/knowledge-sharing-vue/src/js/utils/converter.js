
import { myEnum } from "../resources/enum";

class Converter {
    /**
     * Convert a course relation type into a course role type
     * @param {myEnum.courseRelationType} courseRelationType - value of the course relation type need to be converted
     * @returns {myEnum.courseRoleType} the course role type
     * @Created PhucTV (25/5/24)
     * @Modified None
     */
    static CourseRelationTypeToCourseRoleType(courseRelationType){
        switch(courseRelationType){
            case myEnum.ECourseRelationType.Invite:
                return myEnum.ECourseRoleType.Invited;
            case myEnum.ECourseRelationType.Request:
                return myEnum.ECourseRoleType.Requesting;
            default:
                return null;
        }
    }

    /**
     * Convert a course role type into a course relation type
     * @param {myEnum.courseRoleType} courseRoleType - value of the course role type need to be converted
     * @returns {myEnum.courseRelationType} the course relation type
     * @Created PhucTV (25/5/24)
     * @Modified None
     */
    static CourseRoleTypeToCourseRelationType(courseRoleType){
        switch(courseRoleType){
            case myEnum.ECourseRoleType.Invited:
                return myEnum.ECourseRelationType.Invite;
            case myEnum.ECourseRoleType.Requesting:
                return myEnum.ECourseRelationType.Request;
            default:
                return null;
        }
    }
}


export default Converter;
