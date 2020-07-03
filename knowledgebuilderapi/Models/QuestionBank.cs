using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    [Table("QuestionBankItem")]
    public sealed class QuestionBankItem : BaseModel
    {
        [Key]
        public Int32 ID { get; set; }

        [Column("KnowledgeItem", TypeName = "INT")]
        public Int32? KnowledgeItemID { get; set; }

        [Column("ParentID", TypeName = "INT")]
        public Int32? ParentID { get; set; }

        [Required]
        [Column("QBType", TypeName = "INT")]
        public Int32 QBType { get; set; }

        [Required]
        [Column("Content", TypeName = "TEXT")]
        public string Content { get; set; }

        public KnowledgeItem CurrentKnowledgeItem { get; set; }
        public ICollection<QuestionBankSubItem> SubItems { get; set; }
    }


    [Table("QuestionBankSubItem")]
    public sealed class QuestionBankSubItem
    {
        [Required]
        [Column("ItemID", TypeName = "INT")]
        public Int32 ItemID { get; set; }

        [Required]
        [Column("SubID", TypeName = "INT")]
        public Int32 SubID { get; set; }

        [Required]
        [Column("QBType", TypeName = "INT")]
        public Int32 QBType { get; set; }

        [Required]
        [Column("Content", TypeName = "TEXT")]
        public string Content { get; set; }

        public QuestionBankItem CurrentQuestionBankItem { get; set; }
    }
}
