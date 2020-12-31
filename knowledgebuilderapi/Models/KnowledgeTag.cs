using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    [Table("KnowledgeTag")]
    public class KnowledgeTag
    {
        [Key]
        [MaxLength(20)]
        [Column("Tag", TypeName = "NVARCHAR(20)")]
        public String TagTerm { get; set; }

        [Key]
        [Required]
        [Column("RefID", TypeName = "INT")]
        public Int32 RefID { get; set; }

        public KnowledgeItem CurrentKnowledgeItem { get; set; }
    }
}
