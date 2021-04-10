using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    [Table("KnowledgeItem")]
    public sealed class KnowledgeItem : BaseModel
    {
        public KnowledgeItem()
        {
        }
        public KnowledgeItem(KnowledgeItem other)
        {
            this.ID = other.ID;
            this.Category = other.Category;
            this.Title = other.Title;
            this.Content = other.Content;
        }

        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Required]
        [Column("ContentType", TypeName = "SMALLINT")]
        public KnowledgeItemCategory Category { get;set; }

        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        [Column("Title", TypeName = "NVARCHAR(50)")]
        public string Title { get;set; }

        [Required]
        [Column("Content")]
        public string Content { get;set; }

        public ICollection<ExerciseItem> Exercises { get; set; }
        public ICollection<KnowledgeTag> Tags { get; set; }

        public override Boolean Equals(Object other)
        {
            if (other == null || !(other is KnowledgeItem))
                throw new InvalidOperationException("Invalid parameter: Other");

            KnowledgeItem ei2 = other as KnowledgeItem;
            if (this.ID != ei2.ID)
                return false;
            if (this.Category != ei2.Category)
                return false;
            if (String.CompareOrdinal(this.Title, ei2.Title) != 0)
                return false;
            if (String.CompareOrdinal(this.Content, ei2.Content) != 0)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.ID;
        }

        public void UpdateData(KnowledgeItem other)
        {
            if (other == null)
                throw new InvalidOperationException("Invalid parameter: Other");

            if (Category != other.Category)
                Category = other.Category;
            if (String.CompareOrdinal(Title, other.Title) != 0)
                Title = other.Title;
            if (String.CompareOrdinal(Content, other.Content) != 0)
                Content = other.Content;
        }
    }

    public class KnowledgeItemWithTagView
    {
        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Column("ContentType", TypeName = "SMALLINT")]
        public KnowledgeItemCategory Category { get; set; }

        [Column("Title", TypeName = "NVARCHAR(50)")]
        public string Title { get; set; }

        [Column("Content")]
        public string Content { get; set; }

        [Column("Tags")]
        public String Tags { get; set; }
    }
}
