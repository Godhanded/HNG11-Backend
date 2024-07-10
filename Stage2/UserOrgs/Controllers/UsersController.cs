using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserOrgs.Data;
using UserOrgs.Dto;

namespace UserOrgs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dc;

        public UsersController(AppDbContext dbContext)
        {
            _dc = dbContext;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserDetails(string userId)
        {
            var user = await _dc.Users.FindAsync(userId);
            if (user is null) return NotFound(new FailiureResponse("Not Found", (int)HttpStatusCode.NotFound, "Not Found"));

            return Ok(new SuccessResponse("success", user.ToUserDataDto()));
        }
    }
}
