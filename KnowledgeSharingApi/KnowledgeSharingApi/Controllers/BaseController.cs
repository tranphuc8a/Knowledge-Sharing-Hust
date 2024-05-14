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

        protected virtual List<(string Field, bool IsAscending)> ParseSortFields(string? sort)
        {
            if (string.IsNullOrEmpty(sort)) return []; 

            var sortFields = new List<(string Field, bool Ascending)>();
            // example of sort: https://....?...&sort=field1:asc,field2:desc
            var sortParts = sort.Split(',');
            foreach (var part in sortParts)
            {
                var fieldInfo = part.Split(':');
                if (fieldInfo.Length == 2)
                {
                    var field = fieldInfo[0];
                    var ascending = fieldInfo[1].Equals("asc", StringComparison.CurrentCultureIgnoreCase);
                    sortFields.Add((field, ascending));
                }
            }
            return sortFields;
        }

    }
}
