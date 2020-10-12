using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace Сookbook_EXAM
{
    public class Recipe : INotifyPropertyChanged
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

        [Required]
        public int? CuisineId { get; set; }
        private Cuisine cuisine;
        public Cuisine Cuisine
        {
            get { return cuisine; }
            set
            {
                cuisine = value;
                OnPropertyChanged("Cuisine");
            }
        }

        [Required]
        public int? DishTypeId { get; set; }
        private DishType dishType;
        public DishType DishType
        {
            get { return dishType; }
            set
            {
                dishType = value;
                OnPropertyChanged("DishType");
            }
        }

        private TimeSpan cookTime;
        public TimeSpan CookTime
        {
            get { return cookTime; }
            set
            {
                cookTime = value;
                OnPropertyChanged("CookTime");
            }
        }

        private byte[] photo;
        public byte[] Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                OnPropertyChanged("Photo");
            }
        }

        private ICollection<Step> steps;
        public ICollection<Step> Steps
        {
            get { return steps; }
            set
            {
                steps = value;
                OnPropertyChanged("Steps");
            }
        }
        private ICollection<FoodProductRecipes> foodProductsInRecipe;
        public ICollection<FoodProductRecipes> FoodProductsInRecipe
        {
            get { return foodProductsInRecipe; }
            set
            {
                foodProductsInRecipe = value;
                OnPropertyChanged("FoodProductsInRecipe");
            }
        }
        public Recipe(Image image)
        {
            FoodProductsInRecipe = new List<FoodProductRecipes>();
            Steps = new List<Step>();
            ImageConverter imageConverter = new ImageConverter();
            Photo = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
        }
        public Recipe()
        {
            FoodProductsInRecipe = new List<FoodProductRecipes>();
            Steps = new List<Step>(); 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
