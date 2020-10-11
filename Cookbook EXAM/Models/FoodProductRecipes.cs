using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Сookbook_EXAM
{
    public class FoodProductRecipes : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }

        private double quantity;
        public double Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private Unit productUnit;
        public Unit ProductUnit
        {
            get { return productUnit; }
            set
            {
                productUnit = value;
                OnPropertyChanged("ProductUnit");
            }
        }

        public int? FoodProductId { get; set; }
        private FoodProduct foodProduct;
        public FoodProduct FoodProduct
        {
            get { return foodProduct; }
            set
            {
                foodProduct = value;
                OnPropertyChanged("FoodProduct");
            }
        }

        public int? RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public FoodProductRecipes()   { }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
