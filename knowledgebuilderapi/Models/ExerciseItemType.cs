using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace knowledgebuilderapi.Models
{
    public enum ExerciseItemType : Int16
    {
        Question = 0,
        SingleChoice = 1,
        MultipleChoice = 2,
        ShortAnswer = 3,
        EssayQuestions = 4,
    }
}
