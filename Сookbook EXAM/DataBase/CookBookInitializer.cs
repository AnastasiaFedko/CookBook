using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;

namespace Сookbook_EXAM
{
    public class CookBookInitializer : DropCreateDatabaseAlways<CookBookContext>
    //public class CookBookInitializer : CreateDatabaseIfNotExists<CookBookContext>
    {
        protected override void Seed(CookBookContext db)
        {
            FoodProduct product1 = new FoodProduct { Name = "Колбаса" };
            FoodProduct product2 = new FoodProduct { Name = "Хлеб" };
            FoodProduct product3 = new FoodProduct { Name = "Сухое красное вино" };
            FoodProduct product4 = new FoodProduct { Name = "Лимон" };
            FoodProduct product5 = new FoodProduct { Name = "Сахар" };
            FoodProduct product6 = new FoodProduct { Name = "Коньяк" };
            FoodProduct product7 = new FoodProduct { Name = "Газированная вода" };
            FoodProduct product8 = new FoodProduct { Name = "Апельсин" };
            FoodProduct product9 = new FoodProduct { Name = "Масло растительное" };
            FoodProduct product10 = new FoodProduct { Name = "Яйцо" };
            FoodProduct product11 = new FoodProduct { Name = "Соль" };
            FoodProduct product12 = new FoodProduct { Name = "Горчица" };
            FoodProduct product13 = new FoodProduct { Name = "Уксус" };
            FoodProduct product14 = new FoodProduct { Name = "Мука" };
            FoodProduct product15 = new FoodProduct { Name = "Масло сливочное" };
            FoodProduct product16 = new FoodProduct { Name = "Молоко" };
            FoodProduct product17 = new FoodProduct { Name = "Дрожжи сухие" };
            FoodProduct product18 = new FoodProduct { Name = "Лук репчатый" };
            FoodProduct product19 = new FoodProduct { Name = "Мясо копченое" };
            FoodProduct product20 = new FoodProduct { Name = "Тмин" };
            FoodProduct product21 = new FoodProduct { Name = "Сливки" };


            db.dbProducts.AddRange(new List<FoodProduct> {product1, product2, product3, product4, product5, product6, product7, product8, product9, product10,
            product11, product12, product13, product14, product15, product16, product17, product18, product19, product20, product21});
            db.SaveChanges();

            Cuisine cuisine1 = new Cuisine { Name = "Украинская" };
            Cuisine cuisine2 = new Cuisine { Name = "Французская" };
            Cuisine cuisine3 = new Cuisine { Name = "Испанская" };
            Cuisine cuisine4 = new Cuisine { Name = "Немецкая" };
            Cuisine cuisine5 = new Cuisine { Name = "Японская" };

            db.dbCuisines.AddRange(new List<Cuisine> { cuisine1, cuisine2, cuisine3, cuisine4, cuisine5 });
            db.SaveChanges();

            DishType dishType1 = new DishType { Name = "Закуски" };
            DishType dishType2 = new DishType { Name = "Основные блюда" };
            DishType dishType3 = new DishType { Name = "Десерты" };
            DishType dishType4 = new DishType { Name = "Соусы" };
            DishType dishType5 = new DishType { Name = "Напитки" };

            db.dbDishTypes.AddRange(new List<DishType> { dishType1, dishType2, dishType3, dishType4, dishType5 });        
            db.SaveChanges();


            Recipe recipe1 = new Recipe(new Bitmap (@"..\..\Image\Бутерброд.jpg")) { Name = "Бутерброд с колбасой", Cuisine = cuisine1, DishType = dishType1, CookTime = new TimeSpan(0, 02, 0) };
                                                     
            recipe1.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product2, Quantity = 1, ProductUnit = (Unit)8 });
            recipe1.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product1, Quantity = 1, ProductUnit = (Unit)8 });
            recipe1.Steps.Add(new Step(new Bitmap(@"..\..\Image\Хлеб.jpg")) { Number = 1, Description = "Отрезать 1 кусок хлеба." });
            recipe1.Steps.Add(new Step(new Bitmap(@"..\..\Image\Колбаса.jpg")) { Number = 2, Description = "Отрезать 1 кусок колбасы." });
            recipe1.Steps.Add(new Step(new Bitmap(@"..\..\Image\Бутерброд.jpg")) { Number = 3, Description = "Положить кусок колбасы на кусок хлеба. Готово!" });

            Recipe recipe2 = new Recipe(new Bitmap(@"..\..\Image\Sangria.jpg")) { Name = "Сангрия", Cuisine = cuisine3, DishType = dishType5, CookTime = new TimeSpan(0, 8, 5) };   
            recipe2.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product3, Quantity = 750, ProductUnit = (Unit)3 });
            recipe2.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product4, Quantity = 1, ProductUnit = (Unit)0 });
            recipe2.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product5, Quantity = 2, ProductUnit = (Unit)5 });
            recipe2.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product6, Quantity = 40, ProductUnit = (Unit)3 });
            recipe2.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product7, Quantity = 450, ProductUnit = (Unit)3 });
            recipe2.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product8, Quantity = 1, ProductUnit = (Unit)0 });
            recipe2.Steps.Add(new Step(new Bitmap(@"..\..\Image\Sangria1.jpg"))
            {
                Number = 1,
                Description = "Подготовьте все ингредиенты для приготовления классической сангрии. " +
                "Нарежьте фрукты круглыми дольками и удалите из них все косточки. Вино необходимо заранее охладить."
            });
            recipe2.Steps.Add(new Step(new Bitmap(@"..\..\Image\Sangria2.jpg"))
            {
                Number = 2,
                Description = "Возьмите большой кувшин для напитков, налейте в него вино. " +
                "Туда же выжмите сок из долек лимона и апельсина, и бросьте их внутрь."
            });
            recipe2.Steps.Add(new Step(new Bitmap(@"..\..\Image\Sangria3.jpg"))
            {
                Number = 3,
                Description = "Добавьте сахар и бренди (коньяк). Осторожно помешайте до тех пор, " +
                "пока сахар не растворится. Накройте кувшин полиэтиленовой пленкой и охладите его в холодильнике в течение примерно 8 часов, чтобы фрукты отдали весь " +
                "свой вкус и аромат."
            });
            recipe2.Steps.Add(new Step(new Bitmap(@"..\..\Image\Sangria4.jpg"))
            {
                Number = 4,
                Description = "Добавьте имбирный эль или содовую непосредственно перед подачей напитка на стол. " +
                "Украсьте бокалы дольками лимона, лайма или апельсина. Приятной вечеринки!"
            });

            Recipe recipe3 = new Recipe(new Bitmap(@"..\..\Image\Майонез.jpg")) { Name = "Майонез", Cuisine = cuisine2, DishType = dishType4, CookTime = new TimeSpan(0, 10, 0) };     
            recipe3.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product5, Quantity = 1, ProductUnit = (Unit)6 });
            recipe3.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product9, Quantity = 1, ProductUnit = (Unit)3 });
            recipe3.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product10, Quantity = 200, ProductUnit = (Unit)0 });
            recipe3.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product11, Quantity = 1, ProductUnit = (Unit)0 });
            recipe3.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product12, Quantity = 0.5, ProductUnit = (Unit)6 });
            recipe3.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product13, Quantity = 1, ProductUnit = (Unit)5 });
            recipe3.Steps.Add(new Step(new Bitmap(@"..\..\Image\Майонез1.jpg")) { Number = 1, Description = "Наливаем в миску растительное масло." });
            recipe3.Steps.Add(new Step(new Bitmap(@"..\..\Image\Майонез2.jpg")) { Number = 2, Description = "Добавляем сахар, соль, уксус, горчицу." });
            recipe3.Steps.Add(new Step(new Bitmap(@"..\..\Image\Майонез3.jpg")) { Number = 3, Description = "Добавляем яйцо." });
            recipe3.Steps.Add(new Step(new Bitmap(@"..\..\Image\Майонез4.jpg"))
            {
                Number = 4,
                Description = "Взбиваем все ингредиенты погружным блендером до загустения, " +
                "примерно 1 минуту. Приятного аппетита!"
            });

            Recipe recipe4 = new Recipe(new Bitmap(@"..\..\Image\Луковый пирог.jpg")) { Name = "Луковый пирог с копченым мясом", Cuisine = cuisine4, DishType = dishType2, CookTime = new TimeSpan(1, 50, 0) };
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product5, Quantity = 0.5, ProductUnit = (Unit)6 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product10, Quantity = 1, ProductUnit = (Unit)0 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product11, Quantity = 1, ProductUnit = (Unit)6 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product14, Quantity = 125, ProductUnit = (Unit)2 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product15, Quantity = 60, ProductUnit = (Unit)2 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product16, Quantity = 75, ProductUnit = (Unit)3 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product17, Quantity = 3, ProductUnit = (Unit)3 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product18, Quantity = 250, ProductUnit = (Unit)2 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product19, Quantity = 50, ProductUnit = (Unit)2 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product20, Quantity = 0.25, ProductUnit = (Unit)6 });
            recipe4.FoodProductsInRecipe.Add(new FoodProductRecipes { FoodProduct = product21, Quantity = 50, ProductUnit = (Unit)3 });

            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог1.jpg")) { Number = 1, Description = "Подготовьте все необходимые ингредиенты для приготовления лукового пирога с копчёным мясом." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог2.jpg")) { Number = 2, Description = "В тёплое молоко всыпьте соль, сахар и дрожжи. Перемешайте." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог3.jpg")) { Number = 3, Description = "Муку пересыпьте в миску, влейте опару и растопленное и остывшее сливочное масло." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог4.jpg")) { Number = 4, Description = "Замесите тесто, соберите его в шар и, накрыв миску чистым кухонным полотенцем, поставьте в тёплое место для расстойки на 1 час." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог5.jpg")) { Number = 5, Description = "За время расстойки теста приготовьте начинку для пирога.Для этого почистите и нарежьте небольшими кубиками репчатый лук." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог6.jpg")) { Number = 6, Description = "Сливочное масло растопите в сковороде или сотейнике, отправьте туда лук и готовьте на среднем огне до мягкости лука и его прозрачности. Лук не должен стать румяным, его ни в коем случае не следует обжаривать, он скорее должен протушиться. Посолите лук во время приготовления." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог7.jpg")) { Number = 7, Description = "Когда лук станет прозрачным и мягким, присыпьте его мукой." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог8.jpg")) { Number = 8, Description = "Лук перемешайте, прогрейте минуту и снимите с плиты." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог9.jpg")) { Number = 9, Description = "Любое копчёное мясо нарежьте соломкой." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог10.jpg")) { Number = 10, Description = "Яйцо и сливки соедините в миске, перемешайте вилкой." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог11.jpg")) { Number = 11, Description = "В другой миске соедините лук, 3/4 всего объёма копчёного мяса, яично-сливочную смесь и тмин." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог12.jpg")) { Number = 12, Description = "Перемешайте. Начинка для лукового пирога готова." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог13.jpg")) { Number = 13, Description = "Подошедшее тесто обомните." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог14.jpg")) { Number = 14, Description = "Тесто раскатайте в пласт и выложите его в форму, формируя бортик высотой примерно 1,5-2 см. (Я выпекала пирог в силиконовой форме диаметром 24 см.)" });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог15.jpg")) { Number = 15, Description = "Начинку распределите поверх теста." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог16.jpg")) { Number = 16, Description = "Сверху разложите оставшееся копчёное мясо и кусочки сливочного масла." });
            recipe4.Steps.Add(new Step(new Bitmap(@"..\..\Image\Луковый пирог17.jpg")) { Number = 17, Description = "Запекайте луковый пирог в духовке,предварительно разогретой до 180 градусов, 40 минут." });
            db.dbRecipes.AddRange(new List<Recipe> { recipe1, recipe2, recipe3, recipe4 });

            db.SaveChanges();
        }
    }
}
