using System.ComponentModel.DataAnnotations;

namespace GisZhkhAdmin.Models
{
    public class Contract
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Number { get; set; } = string.Empty;
        
        [Required]
        public DateTime SignDate { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Required]
        public int StatusId { get; set; }
        
        public virtual ContractStatus Status { get; set; } = null!;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}