using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Сookbook_EXAM
{
    public class Cuisine
    {
        [Key]
        public int Id { get; set; }
        public string Name{ get; set; }
        public ICollection<Recipe> CuisineRecipes { get; set; }
        public Cuisine()
        {
            CuisineRecipes = new ObservableCollection<Recipe>();
        }
    }
}
