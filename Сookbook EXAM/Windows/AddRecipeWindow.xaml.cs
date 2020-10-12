using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Windows.Controls;

namespace Сookbook_EXAM
{
    /// <summary>
    /// Логика взаимодействия для AddRecipeWindow.xaml
    /// </summary>
    public partial class AddRecipeWindow : Window
    {
        ObservableCollection<FoodProductRecipes> addRecipeProducts;
        ObservableCollection<Step> addRecipeSteps;
        ObservableCollection<FoodProductRecipes> newProducts;

        public AddRecipeWindow()
        {
            InitializeComponent();
            addRecipeProducts = new ObservableCollection<FoodProductRecipes>();
            addRecipeSteps = new ObservableCollection<Step>();
            newProducts = new ObservableCollection<FoodProductRecipes>();
        }

        /// <summary>
        /// click
        /// </summary>
        
        private void addProductBT_Click(object sender, RoutedEventArgs e)
        {
            if (quantityProductDUD.Text != "" && quantityProductDUD.Text != "0" && unitCB.SelectedItem != null &&
                (nameProductCB.SelectedItem != null || nameProductCB.Text != null))
            {
                FoodProductRecipes foodProductRecires = new FoodProductRecipes();
                foodProductRecires.Quantity = Convert.ToDouble(quantityProductDUD.Text);
                foodProductRecires.ProductUnit = (Unit)unitCB.SelectedItem;

               
                if (nameProductCB.SelectedItem != null)
                {
                    FoodProduct foodProduct = new FoodProduct();
                    foodProduct.Name = (nameProductCB.SelectedItem as FoodProduct).Name;
                    foodProduct.Id = (nameProductCB.SelectedItem as FoodProduct).Id;
                    foodProductRecires.FoodProduct = foodProduct;
                    nameProductCB.SelectedItem = null;
                }
                else if (nameProductCB.Text != null)
                {
                    FoodProduct newProduct = new FoodProduct { Name = nameProductCB.Text };
                    foodProductRecires.FoodProduct = newProduct;
                    nameProductCB.Text = "";
                    newProducts.Add(foodProductRecires);     
                }           

                addRecipeProducts.Add(foodProductRecires);
                selectedProductsDG.ItemsSource = addRecipeProducts;
                selectedProductsDG.Items.Refresh();
                quantityProductDUD.Text = "";
            }
            else
                System.Windows.MessageBox.Show("Заполните название, количество и единицу измерения продукта!");
        }

        private void removeProductBT_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProductsDG.SelectedItem != null)
            {
                if (newProducts.Contains(selectedProductsDG.SelectedItem as FoodProductRecipes))
                    newProducts.Remove(selectedProductsDG.SelectedItem as FoodProductRecipes);
                addRecipeProducts.Remove(selectedProductsDG.SelectedItem as FoodProductRecipes);
                selectedProductsDG.Items.Refresh();
            }
        }

        private void addImageBTN_Click(object sender, RoutedEventArgs e)
        {
            AddImage(imagePathTB);
        }

        private void addRecipeImageBTN_Click(object sender, RoutedEventArgs e)
        {
            AddImage(imageRecipePathTB);
        }

        private void removeRecipeStepBT_Click(object sender, RoutedEventArgs e)
        {
            if (newStepsDG.SelectedItem != null)
            {
                addRecipeSteps.Remove(newStepsDG.SelectedItem as Step);
                newStepsDG.Items.Refresh();
            }
        }

        private void addRecipeStep_Click(object sender, RoutedEventArgs e)
        {
            if (stepDescriptionTBX.Text != "")
            {
                if (imagePathTB.Text != "")
                {
                    if (File.Exists(imagePathTB.Text))
                    {
                        Step newStep = new Step(new Bitmap(imagePathTB.Text)) { Description = stepDescriptionTBX.Text };
                        addRecipeSteps.Add(newStep);
                        newStepsDG.ItemsSource = addRecipeSteps;
                        newStepsDG.Items.Refresh();
                        imagePathTB.Text = "";
                        stepDescriptionTBX.Text = "";
                    }
                    else
                        System.Windows.MessageBox.Show("Выбранное изображение не существует!");
                }
            }
            else
                System.Windows.MessageBox.Show("Заполните описание шага в приготовлении рецепта и выберите изображение!");
        }

        private void addNewRecipeBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckForEmptyFields())
                System.Windows.MessageBox.Show("Все поля должны быть заполнены!");
            else
            {
                using (CookBookContext db = new CookBookContext())
                {
                    DishType dishType;
                    if (newTypeTBX.Text != "")
                    {
                        dishType = new DishType { Name = newTypeTBX.Text };
                        db.dbDishTypes.Add(dishType);
                        db.SaveChanges();
                    }
                    else
                    {
                        DishType selectedType = oldTypesCB.SelectedItem as DishType;
                        dishType = db.dbDishTypes.FirstOrDefault(d => d.Id == selectedType.Id);
                    }

                    Cuisine cuisine;
                    if (newCuisineTBX.Text != "")
                    {
                        cuisine = new Cuisine { Name = newCuisineTBX.Text };
                        db.dbCuisines.Add(cuisine);
                        db.SaveChanges();
                    }
                    else
                    {
                        Cuisine selectedCuisine = oldCuisinesCB.SelectedItem as Cuisine;
                        cuisine = db.dbCuisines.FirstOrDefault(c => c.Id == selectedCuisine.Id);
                    }

                    Recipe recipe = new Recipe(new Bitmap(imageRecipePathTB.Text)) { Name = recipeNameTBX.Text, Cuisine = cuisine, DishType = dishType, };

                    if (newProducts.Count > 0)
                    {
                        foreach (var fpr in newProducts)
                        {
                            FoodProduct product = new FoodProduct { Name = fpr.FoodProduct.Name };  
                            db.dbProducts.Add(product);
                            db.SaveChanges();

                            fpr.FoodProduct = db.dbProducts.FirstOrDefault(p => p.Name == product.Name);
                            fpr.Recipe = recipe;
                            recipe.FoodProductsInRecipe.Add(fpr);

                            addRecipeProducts.Remove(addRecipeProducts.FirstOrDefault(p => p.FoodProduct.Name == product.Name));
                        }
                    }
                    foreach (var prod in addRecipeProducts)
                    {
                        prod.FoodProduct = db.dbProducts.FirstOrDefault(p => p.Id == prod.FoodProduct.Id);                     
                        prod.Recipe = recipe;
                        recipe.FoodProductsInRecipe.Add(prod);
                    }

                    int num = 1;
                    foreach (var step in addRecipeSteps)
                    {
                        Step recipeStep = step;
                        recipeStep.Number = num++;
                        recipeStep.Recipe = recipe;
                        recipe.Steps.Add(recipeStep);
                    }
                    recipe.CookTime = DateTimeToTimeSpan(cookTimeTP.Value);                                   
                    db.dbRecipes.Add(recipe);
                    db.SaveChanges();
                    DialogResult = true;
                }
            }

        }

        /// <summary>
        /// events
        /// </summary>

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            oldCuisineRB.IsChecked = true;
            oldTypeRB.IsChecked = true;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// checked
        /// </summary>       

        private void typeRB_Checked(object sender, RoutedEventArgs e)
        {
            if (oldTypeRB.IsChecked == true)
            {
                oldTypesCB.IsEnabled = true;
                newTypeTBX.IsEnabled = false;
                newTypeTBX.Text = "";
            }
            else
            {
                oldTypesCB.IsEnabled = false;
                newTypeTBX.IsEnabled = true;
                oldTypesCB.SelectedItem = null;
            }

        }

        private void cuisineRB_Checked(object sender, RoutedEventArgs e)
        {
            if (oldCuisineRB.IsChecked == true)
            {
                oldCuisinesCB.IsEnabled = true;
                newCuisineTBX.IsEnabled = false;
                newCuisineTBX.Text = "";
            }
            else
            {
                oldCuisinesCB.IsEnabled = false;
                newCuisineTBX.IsEnabled = true;
                oldCuisinesCB.SelectedItem = null;
            }
        }

        /// <summary>
        /// functions
        /// </summary>           

        private bool CheckForEmptyFields()
        {
            return (recipeNameTBX.Text == "" ||
                   (oldCuisineRB.IsChecked == true && oldCuisinesCB.SelectedItem == null) ||
                   (newCuisineRB.IsChecked == true && newCuisineTBX.Text == "") ||
                   (oldTypeRB.IsChecked == true && oldTypesCB.SelectedItem == null) ||
                   (newTypeRB.IsChecked == true && newTypeTBX.Text == "") ||
                   addRecipeSteps.Count == 0 || addRecipeProducts.Count == 0 ||
                   imageRecipePathTB.Text == ""
                   //|| cookTimeTP.Text == "0:00" ||
                   //cookTimeTP.Text == null
                   ) ? false : true;
        }

        private void AddImage(TextBox imagePathControl)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"JPEG | *.jpg; *.jpeg; |BMP | *.bmp|PNG | *.png";

            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                imagePathControl.Text = openFileDialog.FileName;
            }
        }

        private TimeSpan DateTimeToTimeSpan(DateTime? ts)
        {
            if (!ts.HasValue) return TimeSpan.Zero;
            else
            {
                if (ts.Value.Minute < 10)
                {
                    string minute = $"0{ts.Value.Minute}";
                    return new TimeSpan(ts.Value.Hour, Convert.ToInt32(minute), ts.Value.Second);
                }
                else
                    return new TimeSpan(ts.Value.Hour, ts.Value.Minute, ts.Value.Second);
            }
        }

        /// <summary>
        /// other
        /// </summary>

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
