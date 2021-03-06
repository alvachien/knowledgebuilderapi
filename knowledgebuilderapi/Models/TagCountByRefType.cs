﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    public class TagCountByRefType
    {
        [Key]
        [Column("RefType")]
        public TagRefType RefType { get; set; }

        [Column("TagCount")]
        public int TagCount { get; set; }
    }
}
