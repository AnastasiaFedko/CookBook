using System.Data.Entity;

namespace Сookbook_EXAM
{
    public class CookBookContext : DbContext
    {
        static CookBookContext()
        {
            Database.SetInitializer<CookBookContext>(new CookBookInitializer());      
        }
      
        public CookBookContext() : base("CookBookConnection") { }

        public DbSet<Recipe> dbRecipes { get; set; }
        public DbSet<FoodProduct> dbProducts { get; set; }
        public DbSet<FoodProductRecipes> dbProductsForRecipe { get; set; }
        public DbSet<DishType> dbDishTypes { get; set; }
        public DbSet<Cuisine> dbCuisines { get; set; }
        public DbSet<Step> dbSteps { get; set; }
    }
}
