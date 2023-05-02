using WOF.Domain.Entities;
using WOF.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WOF.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });
        }

        if (!_context.Recipes.Any())
        {
            _context.Recipes.Add(new Recipe
            {
                Name = "Slow-Cooked Habanero Chili",
                UserImport = "Ingredients\r\n3 tablespoons olive oil\r\n\r\n1 pound lean ground turkey\r\n\r\n1 cup red bell pepper, chopped\r\n\r\n3 cloves garlic, minced\r\n\r\n1 (16 ounce) can kidney beans, rinsed and drained\r\n\r\n1 (16 ounce) can black beans, rinsed and drained\r\n\r\n1 cup rinsed and drained canned black-eyed peas\r\n\r\n1 (15 ounce) can low sodium tomato sauce\r\n\r\n1 dried habanero pepper, chopped\r\n\r\n1 cup frozen corn kernels\r\n\r\n1 tablespoon packed brown sugar\r\n\r\n1 teaspoon Worcestershire sauce\r\n\r\n1 tablespoon dried basil\r\n\r\n1 teaspoon dried sage\r\n\r\nsalt to taste",
                Link = "https://www.allrecipes.com/recipe/100705/slow-cooked-habanero-chili/",
                CalledIngredients =
                {
                    new CalledIngredient
                    {
                        Name = "Olive Oil",
                        Ingredient = new Ingredient
                        {
                            Name = "Pompeian Organic Robust Extra Virgin Olive Oil",
                            WalmartId = 13281639
                        }
                    }
                }
            });
        }

        if (!_context.CompletedOrders.Any())
        {
            _context.CompletedOrders.Add(new CompletedOrder
            {
                UserImport = "Store purchase\r\nApr 29, 2023 purchase\r\n28 items\r\n\r\n\r\nall Liquid Laundry Detergent with Advanced OXI Stain Removers and Whiteners, Free Clear, 184.5 Ounce, 103 Loads\r\nQty 1\r\n$14.97\r\nWrite a review\r\n\r\nBest Foods Gluten-Free Mayonnaise, 48 Fl Oz\r\nQty 1\r\n$7.64\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nCrest Pro Health Advanced Mouthwash, Alcohol Free, Fresh Mint, 33.8 fl oz\r\nQty 1\r\n$10.97\r\nWrite a review\r\n\r\nGreat Value Classic Ranch Dressing & Dip, 36 fl oz\r\nQty 1\r\n$3.34\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nGreat Value Dark Red Kidney Beans, 15.5 oz\r\nQty 1\r\n$0.78\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nDel Monte Canned Golden Sweet Whole Kernel Corn, 15.25 oz Can\r\nQty 1\r\n$1.38\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nProgresso Vegetable Classics, Garden Vegetable Canned Soup, 19 oz.\r\nQty 1\r\n$2.18\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nGreat Value Canned Black Eyed Peas, 15.5 oz Can\r\nQty 1\r\n$1.18\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nHunt's Tomato Sauce, No Salt Added, 29 oz Can\r\nQty 1\r\n$1.98\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nS&W - Dark Red Kidney Beans - 29 oz. Can\r\nQty 1\r\n$2.42\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nS&W - Black Beans - Low Sodium - 15 oz. Can\r\nQty 1\r\n$1.42\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nS&W - Black Beans - Low Sodium - 15 oz. Can\r\nQty 1\r\n$1.42\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nProgresso Rich & Hearty, New England Clam Chowder Soup, Gluten Free, 18.5 oz.\r\nQty 1\r\n$1.96\r\nWrite a review\r\n\r\nHunt's Tomato Sauce, No Salt Added, 8 oz Can\r\nQty 1\r\n$0.60\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nGreat Value Canned Black Eyed Peas, 15.5 oz Can\r\nQty 1\r\n$1.18\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nProgresso Light, Chicken Noodle Soup, 18.5 oz.\r\nQty 1\r\n$2.18\r\nWrite a review\r\n\r\nRosarita Traditional Refried Beans, 16 oz\r\nQty 1\r\n$1.24\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nGreat Value Kosher Basil Leaves, 0.8 oz\r\nQty 1\r\n$1.12\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nFresh Red Bell Pepper, 1 Each\r\nQty 3\r\n$4.26\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nGarlic Bulb, Each\r\n$4.27/lb\r\nWt 0.42 lb\r\n$1.79\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nMission Super Soft Taco Flour Tortillas, 10 Count\r\nQty 1\r\n$2.78\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nGreat Value Steamable Whole Kernel Corn, 12 oz (Frozen)\r\nQty 1\r\n$0.98\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nFamous Dave's Original Cornbread Mix, 15 oz\r\nQty 1\r\n$2.64\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nFresh Zucchini Squash, Each\r\n$1.48/lb\r\nWt 1.24 lb\r\n$1.84\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nGreat Value Ground Sage, 1.25 oz\r\nQty 1\r\n$2.00\r\n\r\nAdd to cart\r\nWrite a review\r\n\r\nMainstays Solid Bath Sheet, Turquoise\r\nQty 1\r\n$6.94\r\nWrite a review"
            });
        }
        await _context.SaveChangesAsync();
    }
}
