using System;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace knowledgebuilderapi.Models
{
    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<KnowledgeItem>("KnowledgeItems");
            modelBuilder.EntitySet<ExerciseItem>("ExerciseItems");
            modelBuilder.EntitySet<ExerciseItemAnswer>("ExerciseItemAnswers");
            modelBuilder.EntitySet<KnowledgeTag>("KnowledgeTags");
            modelBuilder.EntitySet<ExerciseTag>("ExerciseTags");
            modelBuilder.EntitySet<Tag>("Tags");
            modelBuilder.EntitySet<TagCount>("TagCounts");
            modelBuilder.EntitySet<TagCountByRefType>("TagCountByRefTypes");
            modelBuilder.EntitySet<OverviewInfo>("OverviewInfos");
            modelBuilder.EntitySet<ExerciseItemWithTagView>("ExerciseItemWithTagViews");
            modelBuilder.EntitySet<KnowledgeItemWithTagView>("KnowledgeItemWithTagViews");
            modelBuilder.Namespace = typeof(KnowledgeItem).Namespace;

            return modelBuilder.GetEdmModel();
        }
    }
}
