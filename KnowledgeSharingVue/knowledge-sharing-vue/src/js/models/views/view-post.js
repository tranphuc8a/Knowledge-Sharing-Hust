// Framework: .NET Core
// Technology Stack: Entity Framework, C#

import Post from '../entities/post';

class ViewPost extends Post {
    constructor() {
        super();
        this.FullName = null;
        this.Avatar = null;
        this.Cover = null;
    }

    init() {
        return new ViewPost();
    }
}

export default ViewPost;

