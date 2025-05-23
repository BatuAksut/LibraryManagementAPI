using System.ComponentModel.DataAnnotations;

namespace API.Models.Dtos
{
    public class AddBookDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int CategoryId { get; set; }

    
        public Guid? ImageId { get; set; }
    }
}
