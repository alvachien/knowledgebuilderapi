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

        public override Boolean Equals(Object other)
        {
            if (other == null || !(other is ExerciseItem))
                throw new InvalidOperationException("Invalid parameter: Other");

            ExerciseItem ei2 = other as ExerciseItem;
            if (this.ID != ei2.ID)
                return false;
            if (this.KnowledgeItemID != ei2.KnowledgeItemID)
                return false;
            if (this.ExerciseType != ei2.ExerciseType)
                return false;
            if (String.CompareOrdinal(this.Content, ei2.Content) != 0)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.ID;
        }

        public void UpdateData(ExerciseItem other)
        {
            if (other == null)
                throw new InvalidOperationException("Invalid parameter: Other");

            if (KnowledgeItemID != other.KnowledgeItemID)
                KnowledgeItemID = other.KnowledgeItemID;
            if (ExerciseType != other.ExerciseType)
                ExerciseType = other.ExerciseType;
            if (String.CompareOrdinal(Content, other.Content) != 0)
                Content = other.Content;
        }
    }


    [Table("ExerciseItemAnswer")]
    public sealed class ExerciseItemAnswer : BaseModel
    {
        public ExerciseItemAnswer()
        {
        }

        public ExerciseItemAnswer(ExerciseItemAnswer other) : this()
        {
            this.ID = other.ID;
            this.Content = other.Content;
        }
        
        [Key]
        [Column("ItemID")]
        public Int32 ID { get; set; }

        [Required]
        [Column("Content", TypeName = "TEXT")]
        public string Content { get; set; }

        public ExerciseItem ExerciseItem { get; set; }

        public override Boolean Equals(Object other)
        {
            if (other == null || !(other is ExerciseItemAnswer))
                throw new InvalidOperationException("Invalid parameter: Other");

            ExerciseItemAnswer ei2 = other as ExerciseItemAnswer;
            if (this.ID != ei2.ID)
                return false;
            if (String.CompareOrdinal(this.Content, ei2.Content) != 0)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.ID;
        }

        public void UpdateData(ExerciseItemAnswer other)
        {
            if (other == null)
                throw new InvalidOperationException("Invalid parameter: Other");

            if (String.CompareOrdinal(Content, other.Content) != 0)
                Content = other.Content;
        }
    }
}
