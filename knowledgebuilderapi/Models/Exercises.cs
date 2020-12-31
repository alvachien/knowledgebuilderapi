using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    [Table("ExerciseItem")]
    public sealed class ExerciseItem : BaseModel
    {
        public ExerciseItem() : base()
        {
            Tags = new HashSet<ExerciseTag>();
        }

        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Column("KnowledgeItem")]
        public Int32? KnowledgeItemID { get; set; }

        [Required]
        [Column("ExerciseType", TypeName = "SMALLINT")]
        public ExerciseItemType ExerciseType { get; set; }

        [Required]
        [Column("Content", TypeName = "TEXT")]
        public string Content { get; set; }

        public KnowledgeItem CurrentKnowledgeItem { get; set; }
        public ExerciseItemAnswer Answer { get; set; }
        public ICollection<ExerciseTag> Tags { get; set; }
    }


    [Table("ExerciseItemAnswer")]
    public sealed class ExerciseItemAnswer : BaseModel
    {
        [Key]
        [Column("ItemID")]
        public Int32 ID { get; set; }

        [Required]
        [Column("Content", TypeName = "TEXT")]
        public string Content { get; set; }

        public ExerciseItem ExerciseItem { get; set; }
    }
}
