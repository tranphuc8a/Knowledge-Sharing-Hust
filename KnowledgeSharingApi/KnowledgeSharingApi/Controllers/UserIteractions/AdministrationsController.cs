using KnowledgeSharingApi.Repositories.Interfaces;
using KnowledgeSharingApi.Repositories.Interfaces.Repositories;
using KnowledgeSharingApi.Services.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSharingApi.Controllers.UserIteractions
{
    [CustomAuthorization(Roles: "Admin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdministrationsController(IAdministrationRepository administrationRepository) : ControllerBase
    {
        protected readonly IAdministrationRepository AdministrationRepository = administrationRepository;

        [CustomAuthorization(Roles: "Admin")]
        [HttpPost("query-by-connection")]
        public async Task<IActionResult> QueryByConnection([FromBody] string query)
        {
            return Ok(await AdministrationRepository.QueryByConnection(query));
        }

        [CustomAuthorization(Roles: "Admin")]
        [HttpPost("query-by-dbcontext")]
        public async Task<IActionResult> QueryByDbContext([FromBody] string query)
        {
            return Ok(await AdministrationRepository.QueryByDbContext(query));
        }
    }
}
