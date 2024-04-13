
import Entity from './entity';

class Profile extends Entity{
    constructor() {
        super();
        this.ProfileId = null;
        this.UserId = null;
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
        this.Nickname = null;
        this.Bio = null;
        this.Gender = null;
        this.DateOfBirth = null;
        this.PhoneNumber = null;
        this.ContactEmail = null;
        this.Country = null;
        this.Address = null;
        this.SocialLink = null;
        this.School = null;
        this.Profession = null;
        this.Cpa = null;
        this.Grade = null;
        this.Class = null;
        this.Job = null;
    }

    init() {
        return new Profile();
    }
}

export default Profile;
