using ApiBlog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBlog.Models;
using ApiBlogAspNet.ViewModels;
using ApiBlogAspNet.Extensions;

namespace ApiBlogAspNet.Controllers
{ 
    [ApiController]
    public class CategoryController: Controller
    {

            [HttpGet("v1/categories")]
            public async Task<IActionResult> GetAsync(
                [FromServices] ApiDataContext context)
            {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>> ("Falha interna no servidor"+ex));
            }

        }

        [HttpGet("v1/categories/{Id:int}")]
            public async Task<IActionResult> GetByIdAsync(
                [FromRoute] int id,
                [FromServices] ApiDataContext context)
            {
            try
            {
                var category = await context
              .Categories
              .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null) return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servido"));
            }
           
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
              [FromBody] EditorCategoryViewmodel model,
              [FromServices] ApiDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                                                                  
                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500,new ResultViewModel<Category>("Não foi possivel incluir a categoria"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
             [FromRoute] int id,
             [FromBody] EditorCategoryViewmodel model,
             [FromServices] ApiDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
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
        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
             [FromRoute] int id,
             [FromServices] ApiDataContext context)
        {
            try
            {
                var category = await context
             .Categories
             .FirstOrDefaultAsync(x => x.Id ==id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possivel incluir a categoria"));
            }
            catch (Exception e)
            {
                return StatusCode(500,new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }
        
    }
}

