using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Exceptions;
using KnowledgeSharingApi.Domains.Interfaces.ResourcesInterfaces;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
using KnowledgeSharingApi.Infrastructures.Encrypts;
using KnowledgeSharingApi.Infrastructures.Interfaces.Repositories;
using KnowledgeSharingApi.Services.Filters;
using KnowledgeSharingApi.Services.Interfaces;
using KnowledgeSharingApi.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom;
using System.Reflection;
using System.Security.Claims;

namespace KnowledgeSharingApi.Controllers
{
    public abstract class BaseController: ControllerBase
    {
        protected string[] BanFields = ["Password", "HashPassword", ];

        protected virtual Guid? GetCurrentUserId()
        {
            string? myUid = KSEncrypt.GetClaimValue(HttpContext.User, ClaimTypes.NameIdentifier);
            if (myUid == null) return null;
            if (Guid.TryParse(myUid, out Guid value))
            {
                return value;
            }
            return null;
        }

        protected virtual Guid GetCurrentUserIdStrictly()
        {
            return GetCurrentUserId() ?? throw new ResponseException()
            {
                StatusCode = EStatusCode.ServerError,
                UserMessage = "Lỗi hệ thống",
                DevMessage = "Get current userid strictly failed"
            };
        }

        protected virtual IActionResult StatusCode(ServiceResult result)
        {
            return StatusCode((int)result.StatusCode, new ApiResponse(result));
        }

        protected virtual List<OrderDto> ParseOrder(string? sort)
        {
            if (string.IsNullOrEmpty(sort)) return []; 

            var sortFields = new List<OrderDto>();
            // example of sort: https://....?...&sort=field1[:asc],field2[:desc]
            var sortParts = sort.Split(',');
            foreach (var part in sortParts)
            {
                var fieldInfo = part.Split(':');
                if (fieldInfo.Length >= 1)
                { 
                    var field = fieldInfo[0];
                    if (BanFields.Contains(field)) continue;

                    var desc = false;
                    if (fieldInfo.Length >= 2)
                    {
                        desc = fieldInfo[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase);
                    }

                    sortFields.Add(new OrderDto()
                    {
                        Field = field,
                        IsAscending = !desc
                    });
                }
            }
            return sortFields;
        }

        protected virtual List<FilterDto> ParseFilter(string? filterString)
        {
            List<FilterDto> filters = [];

            if (string.IsNullOrEmpty(filterString))
                return filters;
            if (string.IsNullOrWhiteSpace(filterString))
                return filters;

            // example of filter: https://....?...&filter=field1:value1[:op1],field2:value2[:op2],...
            string[] filterSegments = filterString.Split(',');
            foreach (var segment in filterSegments)
            {
                string[] parts = segment.Split(':');
                // Đảm bảo rằng có ít nhất hai phần tử (field và value)
                if (parts.Length < 2)
                    continue;

                string field = parts[0];
                if (BanFields.Contains(field)) continue;

                string value = parts[1];
                string operation = FilterOperations.Equal;

                if (parts.Length > 2)
                {
                    operation = MapFilterOperation(parts[2]);
                }

                filters.Add(new FilterDto()
                {
                    Field = field,
                    Value = value,
                    Operation = operation
                });
            }

            return filters;
        }

        protected virtual string MapFilterOperation(string pureString)
        {
            string[] equals = ["e", "eq", "eql", "equal", "equals", "equal-to", "equals-to", "eql-to", "eq-to", "e-to"];
            string[] gt = ["greater", "g", "gt", "greate", "large", "lg", "larger", "big", "bigger", "b", "bg", "lgt", "bgt", "bt"];
            string[] gte = ["gte", "ge", "greater-equal", "gteq", "gt-eq", "greater-or-equal", "bte", "bteq", "bgte", "lge", "lgeq", "lgte"];
            string[] lt = ["less", "less-than", "lt", "small", "smaller", "smaller-than", "sm", "st", "smt"];
            string[] lte = ["less-or-equal", "lte", "smaller-or-equal", "ste", "smte", "se", "le", "sme"];
            string[] ne = ["ne", "not-equal", "different", "neq"];
            string[] contain = ["ctn", "ct", "contain", "inc", "icl", "include", "ic"];
            string[] like = ["li", "like", "same", "lk", "lik", "lke"];

            if (equals.Contains(pureString)) return FilterOperations.Equal;
            if (gt.Contains(pureString)) return FilterOperations.GreaterThan;
            if (gte.Contains(pureString)) return FilterOperations.GreaterThanOrEqual;
            if (lt.Contains(pureString)) return FilterOperations.LessThan;
            if (lte.Contains(pureString)) return FilterOperations.LessThanOrEqual;
            if (ne.Contains(pureString)) return FilterOperations.NotEqual;
            if (contain.Contains(pureString)) return FilterOperations.Contain;
            if (like.Contains(pureString)) return FilterOperations.Like;

            return FilterOperations.Equal;
        }

    }
}
