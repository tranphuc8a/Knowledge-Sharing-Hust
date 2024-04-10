using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Services.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizationAttribute(string? Roles = null) : Attribute, IAuthorizationFilter
    {
        private readonly string? Roles = Roles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (IsAllowAnonymousUsed(context)) return;

            ApiResponse unauthorized = new()
            {
                StatusCode = EStatusCode.Unauthorized,
                UserMessage = ViConstantResource.UNAUTHORIZED,
                DevMessage = ViConstantResource.UNAUTHORIZED
            };

            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                // Người dùng chưa được xác thực, trả về lỗi 401 Unauthorized
                context.Result = unauthorized;
                return;
            }

            if (Roles != null)
            {
                List<string> requiredRoles = Roles.Split(",").Select(role => role.Trim()).ToList();

                // Kiểm tra xem người dùng có vai trò phù hợp không
                List<string> userRoles = context.HttpContext.User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).ToList();

                foreach (string role in requiredRoles)
                {
                    if (userRoles.Contains(role))
                    {
                        // Người dùng có vai trò hợp lệ, cho phép truy cập
                        return;
                    }
                }

                context.Result = new ApiResponse
                {
                    StatusCode = EStatusCode.Forbidden,
                    UserMessage = ViConstantResource.FORBIDDEN,
                    DevMessage = ViConstantResource.FORBIDDEN
                };
            }

            // passed
        }

        private static bool IsAllowAnonymousUsed(AuthorizationFilterContext context)
        {
            ControllerActionDescriptor? actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor != null)
            {
                return actionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType() == typeof(AllowAnonymousAttribute));
            }

            return false;
        }
    }
}
