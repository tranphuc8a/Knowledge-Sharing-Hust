﻿using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories
{
    public interface ICourseRelationRepository : IRepository<CourseRelation>
    {


        /// <summary>
        /// Lấy về vai trò của user đối với một khóa học
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<ECourseRoleType> GetRole(Guid userId, Guid courseId);


        /// <summary>
        /// Lấy về danh sách quan hệ của một course 
        /// </summary>
        /// <param name="courseId"> id của course cần lấy  </param>
        /// <param name="relationType"> loại quan hệ cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<List<CourseRelation>> GetRelationsOfCourse(Guid courseId, ECourseRelationType relationType);


        /// <summary>
        /// Lấy về danh sách quan hệ voi khoa hoc của một user 
        /// </summary>
        /// <param name="userId"> id của user cần lấy  </param>
        /// <param name="relationType"> loại quan hệ cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<List<CourseRelation>> GetRelationsOfUser(Guid userId, ECourseRelationType relationType);


        /// <summary>
        /// Lay ve quan he cua user voi mot danh sach khoa hoc
        /// </summary>
        /// <param name="userId"> id cua user thuc hien </param>
        /// <param name="courseIds"> Danh sach course id </param>
        /// <returns></returns>
        /// Created: PhucTV (13/5/24)
        /// Modified: None
        Task<Dictionary<Guid, CourseRoleTypeDto>> GetCourseRoleType(Guid userId, List<Guid> courseIds);


        /// <summary>
        /// Lấy về mot course relation theo id
        /// </summary>
        /// <param name="courseRelationId"> id courseRelation cần lấy  </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<CourseRelation?> GetRelation(Guid courseRelationId);

        /// <summary>
        /// Confirm a course relation
        /// </summary>
        /// <param name="userRelationId"> id cua course relation can confirm </param>
        /// <returns></returns>
        /// Created: PhucTV (30/3/24)
        /// Modified: None
        Task<int> ConfirmCourseRelation(Guid userRelationId);

        /// <summary>
        /// Lay ve request hoac invite cu the cua khoa hoc toi user
        /// </summary>
        /// <param name="senderId"> Nguoi gui yeu cau tham gia khoa hoc </param>
        /// <param name="receiverId"> Nguoi nhan loi moi tham gia khoa hoc </param>
        /// <param name="courseId"> id cua khoa hoc </param>
        /// <returns></returns>
        Task<CourseRelation?> GetRequest(Guid senderId, Guid courseId);
        Task<CourseRelation?> GetInvite(Guid receiverId, Guid courseId);

    }
}
