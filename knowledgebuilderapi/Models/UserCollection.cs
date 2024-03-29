﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    [Table("UserCollection")]
    public class UserCollection : BaseModel
    {
        public UserCollection(): base()
        {
            Items = new List<UserCollectionItem>();
        }

        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("User", TypeName = "NVARCHAR(50)")]
        public String User { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Name", TypeName = "NVARCHAR(50)")]
        public String Name { get; set; }

        [StringLength(100)]
        [Column("Comment", TypeName = "NVARCHAR(100)")]
        public String Comment { get; set; }

        public ICollection<UserCollectionItem> Items { get; set; }
    }

    [Table("UserCollectionItem")]
    public class UserCollectionItem
    {
        public UserCollectionItem() { }
        public UserCollectionItem(UserCollectionItem other) : this()
        {
            ID = other.ID;
            RefType = other.RefType;
            RefID = other.RefID;
            CreatedAt = other.CreatedAt;
        }

        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Key]
        [Column("RefType")]
        public TagRefType RefType { get; set; }

        [Key]
        [Column("RefID", TypeName = "INT")]
        public Int32 RefID { get; set; }

        [Column("CreatedAt")]
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }

        public UserCollection Collection { get; set; }
    }
}
