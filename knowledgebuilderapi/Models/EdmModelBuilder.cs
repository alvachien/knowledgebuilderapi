using System;
using System.Collections.Generic;
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
            modelBuilder.EntitySet<AwardRuleGroup>("AwardRuleGroups");
            modelBuilder.EntitySet<AwardRule>("AwardRules");
            modelBuilder.EntitySet<DailyTrace>("DailyTraces");            
            modelBuilder.EntitySet<AwardPoint>("AwardPoints");
            modelBuilder.EntitySet<AwardPointReport>("AwardPointReports");
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

            // User collection and user score -- 2021.09.19
            modelBuilder.EntitySet<UserCollection>("UserCollections");

            modelBuilder.EntitySet<UserCollectionItem>("UserCollectionItems");
            var collItemEntity = modelBuilder.EntityType<UserCollectionItem>();
            collItemEntity.Property(prop => prop.CreatedAt).AsDate();
            var addItemAction = collItemEntity.Collection.Action("AddItemToCollection");
            addItemAction.Parameter<String>("User");
            addItemAction.Parameter<int>("ID");
            addItemAction.Parameter<TagRefType>("RefType");
            addItemAction.Parameter<int>("RefID");
            addItemAction.Parameter<DateTime?>("CreatedAt");
            addItemAction.ReturnsFromEntitySet<UserCollectionItem>("UserCollectionItems");
            var delItemAction = collItemEntity.Collection.Action("RemoveItemFromCollection");
            delItemAction.Parameter<String>("User");
            delItemAction.Parameter<int>("ID");
            delItemAction.Parameter<TagRefType>("RefType");
            delItemAction.Parameter<int>("RefID");
            delItemAction.Returns<Boolean>();
            var addItemExAction = collItemEntity.Collection.Action("AddItemToCollectionEx");
            addItemExAction.Parameter<String>("User");
            addItemExAction.CollectionParameter<UserCollectionItem>("UserCollectionItems");
            addItemExAction.ReturnsFromEntitySet<UserCollectionItem>("UserCollectionItems");

            modelBuilder.EntitySet<ExerciseItemUserScore>("ExerciseItemUserScores");
            var userScoreEntity = modelBuilder.EntityType<ExerciseItemUserScore>();
            userScoreEntity.Property(prop => prop.TakenDate).AsDate();
            var latestScoreAction = userScoreEntity.Collection.Action("LatestUserScore");
            latestScoreAction.Parameter<String>("User");
            latestScoreAction.Parameter<int>("RefID");
            latestScoreAction.ReturnsFromEntitySet<ExerciseItemUserScore>("ExerciseItemUserScores");

            modelBuilder.Namespace = typeof(KnowledgeItem).Namespace;

            return modelBuilder.GetEdmModel();
        }
    }
}
