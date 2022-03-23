using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<AccountsController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountsController(UserManager<ApiUser> userManager, 
            ILogger<AccountsController> logger,
            IMapper mapper,IAuthManager authManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email}");
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _mapper.Map<ApiUser>(userDTO);
                    user.UserName = userDTO.Email;
                    var result = await _userManager.CreateAsync(user,userDTO.Password);
                    if (!result.Succeeded)
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                    await _userManager.AddToRolesAsync(user, userDTO.Roles);
                    return Accepted();
                }catch(Exception ex)
                {
                    _logger.LogError(ex, $"Someting Went Wrong{nameof(Register)}");
                    return Problem($"Someting Went Wrong{nameof(Register)}", statusCode: 500);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            _logger.LogInformation($"Login Attempt for {loginUserDTO.Email}");
            if (ModelState.IsValid)
            {
                try
                {
                   if(!await _authManager.ValidateUser(loginUserDTO))
                    {
                        return Unauthorized();
                    }
                    return Accepted(new { Token = await _authManager.CreateToken() });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Someting Went Wrong{nameof(Login)}");
                    return Problem($"Someting Went Wrong{nameof(Login)}", statusCode: 500);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
