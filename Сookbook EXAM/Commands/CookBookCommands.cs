using System.Windows.Input;

namespace Сookbook_EXAM
{
    class CookBookCommands
    {
        public static RoutedCommand Exit { get; }
        public static RoutedCommand AddRecipe { get; }
        public static RoutedCommand DeleteRecipe { get; }
        public static RoutedCommand ChangeRecipe { get; }     
        static CookBookCommands()
        {
            Exit = new RoutedCommand("Exit", typeof(MainWindow));
            AddRecipe = new RoutedCommand("AddRecipe", typeof(MainWindow));
            DeleteRecipe = new RoutedCommand("DeleteRecipe", typeof(MainWindow));
            ChangeRecipe = new RoutedCommand("ChangeRecipe", typeof(MainWindow));          
        }
    }
}
