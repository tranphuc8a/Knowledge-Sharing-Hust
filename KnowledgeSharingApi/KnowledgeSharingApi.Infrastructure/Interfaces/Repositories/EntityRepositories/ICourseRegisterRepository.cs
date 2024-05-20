using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface ICourseRegisterRepository : IRepository<CourseRegister>
    {
        /// <summary>
        /// Lấy về danh sách user là thành viên của một khóa học
        /// </summary>
        /// <param name="courseId"> id của khóa học cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<List<ViewCourseRegister>> GetCourseRegisters(Guid courseId);


        /// <summary>
        /// Lấy về thành viên của khóa học thông qua mã thành viên
        /// </summary>
        /// <param name="registerId"> id của mã thành viên cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modfied: None
        Task<ViewCourseRegister?> GetCourseRegister(Guid registerId);
    }
}
