using ApiBlog.Data;
using ApiBlog.Models;
using ApiBlogAspNet.Extensions;
using ApiBlogAspNet.Services;
using ApiBlogAspNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace ApiBlogAspNet.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        //private readonly TokenService _tokenService;
        //public AccountController(TokenService   tokenService)
        //{
        //       _tokenService = tokenService;    
        //}


        [HttpPost(template: "v1/accounts")]
         public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model,
        [FromServices] ApiDataContext context)  
         {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User
        {
           Name = model.Name,
           Email = model.Email,
           Slug = model.Email.Replace(oldValue: "@", newValue: "-").Replace(oldValue: "-", newValue: "-")
        };
            var password = PasswordGenerator.Generate(length: 25, includeSpecialChars: true, upperCase: false);
              user.PasswordHash = PasswordHasher.Hash(password);
            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>( new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("05X99 - Este E-mail já está cadastrado"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }
        }


        [HttpPost(template: "v1/accounts/login")]
        public async Task<IActionResult> Login(
            [FromBody]LoginViewModel model,
            [FromServices] ApiDataContext context,
            [FromServices] TokenService tokenService) //here in [FromServices]TokenService tokenService ** the same functionality of uper commet
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
            }

        }
        //    [Authorize(Roles = "user")]
        //    [HttpGet(template: "v1/user")]
        //    public IActionResult GetUser() => Ok(User.Identity.Name);


        //    [Authorize(Roles = "author")]
        //    [HttpGet(template: "v1/author")]
        //    public IActionResult GetAuthor() => Ok(User.Identity.Name);

        //    [Authorize(Roles = "admin")]
        //    [HttpGet(template: "v1/admin")]
        //    public IActionResult GetAdmin() => Ok(User.Identity.Name);
        //}   this condctions are only for tests 
    }
}

