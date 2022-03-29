using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace knowledgebuilderapi.Models
{
    [Table("AwardUser")]
    public class AwardUser
    {
        [Key]
        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Key]
        [Column("Supervisor", TypeName = "NVARCHAR(50)")]
        public String Supervisor { get; set; }
    }

    public sealed class AwardUserView
    {
        [Key]
        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Column("Supervisor", TypeName = "NVARCHAR(50)")]
        public String Supervisor { get; set; }

        [Column("UserName", TypeName = "NVARCHAR(50)")]
        public String UserName { get; set; }

        [Column("DisplayAs", TypeName = "NVARCHAR(50)")]
        public String DisplayAs { get; set; }

        public InvitedUser CurrentUser { get; set; }
    }
}
