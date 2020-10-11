using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Сookbook_EXAM
{
    public class Step
    {
        [Key]
        public int Id{ get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }      
        public int? RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public Step(System.Drawing.Image image)
        {
            ImageConverter imageConverter = new ImageConverter();
            Photo = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
        }
        public Step() { }
    }
}
