using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.ApiResponseModels;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities.Tables;
using KnowledgeSharingApi.Domains.Models.Entities.Views;
using KnowledgeSharingApi.Repositories.Interfaces.DecorationRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.KnowledgeRepositories;
using KnowledgeSharingApi.Repositories.Interfaces.EntityRepositories.UserIteractionRepositories;
using KnowledgeSharingApi.Services.Interfaces.Knowledges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Services.Knowledges
{
    public class KnowledgeService : IKnowledgeService
    {
        protected readonly IResourceFactory ResourceFactory;
        protected readonly IResponseResource ResponseResource;
        protected readonly IEntityResource EntityResource;
        protected readonly IDecorationRepository DecorationRepository;

        protected readonly IMarkRepository MarkRepository;
        protected readonly IKnowledgeRepository KnowledgeRepository;
        protected readonly IUserRepository UserRepository;
        protected readonly IUserRelationRepository UserRelationRepository;
        protected readonly string KnowledgeResource;

        protected readonly int DefaultLimit = 20;



        public KnowledgeService(
            IResourceFactory resourceFactory,
            IKnowledgeRepository knowledgeRepository,
            IDecorationRepository decorationRepository,
            IUserRepository userRepository,
            IUserRelationRepository userRelationRepository,
            IMarkRepository markRepository
        )
        {
            ResourceFactory = resourceFactory;
            ResponseResource = resourceFactory.GetResponseResource();
            EntityResource = resourceFactory.GetEntityResource();

            UserRelationRepository = userRelationRepository;
            DecorationRepository = decorationRepository;
            MarkRepository = markRepository;
            UserRepository = userRepository;
            KnowledgeRepository = knowledgeRepository;
            KnowledgeResource = EntityResource.Knowledge();
        }


        protected virtual async Task<List<ResponseUserCardModel>> DecorateUser(Guid myUid, List<ViewUserProfile> users)
        {
            return await DecorationRepository.DecorateResponseUserCardModel(myUid, users.Select(u =>
            {
                ResponseUserCardModel resUser = new();
                resUser.Copy(u);
                return resUser;
            }).ToList());
        }

        public async Task<ServiceResult> GetListUserMarkKnowledge(Guid myUid, Guid knowledgeId, PaginationDto pagination)
        {
            // Kiểm tra knowledge tồn tại và phải là chủ nhân myUid
            Knowledge knowledge = await KnowledgeRepository.CheckExisted(knowledgeId, ResponseResource.NotExist(KnowledgeResource));
            if (knowledge.UserId != myUid)
                return ServiceResult.Forbidden("Bạn không phải chủ nhân của phần tử kiến thức này");

            // Lấy về danh sách những người đã mark knowledge (có tính người đã bị banned không?)
            // Tạm thời lấy toàn bộ những người đã mark, kể cả banned
            PaginationResponseModel<ViewUserProfile> listUsers =
                await KnowledgeRepository.GetListUserMarkedKnowledge(knowledgeId, pagination);

            // Phân trang và trang trí cho từng ViewUserProfile -> ResponseUserCardModel
            PaginationResponseModel<ResponseUserCardModel> res = new()
            {
                Total = listUsers.Total,
                Limit = listUsers.Limit,
                Offset = listUsers.Offset,
                Results = await DecorateUser(myUid, listUsers.Results)
            };

            // Trả về thành công (PaginationItemModel)
            return ServiceResult.Success(ResponseResource.GetSuccess(), string.Empty, res);
        }

        public async Task<ServiceResult> Mark(Guid myUid, Guid knowledgeId, bool isMark)
        {
            // Kiểm tra myUid và knowledgeId tồn tại
            _ = await UserRepository.CheckExisted(myUid, ResponseResource.NotExistUser());
            _ = await KnowledgeRepository.CheckExisted(knowledgeId, ResponseResource.NotExist(KnowledgeResource));

            // Cho phép mark/unmark cả những knowledgeId mà không truy cập tới, chỉ không hiển thị được khi get ra mà thôi
            // Kiểm tra đã đánh dấu chưa và yêu cầu là thêm mark hay bỏ mark:
            Mark? mark = await KnowledgeRepository.GetMark(myUid, knowledgeId);

            if (isMark && mark == null)
            {
                // Khi yêu cầu thêm mới mark mà mark == null:
                mark = new Mark()
                {
                    MarkId = Guid.NewGuid(),
                    UserId = myUid,
                    KnowledgeId = knowledgeId,
                    CreatedTime = DateTime.UtcNow,
                    CreatedBy = myUid.ToString()
                };
                Guid? res = await MarkRepository.Insert(mark.MarkId, mark);
                if (res == null) return ServiceResult.ServerError(ResponseResource.ServerError());
            }
            else if (!isMark && mark != null)
            {
                // Khi yêu cầu bỏ mark mà mark != null
                int res = await MarkRepository.Delete(mark.MarkId);
                if (res <= 0) return ServiceResult.ServerError(ResponseResource.ServerError());
            }

            // Trả về thành công
            return ServiceResult.Success(ResponseResource.Success());
        }
    }
}
