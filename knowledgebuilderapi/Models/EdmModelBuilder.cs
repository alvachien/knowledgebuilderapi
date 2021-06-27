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
            modelBuilder.EnumType<AwardRuleType>();
            modelBuilder.EntitySet<AwardRule>("AwardRules");
            modelBuilder.EntitySet<DailyTrace>("DailyTraces");            
            modelBuilder.EntitySet<AwardPoint>("AwardPoints");
            modelBuilder.EntitySet<Tag>("Tags");
            modelBuilder.EntitySet<TagCount>("TagCounts");
            modelBuilder.EntitySet<TagCountByRefType>("TagCountByRefTypes");
            modelBuilder.EntitySet<OverviewInfo>("OverviewInfos");
            modelBuilder.EntitySet<ExerciseItemWithTagView>("ExerciseItemWithTagViews");
            modelBuilder.EntitySet<KnowledgeItemWithTagView>("KnowledgeItemWithTagViews");

            // Action on Daily trace template documents
            var awardPointEntity = modelBuilder.EntityType<AwardPoint>();
            awardPointEntity.Property(prop => prop.RecordDate).AsDate();
            var dailyTraceEntity = modelBuilder.EntityType<DailyTrace>();
            dailyTraceEntity.Property(prop => prop.RecordDate).AsDate();
            var simulatePointAction = dailyTraceEntity.Collection.Action("SimulatePoints");
            simulatePointAction.Parameter<DailyTrace>("dt");
            simulatePointAction.ReturnsCollectionFromEntitySet<AwardPoint>("AwardPoints");
            //.Parameter<DailyTrace>("dt");

            modelBuilder.Namespace = typeof(KnowledgeItem).Namespace;

            return modelBuilder.GetEdmModel();
        }
    }
}
