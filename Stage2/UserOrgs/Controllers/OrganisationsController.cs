using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using UserOrgs.Data;
using UserOrgs.Dto;

namespace UserOrgs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganisationsController : ControllerBase
    {
        private readonly AppDbContext _dc;

        public OrganisationsController(AppDbContext dbContext)
        {
            _dc = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsersOrganisations()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _dc.Users.Include(u => u.organisations).FirstOrDefaultAsync(u => u.userId == userId);
            if (user is null) return NotFound(new FailiureResponse("Not Found", (int)HttpStatusCode.NotFound, "Not Found"));
            var organisations = user.organisations.Select(o => new OrganisationDto(o.orgId, o.name, o.description!)).ToList();
            return Ok(new SuccessResponse("success", new { organisations }));


        }

        [HttpGet("{orgId}")]
        public async Task<IActionResult> GetOrganisation(string orgId)
        {
            var org = await _dc.Organisations.FirstOrDefaultAsync(o => o.orgId == orgId);

            if (org is null)
                return NotFound(new FailiureResponse("Organisation Not Found", (int)HttpStatusCode.NotFound));

            return Ok(new SuccessResponse("success", org.ToOrgDataDto()));
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrganisation(OrganisationDto organisationDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var user = await _dc.Users.FindAsync(userId);
            if (user is null) return NotFound(new FailiureResponse("Not Found", (int)HttpStatusCode.NotFound, "Not Found"));

            var newOrg = new Organisation
            {
                orgId = Guid.NewGuid().ToString(),
                name = organisationDto.name,
                description = organisationDto.description
            };
            user.organisations.Add(newOrg);

            await _dc.SaveChangesAsync();

            return CreatedAtAction("CreateOrganisation", new SuccessResponse("success", newOrg.ToOrgDataDto()));
        }

        [HttpPost("{orgId}/users")]
        [AllowAnonymous]
        public async Task<ActionResult> AddUserToOrganisation(string orgId, [FromBody] UserIdDto idDto)
        {
            var user = await _dc.Users.FindAsync(idDto.userId);
            if (user is null) return NotFound(new FailiureResponse("Not Found", (int)HttpStatusCode.NotFound, "Not Found"));

            var org = await _dc.Organisations.FindAsync(orgId);
            if (org is null) return NotFound(new FailiureResponse("Not Found", (int)HttpStatusCode.NotFound, "Not Found"));
            if (!org.users.Contains(user))
            {
                org.users.Add(user);
                await _dc.SaveChangesAsync();
            }

            return Ok(new SuccessResponse("User added to organisation successfully", null!));
        }

    }
}
