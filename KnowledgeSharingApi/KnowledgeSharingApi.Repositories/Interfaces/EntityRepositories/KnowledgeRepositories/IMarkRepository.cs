﻿using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories
{
    public interface IMarkRepository : IRepository<Mark>
    {
        /// <summary>
        /// Kiểm tra user đã mark danh sách knowledges hay chưa
        /// </summary>
        /// <param name="userId"> id của user thực hiện </param>
        /// <param name="knowledgeId"> danh sách id của knowledge cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (03/05/24)
        /// Modified: None
        Task<Dictionary<Guid, bool>> GetUserMarkListKnowledge(Guid userId, List<Guid> knowledgeId);
    }
}
