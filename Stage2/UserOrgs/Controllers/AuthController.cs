using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserOrgs.Dto;
using UserOrgs.Services;

namespace UserOrgs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public AuthController(AuthService authService, TokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(SuccessResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
        {
            var registeredUser = await _authService.RegisterUser(userRegisterDto);
            if (registeredUser is null)
                return BadRequest(new 
                    FailiureResponse(
                    "Registration unsuccessful",
                    (int)HttpStatusCode.BadRequest)
                    );

            UserDataDto user = registeredUser.ToUserDataDto();
            string accessToken = _tokenService.GenerateJwt(user);
            var response = new SuccessResponse("Registration successful", new { accessToken, user });
            return CreatedAtAction("RegisterUser", response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(SuccessResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
        {
            var loggedInUser = await _authService.LoginUser(userLoginDto);
            if (loggedInUser is null)
                return Unauthorized(new FailiureResponse("Authentication failed", (int)HttpStatusCode.Unauthorized));
            UserDataDto user = loggedInUser.ToUserDataDto();

            string accessToken = _tokenService.GenerateJwt(user);

            var response = new SuccessResponse("Login successful", new { accessToken, user });
            return Ok(response);
        }
    }
}
