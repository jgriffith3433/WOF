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
        var product = new Product
        {
            Name = "Pompeian Organic Robust Extra Virgin Olive Oil",
            WalmartId = 13281639
        };

        if (!_context.Recipes.Any())
        {
            _context.Recipes.Add(new Recipe
            {
                Name = "Slow-Cooked Habanero Chili",
                UserImport = "Products\r\n3 tablespoons olive oil\r\n\r\n1 pound lean ground turkey\r\n\r\n1 cup red bell pepper, chopped\r\n\r\n3 cloves garlic, minced\r\n\r\n1 (16 ounce) can kidney beans, rinsed and drained\r\n\r\n1 (16 ounce) can black beans, rinsed and drained\r\n\r\n1 cup rinsed and drained canned black-eyed peas\r\n\r\n1 (15 ounce) can low sodium tomato sauce\r\n\r\n1 dried habanero pepper, chopped\r\n\r\n1 cup frozen corn kernels\r\n\r\n1 tablespoon packed brown sugar\r\n\r\n1 teaspoon Worcestershire sauce\r\n\r\n1 tablespoon dried basil\r\n\r\n1 teaspoon dried sage\r\n\r\nsalt to taste",
                Link = "https://www.allrecipes.com/recipe/100705/slow-cooked-habanero-chili/",
                CalledIngredients =
                {
                    new CalledIngredient
                    {
                        Name = "Olive Oil",
                        Product = product
                    }
                }
            });
        }

        if (!_context.CompletedOrders.Any())
        {
            _context.CompletedOrders.Add(new CompletedOrder
            {
                UserImport = "[{\"name\":\"all Liquid Laundry Detergent with Advanced OXI Stain Removers and Whiteners, Free Clear, 184.5 Ounce, 103 Loads\",\"link\":\"https://www.walmart.com/ip/all-Liquid-Laundry-Detergent-with-Advanced-OXI-Stain-Removers-and-Whiteners-Free-Clear-184-5-Ounce-103-Loads/52538272\",\"quantity\":\"Qty 1\",\"price\":\"$14.97\"},{\"name\":\"Best Foods Gluten-Free Mayonnaise, 48 Fl Oz\",\"link\":\"https://www.walmart.com/ip/Best-Foods-Gluten-Free-Mayonnaise-48-Fl-Oz/10291786\",\"quantity\":\"Qty 1\",\"price\":\"$7.64\"},{\"name\":\"Crest Pro Health Advanced Mouthwash, Alcohol Free, Fresh Mint, 33.8 fl oz\",\"link\":\"https://www.walmart.com/ip/Crest-Pro-Health-Advanced-Mouthwash-Alcohol-Free-Fresh-Mint-33-8-fl-oz/468845622\",\"quantity\":\"Qty 1\",\"price\":\"$10.97\"},{\"name\":\"Great Value Classic Ranch Dressing & Dip, 36 fl oz\",\"link\":\"https://www.walmart.com/ip/Great-Value-Classic-Ranch-Dressing-Dip-36-fl-oz/22282307\",\"quantity\":\"Qty 1\",\"price\":\"$3.34\"},{\"name\":\"Great Value Dark Red Kidney Beans, 15.5 oz\",\"link\":\"https://www.walmart.com/ip/Great-Value-Dark-Red-Kidney-Beans-15-5-oz/10534045\",\"quantity\":\"Qty 1\",\"price\":\"$0.78\"},{\"name\":\"Del Monte Canned Golden Sweet Whole Kernel Corn, 15.25 oz Can\",\"link\":\"https://www.walmart.com/ip/Del-Monte-Canned-Golden-Sweet-Whole-Kernel-Corn-15-25-oz-Can/10295217\",\"quantity\":\"Qty 1\",\"price\":\"$1.38\"},{\"name\":\"Progresso Vegetable Classics, Garden Vegetable Canned Soup, 19 oz.\",\"link\":\"https://www.walmart.com/ip/Progresso-Vegetable-Classics-Garden-Vegetable-Canned-Soup-19-oz/10320699\",\"quantity\":\"Qty 1\",\"price\":\"$2.18\"},{\"name\":\"Great Value Canned Black Eyed Peas, 15.5 oz Can\",\"link\":\"https://www.walmart.com/ip/Great-Value-Canned-Black-Eyed-Peas-15-5-oz-Can/10451547\",\"quantity\":\"Qty 1\",\"price\":\"$1.18\"},{\"name\":\"Hunt's Tomato Sauce, No Salt Added, 29 oz Can\",\"link\":\"https://www.walmart.com/ip/Hunt-s-Tomato-Sauce-No-Salt-Added-29-oz-Can/40265449\",\"quantity\":\"Qty 1\",\"price\":\"$1.98\"},{\"name\":\"S&W - Dark Red Kidney Beans - 29 oz. Can\",\"link\":\"https://www.walmart.com/ip/S-W-Dark-Red-Kidney-Beans-29-oz-Can/46298085\",\"quantity\":\"Qty 1\",\"price\":\"$2.42\"},{\"name\":\"S&W - Black Beans - Low Sodium - 15 oz. Can\",\"link\":\"https://www.walmart.com/ip/S-W-Black-Beans-Low-Sodium-15-oz-Can/10314013\",\"quantity\":\"Qty 1\",\"price\":\"$1.42\"},{\"name\":\"S&W - Black Beans - Low Sodium - 15 oz. Can\",\"link\":\"https://www.walmart.com/ip/S-W-Black-Beans-Low-Sodium-15-oz-Can/10314013\",\"quantity\":\"Qty 1\",\"price\":\"$1.42\"},{\"name\":\"Progresso Rich & Hearty, New England Clam Chowder Soup, Gluten Free, 18.5 oz.\",\"link\":\"https://www.walmart.com/ip/Progresso-Rich-Hearty-New-England-Clam-Chowder-Soup-Gluten-Free-18-5-oz/10320671\",\"quantity\":\"Qty 1\",\"price\":\"$1.96\"},{\"name\":\"Hunt's Tomato Sauce, No Salt Added, 8 oz Can\",\"link\":\"https://www.walmart.com/ip/Hunt-s-Tomato-Sauce-No-Salt-Added-8-oz-Can/10295085\",\"quantity\":\"Qty 1\",\"price\":\"$0.60\"},{\"name\":\"Great Value Canned Black Eyed Peas, 15.5 oz Can\",\"link\":\"https://www.walmart.com/ip/Great-Value-Canned-Black-Eyed-Peas-15-5-oz-Can/10451547\",\"quantity\":\"Qty 1\",\"price\":\"$1.18\"},{\"name\":\"Progresso Light, Chicken Noodle Soup, 18.5 oz.\",\"link\":\"https://www.walmart.com/ip/Progresso-Light-Chicken-Noodle-Soup-18-5-oz/10813886\",\"quantity\":\"Qty 1\",\"price\":\"$2.18\"},{\"name\":\"Rosarita Traditional Refried Beans, 16 oz\",\"link\":\"https://www.walmart.com/ip/Rosarita-Traditional-Refried-Beans-16-oz/10292168\",\"quantity\":\"Qty 1\",\"price\":\"$1.24\"},{\"name\":\"Great Value Kosher Basil Leaves, 0.8 oz\",\"link\":\"https://www.walmart.com/ip/Great-Value-Kosher-Basil-Leaves-0-8-oz/330313658\",\"quantity\":\"Qty 1\",\"price\":\"$1.12\"},{\"name\":\"Fresh Red Bell Pepper, 1 Each\",\"link\":\"https://www.walmart.com/ip/Fresh-Red-Bell-Pepper-1-Each/44391581\",\"quantity\":\"Qty 3\",\"price\":\"$4.26\"},{\"name\":\"Garlic Bulb, Each\",\"link\":\"https://www.walmart.com/ip/Garlic-Bulb-Each/44391100\",\"quantity\":\"Wt 0.42 lb\",\"price\":\"$1.79\"},{\"name\":\"Mission Super Soft Taco Flour Tortillas, 10 Count\",\"link\":\"https://www.walmart.com/ip/Mission-Super-Soft-Taco-Flour-Tortillas-10-Count/10309357\",\"quantity\":\"Qty 1\",\"price\":\"$2.78\"},{\"name\":\"Great Value Steamable Whole Kernel Corn, 12 oz (Frozen)\",\"link\":\"https://www.walmart.com/ip/Great-Value-Steamable-Whole-Kernel-Corn-12-oz-Frozen/659879040\",\"quantity\":\"Qty 1\",\"price\":\"$0.98\"},{\"name\":\"Famous Dave's Original Cornbread Mix, 15 oz\",\"link\":\"https://www.walmart.com/ip/Famous-Dave-s-Original-Cornbread-Mix-15-oz/39224284\",\"quantity\":\"Qty 1\",\"price\":\"$2.64\"},{\"name\":\"Fresh Zucchini Squash, Each\",\"link\":\"https://www.walmart.com/ip/Fresh-Zucchini-Squash-Each/44390947\",\"quantity\":\"Wt 1.24 lb\",\"price\":\"$1.84\"},{\"name\":\"Great Value Ground Sage, 1.25 oz\",\"link\":\"https://www.walmart.com/ip/Great-Value-Ground-Sage-1-25-oz/487738251\",\"quantity\":\"Qty 1\",\"price\":\"$2.00\"},{\"name\":\"Mainstays Solid Bath Sheet, Turquoise\",\"link\":\"https://www.walmart.com/ip/Mainstays-Solid-Bath-Sheet-Turquoise/760379597\",\"quantity\":\"Qty 1\",\"price\":\"$6.94\"}]",
                Products =
                {
                    product
                }
            });
        }
        await _context.SaveChangesAsync();
    }
}
