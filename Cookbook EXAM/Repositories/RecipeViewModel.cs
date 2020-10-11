using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Data.Entity;

namespace Сookbook_EXAM
{
    public class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe selectedRecipe;

        public ObservableCollection<Recipe> AllRecipes { get; set; }
        public ObservableCollection<Cuisine> AllCuisines { get; set; }
        public ObservableCollection<FoodProduct> AllProducts { get; set; }
        public ObservableCollection<DishType> AllTypes { get; set; }
        public ObservableCollection<Unit> Units { get; set; }
       

        public Recipe SelectedRecipe
        {
            get { return selectedRecipe; }
            set
            {
                selectedRecipe = value;
                OnPropertyChanged("SelectedRecipe");
            }
        }

        public RecipeViewModel()
        {
            using (CookBookContext db = new CookBookContext())
            {
                var query = db.dbRecipes.Include(x => x.FoodProductsInRecipe).Include(x => x.Steps).ToList().OrderBy(i => i.Name);
                AllRecipes = new ObservableCollection<Recipe>(query);

                AllCuisines = new ObservableCollection<Cuisine>(db.dbCuisines.ToList().OrderBy(i => i.Name));

                var query2 = db.dbProducts.Include(x => x.ProdictInRecipes).ToList().OrderBy(i => i.Name);
                AllProducts = new ObservableCollection<FoodProduct>(query2);

                AllTypes = new ObservableCollection<DishType>(db.dbDishTypes.ToList().OrderBy(i => i.Name));

                Units = new ObservableCollection<Unit> { Unit.шт, Unit.кг, Unit.г, Unit.мл, Unit.л, Unit.ст_л,
                    Unit.ч_л, Unit.ст, Unit.кус };              
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
