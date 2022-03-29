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
            modelBuilder.EntitySet<AwardUser>("AwardUsers");
            modelBuilder.EntitySet<AwardUserView>("AwardUserViews");
            modelBuilder.EntitySet<Tag>("Tags");
            modelBuilder.EntitySet<TagCount>("TagCounts");
            modelBuilder.EntitySet<TagCountByRefType>("TagCountByRefTypes");
            modelBuilder.EntitySet<OverviewInfo>("OverviewInfos");
            modelBuilder.EntitySet<ExerciseItemWithTagView>("ExerciseItemWithTagViews");
            modelBuilder.EntitySet<KnowledgeItemWithTagView>("KnowledgeItemWithTagViews");

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

            modelBuilder.EntitySet<InvitedUser>("InvitedUsers");
            var inviteUserEntity = modelBuilder.EntityType<InvitedUser>();
            inviteUserEntity.Property(prop => prop.CreatedAt).AsDate();
            inviteUserEntity.Property(prop => prop.LastLoginAt).AsDate();
            //var loginAction = inviteUserEntity.Collection.Action("ValidInvitationCode");
            //loginAction.Parameter<String>("InvitationCode");
            //loginAction.ReturnsFromEntitySet<InvitedUser>("InvitedUsers");
            //var displayNameAction = inviteUserEntity.Collection.Function("GetDisplayAs").Returns<String>();
            //displayNameAction.Parameter<String>("UserID");

            // 2021.11.6
            modelBuilder.EntitySet<UserHabit>("UserHabits");
            var usrHabitEntity = modelBuilder.EntityType<UserHabit>();
            usrHabitEntity.Property(prop => prop.ValidFrom).AsDate();
            usrHabitEntity.Property(prop => prop.ValidTo).AsDate();
            modelBuilder.EntitySet<UserHabitRule>("UserHabitRules");
            modelBuilder.EntitySet<UserHabitRecord>("UserHabitRecords");
            var usrHabitRecordEntity = modelBuilder.EntityType<UserHabitRecord>();
            usrHabitRecordEntity.Property(prop => prop.RecordDate).AsDate();

            // 2021.11.27
            modelBuilder.EntitySet<UserHabitPointsByUserDate>("UserHabitPointsByUserDates");
            var usrHabitPointByUserDateEntity = modelBuilder.EntityType<UserHabitPointsByUserDate>();
            usrHabitPointByUserDateEntity.Property(prop => prop.RecordDate).AsDate();
            var latestUserHabitPointAction = usrHabitPointByUserDateEntity.Collection.Action("GetOpeningPoint");
            latestUserHabitPointAction.Parameter<String>("User");
            latestUserHabitPointAction.Parameter<int>("DaysBackTo");
            latestUserHabitPointAction.Returns<int>();

            modelBuilder.EntitySet<UserHabitPointsByUserHabitDate>("UserHabitPointsByUserHabitDates");
            var usrHabitPointByUserHabitDateEntity = modelBuilder.EntityType<UserHabitPointsByUserHabitDate>();
            usrHabitPointByUserHabitDateEntity.Property(prop => prop.RecordDate).AsDate();

            // 2021.11.28
            modelBuilder.EntitySet<UserHabitRecordView>("UserHabitRecordViews");
            var usrHabitRecordViewEntity = modelBuilder.EntityType<UserHabitRecordView>();
            usrHabitRecordViewEntity.Property(prop => prop.RecordDate).AsDate();

            // 2021.12.02
            modelBuilder.EntitySet<UserHabitPointReport>("UserHabitPointReports");
            var usrHabitPointViewEntity = modelBuilder.EntityType<UserHabitPointReport>();
            usrHabitPointViewEntity.Property(prop => prop.RecordDate).AsDate();

            // 2021.12.03
            modelBuilder.EntitySet<UserHabitPoint>("UserHabitPoints");
            var usrHabitPointEntity = modelBuilder.EntityType<UserHabitPoint>();
            usrHabitPointEntity.Property(prop => prop.RecordDate).AsDate();
            var latestUserPointAction = usrHabitPointEntity.Collection.Action("GetOpeningPoint");
            latestUserPointAction.Parameter<String>("User");
            latestUserPointAction.Parameter<int>("DaysBackTo");
            latestUserPointAction.Returns<int>();

            modelBuilder.Namespace = typeof(KnowledgeItem).Namespace;

            return modelBuilder.GetEdmModel();
        }
    }
}
