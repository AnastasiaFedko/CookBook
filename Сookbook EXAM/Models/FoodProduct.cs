using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Сookbook_EXAM
{
    public enum Unit {шт, кг, г, мл, л, ст_л, ч_л, ст, кус}
    public class FoodProduct : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

       
        private ICollection<FoodProductRecipes> prodictInRecipes;
        public ICollection<FoodProductRecipes> ProdictInRecipes
        {
            get { return prodictInRecipes; }
            set
            {
                prodictInRecipes = value;
                OnPropertyChanged("ProdictInRecipes");
            }
        }
        public FoodProduct()
        {
            ProdictInRecipes = new ObservableCollection<FoodProductRecipes>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
