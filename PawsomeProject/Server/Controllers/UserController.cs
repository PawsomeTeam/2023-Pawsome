using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Repositories;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly ILogger<UserController> logger;

    public UserController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<UserController> logger
    )
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user == null) return BadRequest("User does not exist");
        var singInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!singInResult.Succeeded) return BadRequest("Invalid password");
        await signInManager.SignInAsync(user, request.RememberMe);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest parameters)
    {
        var user = new User()
        {
            FirstName = parameters.FirstName,
            LastName = parameters.LastName,
            UserName = parameters.UserName,
            Email = parameters.Email,
            PhoneNumber = parameters.PhoneNumber
        };
        var result = await userManager.CreateAsync(user, parameters.Password);
        if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);
        return await Login(new LoginRequest
        {
            UserName = parameters.UserName,
            Password = parameters.Password
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<CurrentUser>> CurrentUserInfo()
    {
        var userName = User?.Identity?.Name;
        if (userName == null)
        {
            return new CurrentUser
            {
                IsAuthenticated = false
            };
        }

        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return new CurrentUser
            {
                IsAuthenticated = false
            };
        }

        return new CurrentUser
        {
            IsAuthenticated = User.Identity.IsAuthenticated,
            UserName = userName,
            FullName = user.FirstName + " " + user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Claims = User.Claims.ToDictionary(c => c.Type, c => c.Value)
        };
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<FindUser>>> GetAllUsers()
    {
        List<User> users = await userManager.Users.ToListAsync();
        List<FindUser> findUsers = new List<FindUser>();
        foreach (var user in users)
        {
            findUsers.Add(new FindUser
            {
                UserName = user.UserName,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            });
        }

        return findUsers;
    }

    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<FindUser>> GetUserByEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            return new FindUser
            {
                UserName = User.Identity.Name,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
        }

        return BadRequest("User does not exist");
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(string email, UpdateRequest updateRequest)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            user.FirstName = updateRequest.FirstName;
            user.LastName = updateRequest.LastName;
            user.Email = updateRequest.Email;
            user.PhoneNumber = updateRequest.PhoneNumber;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);
            return Ok();
        }

        return BadRequest("User does not exist");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);
            return Ok();
        }

        return BadRequest("User does not exist");
    }
}