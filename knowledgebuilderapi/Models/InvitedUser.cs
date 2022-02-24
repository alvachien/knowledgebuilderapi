using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace knowledgebuilderapi.Models
{
    [Table("InvitedUser")]
    public class InvitedUser
    {
        public InvitedUser()
        {
            AwardUsers = new List<AwardUserView>();
        }

        [Key]
        [Column("UserID", TypeName = "NVARCHAR(50)")]
        [StringLength(50)]
        public String UserID{ get; set; }

        [Required]
        [Column("InvitationCode", TypeName = "NVARCHAR(20)")]
        [IgnoreDataMember]
        public String InvitationCode { get; set; }

        [Required]
        [Column("UserName", TypeName = "NVARCHAR(50)")]
        [StringLength(50)]
        public String UserName { get; set; }

        [Required]
        [Column("DisplayAs", TypeName = "NVARCHAR(50)")]
        [StringLength(50)]
        public String DisplayAs { get; set; }

        [Column("Deleted", TypeName = "BIT")]
        public Boolean? Deleted;

        [Column("CreatedAt", TypeName = "DATETIME")]
        public DateTime CreatedAt { get; set; }

        [Column("LastLoginAt", TypeName = "DATETIME")]
        public DateTime LastLoginAt { get; set; }

        public ICollection<AwardUserView> AwardUsers { get; set; }
    }
}
