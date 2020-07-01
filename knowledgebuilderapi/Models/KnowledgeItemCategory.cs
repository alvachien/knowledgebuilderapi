using System;
using System.ComponentModel.DataAnnotations;

namespace knowledgebuilderapi.Models 
{
    [Flags]
    public enum KnowledgeItemCategory: Int16 
    {
        Concept     = 0,
        Formula     = 1,
    }
}
