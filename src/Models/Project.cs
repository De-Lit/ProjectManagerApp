using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class Project
    {
        [SQLite.PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        [Required]
        [Display(Name = "Project Name")]
        [StringLength(30, MinimumLength = 1)]
        public string ProjectName { get; set; } = null!;
        [Display(Name = "Client Name")]
        [StringLength(30, MinimumLength = 1)]
        public string? ClientName { get; set; }
        [ForeignKey("Staff")]
        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }
        public Staff? Manager { get; set; }
        public ICollection<Staff> Employees { get; } = new List<Staff>();
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        [NotNull]
        [Required]
        [Range(1, 5)]
        public int Priority { get; set; }
    }
}
