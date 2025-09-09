using System.ComponentModel.DataAnnotations;

namespace GisZhkhAdmin.Models
{
    public class ContractStatus
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}