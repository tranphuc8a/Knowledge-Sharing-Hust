﻿using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Infrastructures.Interfaces.DbContexts;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Repositories.Repositories.MySqlRepositories.MySqlKnowledgeRepositories
{
    public class CategoryMySqlRepository(IDbContext dbContext)
        : BaseMySqlRepository<Category>(dbContext), ICategoryRepository
    {
        public async Task<List<string>> GetByKnowledgeId(Guid knowledgeId)
        {
            var cateIds = DbContext.KnowledgeCategories
                .Where(item => item.KnowledgeId == knowledgeId)
                .Select(item => item.CategoryId)
                .Distinct();
            return await DbContext.Categories
                .Where(cate => cateIds.Contains(cate.CategoryId))
                .OrderByDescending(cate => cate.CreatedTime)
                .Select(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<Dictionary<Guid, List<string>>> GetByKnowledgeId(List<Guid> knowledgeIds)
        {
            Dictionary<Guid, List<string>> categories = await DbContext.ViewKnowledgeCategories
                .Where(knowCate => knowledgeIds.Contains(knowCate.KnowledgeId))
                .GroupBy(knowCate => knowCate.KnowledgeId)
                .ToDictionaryAsync(group => group.Key, group => group.Select(item => item.CategoryName).ToList());

            return knowledgeIds.Distinct().ToDictionary(
                id => id,
                id =>
                {
                    if (categories.TryGetValue(id, out List<string>? value))
                        return value ?? [];
                    return [];
                }
            );
        }

        public async Task<Category?> GetByName(string catName)
        {
            return await DbContext.Categories
                .Where(cate => cate.CategoryName == catName)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ViewKnowledgeCategory>> GetKnowledgesByCategory(Guid catId)
        {
            return await DbContext.ViewKnowledgeCategories
                .Where(cate => cate.CategoryId == catId)
                .OrderByDescending(cate => cate.CreatedTime)
                .ToListAsync();
        }

        public async Task<List<ViewKnowledgeCategory>> GetKnowledgesByCategory(string catName)
        {
            return await DbContext.ViewKnowledgeCategories
                .Where(cate => cate.CategoryName == catName)
                .OrderByDescending(cate => cate.CreatedTime)
                .ToListAsync();
        }

        public async Task<int> UpdateKnowledgeCategories(Guid knowledgeId, List<string> catNames)
        {
            using var transaction = await DbContext.BeginTransaction();

            try
            {
                // Xóa các cate của knowledge Id, nếu có sự thay đổi
                var existingCategories = await DbContext.KnowledgeCategories
                    .Where(item => item.KnowledgeId == knowledgeId)
                    .ToListAsync();

                if (existingCategories.Count != 0)
                    DbContext.KnowledgeCategories.RemoveRange(existingCategories);

                // Lấy danh sách CategoryName đã tồn tại trong database
                var existingCatNames = await DbContext.Categories
                    .Where(c => catNames.Contains(c.CategoryName))
                    .ToListAsync();

                var existingSet = new HashSet<string>(existingCatNames.Select(c => c.CategoryName));
                var currentTime = DateTime.UtcNow;

                // Tìm ra những CategoryName mới và thêm chúng vào database
                var newCategories = catNames.Except(existingSet).Select(cn => new Category
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryName = cn,
                    CreatedBy = "Knowledge Sharing Admin",
                    CreatedTime = currentTime
                }).ToList();

                DbContext.Categories.AddRange(newCategories);
                int res1 = await DbContext.SaveChangesAsync();

                // Thêm quan hệ KnowledgeCategory
                var allCatNamesToLink = existingCatNames.Concat(newCategories).ToList();
                var knowledgeCategoriesToInsert = allCatNamesToLink.Select(cate => new KnowledgeCategory
                {
                    KnowledgeCategoryId = Guid.NewGuid(),
                    CategoryId = cate.CategoryId,
                    KnowledgeId = knowledgeId,
                    CreatedBy = "Knowledge Sharing Admin",
                    CreatedTime = currentTime
                });

                DbContext.KnowledgeCategories.AddRange(knowledgeCategoriesToInsert);

                // Commit
                int res = res1 + await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return res + 1;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        protected override DbSet<Category> GetDbSet()
        {
            return DbContext.Categories;
        }
    }
}
