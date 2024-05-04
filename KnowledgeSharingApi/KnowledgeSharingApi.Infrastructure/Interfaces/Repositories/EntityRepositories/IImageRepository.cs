﻿using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Interfaces.Repositories.EntityRepositories
{
    public interface IImageRepository : IRepository<Image>
    {

        /// <summary>
        /// Lấy về danh sách ảnh của một userId
        /// </summary>
        /// <param name="userId"> id user cần lấy </param>
        /// <returns></returns>
        /// Created: PhucTV (1/5/24)
        /// Modified: None
        Task<IEnumerable<Image>> GetByUserId(Guid userId);
    }
}