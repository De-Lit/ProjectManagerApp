using SQLite;
using System.ComponentModel.DataAnnotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace ProjectManager.Models
{
    public class Staff
    {
        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        [Required]
        [StringLength(30, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]+|[а-яА-Я]+$")]
        public string Name { get; set; } = null!;
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z]+|[а-яА-Я]+$")]
        public string? Patronymic { get; set; }
        [NotNull]
        [Required]
        [StringLength(30, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]+|[а-яА-Я]+$")]
        public string Surname { get; set; } = null!;
        [StringLength(30)]
        [RegularExpression(@"[\w-\.]+@([\w-]+\.)+[\w-]{2,4}")]
        public string? Email { get; set; }
        public ICollection<Project> ?ProjectsManager { get; } = new List<Project>();
        public ICollection<Project> ProjectsEmployee { get; } = new List<Project>();
    }
}
