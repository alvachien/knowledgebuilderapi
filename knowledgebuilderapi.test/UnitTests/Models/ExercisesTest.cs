using knowledgebuilderapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace knowledgebuilderapi.test.unittest.Models
{
    [Collection("KBAPI_UnitTests#2")]
    public class ExercisesTest
    {
        [Fact]
        public void TestCase_ExerciseItem_Equals()
        {
            var ei = new ExerciseItem();
            ei.ExerciseType = ExerciseItemType.Question;
            ei.Content = "Test1";

            var ei2 = new ExerciseItem(); ;
            ei2.ExerciseType = ExerciseItemType.EssayQuestions;
            ei2.Content = "Test2";

            var rst = ei.Equals(ei2);
            Assert.False(rst);
        }

        [Fact]
        public void TestCase_ExerciseItem_Equals2()
        {
            var ei = new ExerciseItem();
            ei.ExerciseType = ExerciseItemType.Question;
            ei.Content = "Test1";

            Assert.Throws<InvalidOperationException>(() => ei.Equals(null));
        }

        [Fact]
        public void TestCase_ExerciseItemAnswer_Equals()
        {
            var eia = new ExerciseItemAnswer();
            eia.ID = 1;
            eia.Content = "Test1";

            var ei2 = new ExerciseItemAnswer();
            ei2.ID = 1;
            ei2.Content = "Test1";

            Assert.True(eia.Equals(ei2));

            Assert.Throws<InvalidOperationException>(() => eia.Equals(null));

            // ExerciseItemWithTagView
            var eitv = new ExerciseItemWithTagView();
            eitv.ID = 1;
            eitv.ExerciseType = ExerciseItemType.MultipleChoice;
            eitv.Content = "Test 1";
            Assert.NotNull(eitv);
        }

        [Fact]
        public void TestCase_ExerciseItemUserScore_UpdateData()
        {
            var score = new ExerciseItemUserScore();
            score.ID = 1;
            score.RefID = 2;
            score.Score = 100;
            score.User = "Test";

            Assert.Throws<InvalidOperationException>(() => score.UpdateData(null));

            var score2 = new ExerciseItemUserScore();
            score2.UpdateData(score);

            Assert.Equal(score2.Score, score.Score);
        }

        [Fact]
        public void TestCase_ExerciseItemResult()
        {
            var rst = new ExerciseItemResult();
            rst.ID = 1;
            rst.User = "Test";
            rst.ResultScore = 100;

            Assert.NotNull(rst);
        }
    }
}
