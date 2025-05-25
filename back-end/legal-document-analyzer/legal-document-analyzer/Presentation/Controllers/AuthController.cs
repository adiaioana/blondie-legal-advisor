using legal_document_analyzer.Application.DTOs;
using legal_document_analyzer.Domain.Repositories;
using legal_document_analyzer.Domain.ValueObjects;
using legal_document_analyzer.Infrastructure.Extensions;
using legal_document_analyzer.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace legal_document_analyzer.Presentation.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO user)
        {
            if (await _userRepository.UserExists(user.Username))
                return BadRequest("Username already exists");

            var createdUser = await _userRepository.Register(user, user.Password);
            return Ok(createdUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userInfo)
        {
            var user = await _userRepository.Login(userInfo.Username, userInfo.Password);
            if (user == null)
                return Unauthorized();

            var jwtToken = _tokenService.GenerateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Save the refresh token in the database
            _userRepository.SaveRefreshToken(user.Username, refreshToken);
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                //Secure = true, // only send over HTTPS
                SameSite = SameSiteMode.Lax, // or Lax depending on your use case
                Expires = DateTime.UtcNow.AddDays(7)
            });


            return Ok(new { token = jwtToken, refreshToken });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.RefreshToken))
            {
                return BadRequest("Invalid client request");
            }

            var token = Request.GetBearerToken();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token");
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                return BadRequest("Invalid token");
            }

            var username = principal.Identity.Name;

            // Remove the refresh token from the database
            _userRepository.RemoveRefreshToken(username, tokenRequest.RefreshToken);

            return Ok();
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.RefreshToken))
            {
                return BadRequest("Invalid client request");
            }

            var token = Request.GetBearerToken();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token");
            }

            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                return BadRequest("Invalid token");
            }

            var username = principal.Identity.Name;
            var savedRefreshToken = _userRepository.GetRefreshToken(username); // Retrieve the saved refresh token from the database

            if (savedRefreshToken != tokenRequest.RefreshToken)
            {
                return Unauthorized("Invalid refresh token");
            }

            var newJwtToken = _tokenService.GenerateJwtToken(principal);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Save the new refresh token in the database
            _userRepository.SaveRefreshToken(username, newRefreshToken);

            return Ok(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpGet("user")]
        [Authorize] // Ensures only authenticated users can access
        public async Task<IActionResult> GetUserInfo()
        {
            // Get the username from the authenticated user's claims
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not authenticated");

            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
                return NotFound("User not found");

            // Return user info without sensitive data
            return Ok(new
            {
                userId = user.UserId,
                username = user.Username,
                email = user.Email
            });
        }

        [Authorize]
        [HttpGet("user/current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized("Invalid user identity");
            }

            var user = await _userRepository.GetUserById(userGuid);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Returnează informațiile despre utilizator, excluzând datele sensibile
            return Ok(new
            {
                userId = user.UserId,
                username = user.Username,
                email = user.Email,
                //firstName = user.FirstName,
                //lastName = user.LastName,
                //phoneNumber = user.PhoneNumber,
                //address = user.Address,
                //birthdate = user.Birthdate,
                //createdAt = user.CreatedAt
            });
        }

        [Authorize]
        [HttpPut("user/update")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserUpdateDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized("Invalid user identity");
            }

            var user = await _userRepository.GetUserById(userGuid);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Actualizează doar câmpurile permise
            user.Username = model.Username ?? user.Username;
            user.Email = model.Email ?? user.Email;
            //user.FirstName = model.FirstName ?? user.FirstName;
            //user.LastName = model.LastName ?? user.LastName;
            //user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            //user.Address = model.Address ?? user.Address;
            //user.Birthdate = model.Birthdate ?? user.Birthdate;

            // Salvează modificările
            var updatedUser = await _userRepository.UpdateUser(user);

            return Ok(new
            {
                userId = updatedUser.UserId,
                username = updatedUser.Username,
                email = updatedUser.Email,
                //firstName = updatedUser.FirstName,
                //lastName = updatedUser.LastName,
                //phoneNumber = updatedUser.PhoneNumber,
                //address = updatedUser.Address,
                //birthdate = updatedUser.Birthdate,
                //createdAt = updatedUser.CreatedAt
            });
        }
    }
}