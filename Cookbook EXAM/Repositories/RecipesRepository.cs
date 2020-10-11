using System;
using System.Linq;
using System.Data.Entity;
using Microsoft.Office.Interop.Word;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;

namespace Сookbook_EXAM
{
    public class RecipesRepository
    {       
        Recipe selectedRecipe;

        public RecipesRepository(RecipeViewModel rvm)
        {
            selectedRecipe = rvm.SelectedRecipe;
        }

        /// <summary>
        /// save to file
        /// </summary>
       
        public void CreateDocDocument(string fileName)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
                winword.ShowAnimation = false;
                winword.Visible = false;
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                document.Content.SetRange(0, 0);
                document.Content.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                document.Content.Font.Name = "Georgia";
                document.Content.Font.Size = 20;
                document.Content.Bold = 1;
                document.Content.Text = selectedRecipe.Name + Environment.NewLine;

                System.Drawing.Image recipePhoto = ByteArrayToImage(selectedRecipe.Photo);
                Clipboard.SetDataObject(recipePhoto);
                Microsoft.Office.Interop.Word.Paragraph para = document.Content.Paragraphs.Add(ref missing);
                para.Range.Paste();
                para.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                para1.Range.Font.Size = 13;
                para1.Range.Bold = 0;
                para1.Range.Text = $"Кухня: {selectedRecipe.Cuisine.Name}";
                para1.Range.InsertParagraphAfter();
                
                Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
                para2.Range.Font.Size = 13;
                para2.Range.Bold = 0;
                para2.Range.Text = $"Тип блюда: {selectedRecipe.DishType.Name}";
                para2.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Paragraph para3 = document.Content.Paragraphs.Add(ref missing);
                para3.Range.Font.Size = 13;
                para3.Range.Bold = 0;
                para3.Range.Text = $"Время приготовления: {selectedRecipe.CookTime.ToString(@"hh\:mm")}";
                para3.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Paragraph para4 = document.Content.Paragraphs.Add(ref missing);
                para4.Range.Font.Size = 15;
                para4.Range.Bold = 1;
                para4.Range.Text = "Продукты";
                para4.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Paragraph para5 = document.Content.Paragraphs.Add(ref missing);
                para5.Range.Bold = 0;
                para5.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Paragraph para6 = document.Content.Paragraphs.Add(ref missing);
                para6.Range.Font.Size = 15;
                para6.Range.Bold = 1;
                para6.Range.Text = "Приготовление";
                para6.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Table productTable = document.Tables.Add(para5.Range, selectedRecipe.FoodProductsInRecipe.Count, 3, ref missing, ref missing);
                document.Tables[1].Range.Font.Size = 12;
                document.Tables[1].Range.Bold = 0;
                document.Tables[1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                productTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitFixed);
                productTable.Borders.Enable = 1;
                productTable.Borders.OutsideColor = WdColor.wdColorGray05;
                productTable.Borders.InsideColor = WdColor.wdColorGray20;
                int index = 1;
                foreach (var product in selectedRecipe.FoodProductsInRecipe)
                {
                    productTable.Rows[index].Cells[1].Range.Text = product.FoodProduct.Name;
                    productTable.Rows[index].Cells[2].Range.Text = product.Quantity.ToString();
                    productTable.Rows[index].Cells[3].Range.Text = product.ProductUnit.ToString();
                    index++;
                }

                Microsoft.Office.Interop.Word.Table stepsTable = document.Tables.Add(para1.Range, selectedRecipe.Steps.Count, 2, ref missing, ref missing);
                document.Tables[2].Range.Font.Size = 12;
                document.Tables[2].Range.Bold = 0;
                document.Tables[2].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                document.Tables[2].Columns[1].Width = 90;
                document.Tables[2].Columns[2].Width = 375;

                stepsTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitFixed);
                stepsTable.Borders.Enable = 0;

                int indexstep = 1;
                foreach (var step in selectedRecipe.Steps)
                {
                    System.Drawing.Image stepPhoto = ByteArrayToImage(step.Photo);
                    Clipboard.SetDataObject(stepPhoto);
                    stepsTable.Rows[indexstep].Cells[1].Range.Paste();
                    stepsTable.Rows[indexstep].Cells[2].Range.Text = step.Description;
                    indexstep++;
                }

                object filename = fileName;
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                System.Windows.MessageBox.Show("Рецепт сохранен в файл!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void CreatePdfDocument(string fileName, Grid recipe)
        {
            try
            {
                MigraDoc.DocumentObjectModel.Document document = CreateDocument();
                document.UseCmykColor = true;
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(fileName);
                System.Windows.MessageBox.Show("Рецепт сохранен в файл!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private MigraDoc.DocumentObjectModel.Document CreateDocument()
        {

            MigraDoc.DocumentObjectModel.Document document = new MigraDoc.DocumentObjectModel.Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            MigraDoc.DocumentObjectModel.Paragraph paragraph = section.AddParagraph();
            section.PageSetup.BottomMargin = 20;
            section.PageSetup.TopMargin = 20;
            paragraph.Format.Font.Color = Colors.Black;
            paragraph.Format.Font.Size = 20;
            paragraph.Format.Font.Name = "Georgia";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddFormattedText(selectedRecipe.Name, TextFormat.Bold);
            paragraph = section.AddParagraph();

            MigraDoc.DocumentObjectModel.Shapes.Image image = section.AddImage(MigraDocFilenameFromByteArray(selectedRecipe.Photo));
            image.Width = "10cm";
            image.Left = ShapePosition.Center;
            paragraph = section.AddParagraph();

            paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Name = "Georgia";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddFormattedText($"Кухня: {selectedRecipe.Cuisine.Name}");

            paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Name = "Georgia";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddFormattedText($"Тип блюда: {selectedRecipe.DishType.Name}");

            paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 13;
            paragraph.Format.Font.Name = "Georgia";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddFormattedText($"Время приготовления: {selectedRecipe.CookTime.ToString(@"hh\:mm")}");

            paragraph = section.AddParagraph();
            paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 15;
            paragraph.Format.Font.Name = "Georgia";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddFormattedText("Продукты:", TextFormat.Bold);

            paragraph = section.AddParagraph();

            MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            table.Borders.Width = 0.1;
            table.Borders.Color = Colors.GhostWhite;
            table.Format.Font.Name = "Georgia";
            table.Format.Font.Size = 12;
            table.Format.Alignment = ParagraphAlignment.Center;
            table.Rows.Alignment = RowAlignment.Center;
            MigraDoc.DocumentObjectModel.Tables.Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(6));
            column.Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));

            foreach (var product in selectedRecipe.FoodProductsInRecipe)
            {
                MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
                row.VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
                row.HeightRule = RowHeightRule.Auto;

                MigraDoc.DocumentObjectModel.Tables.Cell cell = row.Cells[0];
                cell.AddParagraph(product.FoodProduct.Name);
                cell = row.Cells[1];
                cell.AddParagraph(product.Quantity.ToString());
                cell = row.Cells[2];
                cell.AddParagraph(product.ProductUnit.ToString());
            }
            table.SetEdge(0, 0, 3, selectedRecipe.FoodProductsInRecipe.Count, Edge.Interior, BorderStyle.Single, 0.75, Colors.LightGray);
            section.Add(table);

            paragraph = section.AddParagraph();

            paragraph = section.AddParagraph();
            paragraph.Format.Font.Size = 15;
            paragraph.Format.Font.Name = "Georgia";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddFormattedText("Приготовление:", TextFormat.Bold);

            paragraph = section.AddParagraph();

            MigraDoc.DocumentObjectModel.Tables.Table tableSteps = new MigraDoc.DocumentObjectModel.Tables.Table();
            tableSteps.Borders.Width = 0;
            tableSteps.Format.Font.Name = "Georgia";
            tableSteps.Format.Font.Size = 12;
            tableSteps.Format.Alignment = ParagraphAlignment.Center;
            tableSteps.Rows.Alignment = RowAlignment.Center;
            MigraDoc.DocumentObjectModel.Tables.Column columnStep = tableSteps.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
            columnStep.Format.Alignment = ParagraphAlignment.Center;
            tableSteps.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(12));

            foreach (var step in selectedRecipe.Steps)
            {
                MigraDoc.DocumentObjectModel.Tables.Row row = tableSteps.AddRow();
                row.VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
                row.HeightRule = RowHeightRule.AtLeast;

                MigraDoc.DocumentObjectModel.Tables.Cell cell = row.Cells[0];

                MigraDoc.DocumentObjectModel.Shapes.Image imageStep = cell.AddImage(MigraDocFilenameFromByteArray(step.Photo));
                imageStep.Width = "3 cm";
                image.Left = ShapePosition.Center;

                cell = row.Cells[1];
                cell.AddParagraph(step.Description);
            }
            section.Add(tableSteps);
            return document;
        }

        /// <summary>
        /// delete recipe
        /// </summary>       

        public void DeleteRecipe()
        {
            using (CookBookContext db = new CookBookContext())
            {
                Recipe recipe = db.dbRecipes.Include(r => r.Steps).Include(r => r.FoodProductsInRecipe).FirstOrDefault(r => r.Id == selectedRecipe.Id);

                db.dbSteps.RemoveRange(db.dbSteps.Where(s => s.RecipeId == recipe.Id));
                db.SaveChanges();

                db.dbProductsForRecipe.RemoveRange(db.dbProductsForRecipe.Where(s => s.RecipeId == recipe.Id));
                db.SaveChanges();

                db.dbRecipes.Remove(recipe);
                db.SaveChanges();
                Cuisine cuisine = db.dbCuisines.FirstOrDefault(c => c.Id == selectedRecipe.CuisineId);
                if (cuisine.CuisineRecipes.Count == 0)
                {
                    db.dbCuisines.Remove(cuisine);
                    db.SaveChanges();
                }
                DishType dishType = db.dbDishTypes.FirstOrDefault(d => d.Id == selectedRecipe.DishTypeId);
                if (dishType.TypeRecipes.Count == 0)
                {
                    db.dbDishTypes.Remove(dishType);
                    db.SaveChanges();
                }              

                foreach (var product in selectedRecipe.FoodProductsInRecipe)
                {
                    FoodProduct foodProduct = db.dbProducts.Include(x => x.ProdictInRecipes).FirstOrDefault(p => p.Id == product.FoodProduct.Id);
                    if (foodProduct.ProdictInRecipes.Count == 0)
                    {
                        db.dbProducts.Remove(foodProduct);
                        db.SaveChanges();
                    }
                }
                db.dbSteps.RemoveRange(db.dbSteps.Where(s => s.RecipeId == recipe.Id));
                db.SaveChanges();
            }
        }

        /// <summary>
        /// functions
        /// </summary>

        static string MigraDocFilenameFromByteArray(byte[] image)
        {
            return "base64:" + Convert.ToBase64String(image);
        }

        public System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                return returnImage;
            }
        }
    }
}

