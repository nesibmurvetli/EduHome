
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public bool IsDeactive { get; set; }
    }
}
