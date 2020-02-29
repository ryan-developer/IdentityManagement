using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityManagement.Persistence.Tenancy.Entities
{
    [Table("Tenant")]
    public class TenantEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InternalId { get; set; }

        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(500)]
        public string CompanyName { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}