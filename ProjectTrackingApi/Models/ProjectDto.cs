using System.ComponentModel.DataAnnotations;

namespace ProjectTrackingApi.Models
{
    public class ProjectDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characterss")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage ="Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Status is required.")]
        public string Status { get; set; }

        public string Owner { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage ="StartDate is required.")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }
}
