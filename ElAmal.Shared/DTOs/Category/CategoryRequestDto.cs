using System.ComponentModel.DataAnnotations;

namespace ElAmal.Shared.DTOs.Category
{
    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;
    }
}
