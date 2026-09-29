using CafeteriaAromas.Data;
using CafeteriaAromas.Models;
using CafeteriaAromas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAromas.Controllers;

[Authorize]
public class RecipesController : Controller
{
    private readonly CafeteriaDbContext _dbContext;

    public RecipesController(CafeteriaDbContext context)
    {
        _dbContext = context;
    }

    /// <summary>
    /// GET, para la view principal, carga los datos al viewModel validando si no es vacios
    /// para iniciarlos y no rompa la aplicacion
    /// </summary>
    /// <param name="productId"> es opcional a null asi me ahorro de hacer la sobrecarga</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Index(int? productId)
    {
        var model = new RecipeManagementViewModel
        {
            SelectedProductId = productId
        };

        await PopulateDropdowns(model);

        if (productId.HasValue)
        {
            var recipe = await _dbContext.Recipes
                .FirstOrDefaultAsync(r => r.ProductId == productId.Value);

            if (recipe != null)
            {
                model.RecipeId = recipe.Id;
                model.Instructions = recipe.Instructions;

                model.Ingredients = await _dbContext.RecipeSupplies
                    .Where(rs => rs.RecipeId == recipe.Id)
                    .Include(rs => rs.Supply)
                    .Select(rs => new RecipeItemViewModel
                    {
                        RecipeSupplyId = rs.Id,
                        SupplyId = rs.SupplyId,
                        SupplyName = rs.Supply.Name,
                        UnitOfMeasure = rs.Supply.UnitOfMeasure,
                        IngredientQuantity = rs.IngredientQuantity
                    }).ToListAsync();
            }
        }

        return View(model);
    }

    /// <summary>
    /// Para agregar un nuevo ingrediente a lista de la receta
    /// </summary>
    /// <param name="model"></param>
    /// <param name="newSupplyId"></param>
    /// <param name="newQuantity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddIngredient(RecipeManagementViewModel model, int? newSupplyId, decimal? newQuantity)
    {
        await PopulateDropdowns(model);

        if (newSupplyId.HasValue && newQuantity.HasValue && newQuantity > 0)
        {
            var supply = await _dbContext.Supplies.FindAsync(newSupplyId.Value);
            if (supply != null)
            {
                model.Ingredients.Add(new RecipeItemViewModel
                {
                    SupplyId = supply.Id,
                    SupplyName = supply.Name,
                    UnitOfMeasure = supply.UnitOfMeasure,
                    IngredientQuantity = newQuantity.Value,
                    RecipeSupplyId = 0
                });
            }
        }

        return View("Index", model);
    }

    /// <summary>
    /// Eliminar el ingrediente selecionaddo, con el indice
    /// </summary>
    /// <param name="model"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RemoveIngredient(RecipeManagementViewModel model, int index)
    {
        await PopulateDropdowns(model);

        if (index >= 0 && index < model.Ingredients.Count)
        {
            model.Ingredients.RemoveAt(index);
        }

        return View("Index", model);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SaveRecipe(RecipeManagementViewModel model)
    {
        if (!model.SelectedProductId.HasValue)
            return RedirectToAction(nameof(Index));

        // pa buscar si la receta ya existe
        var recipe = await _dbContext.Recipes
            .FirstOrDefaultAsync(r => r.ProductId == model.SelectedProductId.Value);

        if (recipe == null)
        {
            recipe = new Recipe
            {
                ProductId = model.SelectedProductId.Value,
                Instructions = model.Instructions
            };
            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            recipe.Instructions = model.Instructions;
            _dbContext.Recipes.Update(recipe);
        }

        // borrar ingredientes antiguos de la receta
        var existingSupplies = await _dbContext.RecipeSupplies
            .Where(rs => rs.RecipeId == recipe.Id)
            .ToListAsync();
        _dbContext.RecipeSupplies.RemoveRange(existingSupplies);

        // a guardar los nuevos ingredientes seleccionados
        foreach (var item in model.Ingredients)
        {
            _dbContext.RecipeSupplies.Add(new RecipeSupply
            {
                RecipeId = recipe.Id,
                SupplyId = item.SupplyId,
                IngredientQuantity = item.IngredientQuantity
            });
        }

        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new { productId = model.SelectedProductId });
    }
    
    /// <summary>
    /// Para cargar datos necesarion pa los dropdowns, modifica al objeto por referencia
    /// </summary>
    /// <param name="model"></param>
    private async Task PopulateDropdowns(RecipeManagementViewModel model)
    {
        var deleteProduct = await ProductsController.GetDeletedIdsAsync();
        
        model.ProductList = await _dbContext.Products
            .Where(p => !deleteProduct.Contains(p.Id))
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name,
                Selected = p.Id == model.SelectedProductId
            }).ToListAsync();

        model.AvailableSupplies = await _dbContext.Supplies
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.Name} ({s.UnitOfMeasure})"
            }).ToListAsync();
    }
}