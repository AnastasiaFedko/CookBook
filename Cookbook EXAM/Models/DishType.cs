using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Сookbook_EXAM
{
    public class DishType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Recipe> TypeRecipes { get; set; }
        public DishType()
        {
            TypeRecipes = new ObservableCollection<Recipe>();
        }
    }
}
