
import Course from '../entities/course';

class ViewCourse extends Course {
    constructor() {
        super();
        this.Username = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;


        this.TotalStar = null;
        this.SumStar = null;
        this.AverageStar = null;
        this.TotalComment = null;

        this.TotalLesson = null; 
        this.TotalQuestion = null; 
        this.TotalRegister = null; 
        this.TotalInvite = null; 
        this.TotalRequest = null; 
        
    }
}

export default ViewCourse;
