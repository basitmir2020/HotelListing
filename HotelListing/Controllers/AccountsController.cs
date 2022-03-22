using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
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

        public AccountsController(UserManager<ApiUser> userManager, 
            ILogger<AccountsController> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
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

        /*[HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            _logger.LogInformation($"Login Attempt for {loginUserDTO.Email}");
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _signInManager.PasswordSignInAsync(loginUserDTO.Email, loginUserDTO.Password, false, false);
                    if (!result.Succeeded)
                    {
                        return Unauthorized(loginUserDTO);
                    }
                    return Accepted();
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
        }*/

    }
}
