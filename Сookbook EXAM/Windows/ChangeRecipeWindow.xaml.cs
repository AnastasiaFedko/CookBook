using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Drawing;
using System.Globalization;

namespace Сookbook_EXAM
{
    /// <summary>
    /// Логика взаимодействия для ChangeRecipeWindow.xaml
    /// </summary>
    public partial class ChangeRecipeWindow : Window
    {
        ObservableCollection<FoodProductRecipes> addRecipeProducts;
        ObservableCollection<Step> addRecipeSteps;
        ObservableCollection<FoodProductRecipes> newProducts;
        object dgProductLastSelectedItem;
        public ChangeRecipeWindow(RecipeViewModel recipeViewModel)
        {
            InitializeComponent();
            DataContext = recipeViewModel;

            imageRecipePathTB.Text = FilenameFromByteArray(recipeViewModel.SelectedRecipe.Photo);
            cookTimeTP.Value = TimeSpanToDateTime(recipeViewModel.SelectedRecipe.CookTime);



            addRecipeProducts = new ObservableCollection<FoodProductRecipes>();
            addRecipeSteps = new ObservableCollection<Step>();
            newProducts = new ObservableCollection<FoodProductRecipes>();
        }
        private void addProductBT_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void removeProductBT_Click(object sender, RoutedEventArgs e)
        {
            
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
           
        }

        private void addRecipeStep_Click(object sender, RoutedEventArgs e)
        {
          
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

            oldTypesCB.SelectedItem = oldTypesCB.Items.Cast<DishType>().FirstOrDefault(x => x.Id == (DataContext as RecipeViewModel).SelectedRecipe.DishType.Id);
            oldCuisinesCB.SelectedItem = oldCuisinesCB.Items.Cast<Cuisine>().FirstOrDefault(x => x.Id == (DataContext as RecipeViewModel).SelectedRecipe.Cuisine.Id);
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

        public string FilenameFromByteArray(byte[] image)
        {
            return "base64:" + Convert.ToBase64String(image);
        }

        public static DateTime? TimeSpanToDateTime(TimeSpan ts)
        {
            DateTime? FResult = null;
            try
            {
                string year = string.Format("{0:0000}", DateTime.MinValue.Date.Year);
                string month = string.Format("{0:00}", DateTime.MinValue.Date.Month);
                string day = string.Format("{0:00}", DateTime.MinValue.Date.Day);

                string hours = string.Format("{0:00}", ts.Hours);
                string minutes = string.Format("{0:00}", ts.Minutes);
                string seconds = string.Format("{0:00}", ts.Seconds);

                string dSep = "-"; string tSep = ":"; string dtSep = "T";

                // yyyy-mm-ddTHH:mm:ss
                string dtStr = string.Concat(year, dSep, month, dSep, day, dtSep, hours, tSep, minutes, tSep, seconds);

                DateTime dt;
                if (DateTime.TryParseExact(dtStr, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt))
                {
                    FResult = dt;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }

            return FResult;
        }

        /// <summary>
        /// other
        /// </summary>

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void recipeStepsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (recipeStepsDG.SelectedItem != null)
            {
                imagePathTB.Text = FilenameFromByteArray((recipeStepsDG.SelectedItem as Step).Photo);
                stepDescriptionTBX.Text = (recipeStepsDG.SelectedItem as Step).Description;
                addStepImageBTN.Content = "Изменить изображение";
                addRecipeStepBTN.Content = "Изменить шаг";
            }
        }

        private void recipeStepsDG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imagePathTB.Text = null;
            stepDescriptionTBX.Text = null;
            addStepImageBTN.Content = "Добавить изображение";
            addRecipeStepBTN.Content = "Добавить шаг";
        }

        private void recipeProductsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (recipeProductsDG.SelectedItem != null)
            {
                quantityProductDUD.Value = (decimal)(recipeProductsDG.SelectedItem as FoodProductRecipes).Quantity;
                nameProductCB.SelectedItem = nameProductCB.Items.Cast<FoodProduct>().FirstOrDefault(x => x.Id == (recipeProductsDG.SelectedItem as FoodProductRecipes).FoodProduct.Id);
                unitCB.SelectedItem = unitCB.Items.Cast<Unit>().FirstOrDefault(x => x.ToString() == (recipeProductsDG.SelectedItem as FoodProductRecipes).ProductUnit.ToString());
                addProductBT.Content = "Изменить продукт";
                dgProductLastSelectedItem = recipeProductsDG.SelectedItem;

            }
        }

        private void recipeProductsDG_MouseDown(object sender, MouseButtonEventArgs e)
        {
                recipeProductsDG.SelectedItem = null;
                quantityProductDUD.Value = null;
                nameProductCB.SelectedItem = null;
                unitCB.SelectedItem = null;
                addProductBT.Content = "Добавить продукт";
        }
    }
}
