using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Сookbook_EXAM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary> 


    public partial class MainWindow : System.Windows.Window
    {
        RecipeViewModel recipeVM;
        MenuItem lastCheckedMI;
        List<string> checkedProducts;
        List<string> checkedTypes;
        List<string> checkedCuisines;

        public MainWindow()
        {
            InitializeComponent();
            recipeVM = new RecipeViewModel();
            DataContext = recipeVM;
            lastCheckedMI = allMI;
            checkedProducts = new List<string>();
            checkedTypes = new List<string>();
            checkedCuisines = new List<string>();
        }


        /// <summary>
        /// commands
        /// </summary>
        private void SaveAs_CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (recipeVM.SelectedRecipe != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = @"DOC (.doc)|*.doc|DOCX (.docx)|*.docx|PDF (.pdf)|*.pdf";
                saveFileDialog.FileName = recipeVM.SelectedRecipe.Name;
                if (saveFileDialog.ShowDialog() == true)
                {
                    RecipesRepository repository = new RecipesRepository(recipeVM);
                    if (saveFileDialog.FilterIndex == 1 || saveFileDialog.FilterIndex == 2)
                        repository.CreateDocDocument(saveFileDialog.FileName);
                    else
                        repository.CreatePdfDocument(saveFileDialog.FileName, selectedRecipe);

                }
            }
        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            addRecipeWindow.DataContext = recipeVM;
            if (addRecipeWindow.ShowDialog() == true)
            {
                RefreshBindings();
                RefreshRecipeCatalog();
            }
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Вы действительно хотите удалить выбранный рецепт?", "Удаление рецепта", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (recipeVM.SelectedRecipe != null)
                {
                    RecipesRepository recipesRepository = new RecipesRepository(recipeVM);
                    recipesRepository.DeleteRecipe();
                    RefreshBindings();
                    RefreshRecipeCatalog();
                    if (selectedRecipe.Visibility == Visibility.Visible)
                        HideSelectedRecipe();
                }
            }
        }

        private void Change_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ChangeRecipeWindow changeRecipeWindow = new ChangeRecipeWindow(recipeVM);
            if (changeRecipeWindow.ShowDialog() == true)
            {
                RefreshBindings();
                RefreshRecipeCatalog();
            }
        }

        /// <summary>
        /// click
        /// </summary>

        private void CatalogMItems_Click(object sender, RoutedEventArgs e)
        {
            lastCheckedMI.IsChecked = false;
            (sender as MenuItem).IsChecked = true;
            lastCheckedMI = (sender as MenuItem);
        }

        private void searchByNameBTN_Click(object sender, RoutedEventArgs e)
        {
            if (recipeSearchCB.SelectedItem != null || recipeSearchCB.Text != null)
            {
                if (recipeSearchCB.SelectedItem != null)
                {
                    RecipesList.ItemsSource = recipeVM.AllRecipes.Where(x => x.Name == recipeSearchCB.SelectedValue.ToString());
                    recipeSearchCB.SelectedItem = null;
                }
                else
                {
                    RecipesList.ItemsSource = recipeVM.AllRecipes.Where(x => x.Name.IndexOf(recipeSearchCB.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    recipeSearchCB.Text = null;
                }
                recipeSearchCB.IsDropDownOpen = false;
            }
        }

        private void searchAllBTN_Click(object sender, RoutedEventArgs e)
        {
            List<Recipe> searchResultResipes = new List<Recipe>();
            foreach (var recipe in recipeVM.AllRecipes)
            {
                if (CheckCuisine(recipe))
                {
                    if (CheckFoodType(recipe))
                    {
                        if (CheckProducts(recipe))
                            searchResultResipes.Add(recipe);
                    }
                }
            }
            RecipesList.ItemsSource = recipeVM.AllRecipes.Where(r => searchResultResipes.Select(s => s.Id).Contains(r.Id));
        }

        private void selectedRecipeOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenSelectedRecipe();
        }

        private void recipeClose_Click(object sender, RoutedEventArgs e)
        {
            HideSelectedRecipe();
        }

        private void SelectedRecipe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenSelectedRecipe();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// checked
        /// </summary>

        private void allMI_Checked(object sender, RoutedEventArgs e)
        {
            RefreshRecipeCatalog();
        }

        private void CuisineMI_Checked(object sender, RoutedEventArgs e)
        {
            RecipesList.ItemsSource = recipeVM.AllRecipes.Where(x => x.Cuisine.Name == (sender as MenuItem).Header.ToString());
        }

        private void TypeMI_Checked(object sender, RoutedEventArgs e)
        {
            RecipesList.ItemsSource = recipeVM.AllRecipes.Where(x => x.DishType.Name == (sender as MenuItem).Header.ToString());
        }

        private bool CheckProductCollection(Recipe recipe, string checkedProduct)
        {
            foreach (var product in recipe.FoodProductsInRecipe)
            {
                if (product.FoodProduct.Name == checkedProduct)
                    return true;
            }
            return false;
        }

        private void SearchChoiseCB_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkedCheckBox = sender as System.Windows.Controls.CheckBox;
            if (checkedCheckBox.Tag.ToString() == "Product")
                checkedProducts.Add(checkedCheckBox.Content.ToString());
            else if (checkedCheckBox.Tag.ToString() == "Cuisine")
                checkedCuisines.Add(checkedCheckBox.Content.ToString());
            else if (checkedCheckBox.Tag.ToString() == "Type")
                checkedTypes.Add(checkedCheckBox.Content.ToString());
        }

        private void SearchChoiseCB_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkedCheckBox = sender as System.Windows.Controls.CheckBox;
            if (checkedCheckBox.Tag.ToString() == "Product")
                checkedProducts.Remove(checkedCheckBox.Content.ToString());
            else if (checkedCheckBox.Tag.ToString() == "Cuisine")
                checkedCuisines.Remove(checkedCheckBox.Content.ToString());
            else if (checkedCheckBox.Tag.ToString() == "Type")
                checkedTypes.Remove(checkedCheckBox.Content.ToString());
        }

        /// <summary>
        /// functions
        /// </summary>

        private bool CheckCuisine(Recipe recipe)
        {
            if (checkedCuisines.Count > 0)
            {
                foreach (string cuisine in checkedCuisines)
                    if (recipe.Cuisine.Name == cuisine)
                        return true;
                return false;
            }
            else return true;
        }

        private bool CheckFoodType(Recipe recipe)
        {
            if (checkedTypes.Count > 0)
            {
                foreach (string type in checkedTypes)
                    if (recipe.DishType.Name == type)
                        return true;
                return false;
            }
            else return true;
        }

        private bool CheckProducts(Recipe recipe)
        {
            if (checkedProducts.Count > 0)
            {
                foreach (string product in checkedProducts)
                {
                    if (!CheckProductCollection(recipe, product))
                        return false;
                }
                return true;
            }
            else return true;
        }

        private void HideSelectedRecipe()
        {
            CatalogDP.Visibility = Visibility.Visible;
            selectedRecipe.Visibility = Visibility.Collapsed;
        }

        private void OpenSelectedRecipe()
        {
            CatalogDP.Visibility = Visibility.Collapsed;
            selectedRecipe.Visibility = Visibility.Visible;
        }

        private void RefreshRecipeCatalog()
        {
            if (recipeVM != null)
            {
                RecipesList.ItemsSource = recipeVM.AllRecipes;
                RecipesList.Items.Refresh();
            }
        }

        private void RefreshBindings()
        {
            recipeVM = new RecipeViewModel();
            typeMI.ItemsSource = recipeVM.AllTypes;
            typesLV.ItemsSource = recipeVM.AllTypes;
            cuisineMI.ItemsSource = recipeVM.AllCuisines;
            cuisinesLV.ItemsSource = recipeVM.AllCuisines;
            productsLV.ItemsSource = recipeVM.AllProducts;
        }

        /// <summary>
        /// other
        /// </summary>

        private void RecipesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            recipeVM.SelectedRecipe = (Recipe)(sender as ListBox).SelectedItem;
        }

        private void recipeSearchCB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (recipeSearchCB.SelectedItem != null)
                recipeSearchCB.SelectedItem = null;
        }

    }
}

