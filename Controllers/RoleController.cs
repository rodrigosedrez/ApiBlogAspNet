using ApiBlog.Data;
using ApiBlog.Models;
using ApiBlogAspNet.Extensions;
using ApiBlogAspNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBlogAspNet.Controllers
{
    public class RoleController : ControllerBase
    {
        [HttpGet("v1/roles")]
        public async Task<IActionResult> GetAsync(
               [FromServices] ApiDataContext context)
        {
            try
            {
                var roles = await context.Roles.ToListAsync();
                return Ok(new ResultViewModel<List<Role>>(roles));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<List<Role>>("Falha interna no servidor"));
            }

        }

        [HttpGet("v1/roles/{Id:int}")]
        public async Task<IActionResult> GetByIdAsync(
                [FromRoute] int id,
                [FromServices] ApiDataContext context)
        {
            try
            {
                var role = await context
              .Roles
              .FirstOrDefaultAsync(x => x.Id == id);

                if (role == null) return NotFound(new ResultViewModel<Role>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Role>(role));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Role>("Falha interna no servido"));
            }

        }

        [HttpPost("v1/roles")]
        public async Task<IActionResult> PostAsync(
              [FromBody] EditorCategoryViewmodel model,
              [FromServices] ApiDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Role>(ModelState.GetErrors()));
            try
            {
                var role = new Role
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };
                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{role.Id}", new ResultViewModel<Role>(role));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Role>("Não foi possivel incluir a categoria"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<Role>("Falha interna no servidor"));
            }
        }

        [HttpPut("v1/roles/{id:int}")]
        public async Task<IActionResult> PutAsync(
             [FromRoute] int id,
             [FromBody] EditorCategoryViewmodel model,
             [FromServices] ApiDataContext context)
        {
            try
            {
                var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);

                if (role == null)
                    return NotFound(new ResultViewModel<Role>("Conteúdo não encontrado"));

                role.Name = model.Name;
                role.Slug = model.Slug;

                context.Roles.Update(role);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possivel incluir a categoria");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }
        [HttpDelete("v1/roles/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
             [FromRoute] int id,
             [FromServices] ApiDataContext context)
        {
            try
            {
                var role = await context
             .Roles
             .FirstOrDefaultAsync(x => x.Id ==id);

                if (role == null)
                    return NotFound(new ResultViewModel<Role>("Conteúdo não encontrado"));

                context.Roles.Remove(role);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Role>(role));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Role>("Não foi possivel incluir a categoria"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<Role>("Falha interna no servidor"));
            }
        }
    }
}
