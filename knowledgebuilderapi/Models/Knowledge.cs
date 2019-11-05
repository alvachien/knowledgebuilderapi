using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    [Table("Knowledge")]
    public class Knowledge : BaseModel
    {

        [Key]
        public Int32 ID { get; set; }
        [Required]
        [Column("ContentType")]
        public KnowledgeCategory Category { get;set; }
        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        [Column("Title", TypeName = "NVARCHAR(50)")]
        public string Title { get;set; }
        [Required]
        [Column("Content")]
        public string Content { get;set; }
        [Column("Tags")]
        public string Tags { get; set; }
    }
}
