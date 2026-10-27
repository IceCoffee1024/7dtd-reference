using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016D1 RID: 5841
	[EventTitle("QuestProgression")]
	public class QuestProgressionEventData : BaseEventData
	{
		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x0600B708 RID: 46856 RVA: 0x0043E544 File Offset: 0x0043C744
		public override string EventType
		{
			get
			{
				return "quest_progression";
			}
		}

		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x0600B709 RID: 46857 RVA: 0x0043E54B File Offset: 0x0043C74B
		// (set) Token: 0x0600B70A RID: 46858 RVA: 0x0043E553 File Offset: 0x0043C753
		[JsonProperty(PropertyName = "server_id")]
		[Description("Server identity")]
		public string ServerId { get; set; }

		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x0600B70B RID: 46859 RVA: 0x0043E55C File Offset: 0x0043C75C
		// (set) Token: 0x0600B70C RID: 46860 RVA: 0x0043E564 File Offset: 0x0043C764
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId { get; set; }

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x0600B70D RID: 46861 RVA: 0x0043E56D File Offset: 0x0043C76D
		// (set) Token: 0x0600B70E RID: 46862 RVA: 0x0043E575 File Offset: 0x0043C775
		[JsonProperty(PropertyName = "quest_progression_ts")]
		[DateTimeFormat]
		public string QuestProgressionTimestamp { get; set; }

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x0600B70F RID: 46863 RVA: 0x0043E57E File Offset: 0x0043C77E
		// (set) Token: 0x0600B710 RID: 46864 RVA: 0x0043E586 File Offset: 0x0043C786
		[JsonProperty(PropertyName = "quest_instance_id")]
		[Description("Primary join key — unique per accepted instance; identical across quest_started → quest_progression → quest_reward.")]
		public string QuestInstanceId { get; set; }

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x0600B711 RID: 46865 RVA: 0x0043E58F File Offset: 0x0043C78F
		// (set) Token: 0x0600B712 RID: 46866 RVA: 0x0043E597 File Offset: 0x0043C797
		[JsonProperty(PropertyName = "quest_id")]
		public string QuestId { get; set; }

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x0600B713 RID: 46867 RVA: 0x0043E5A0 File Offset: 0x0043C7A0
		// (set) Token: 0x0600B714 RID: 46868 RVA: 0x0043E5A8 File Offset: 0x0043C7A8
		[JsonProperty(PropertyName = "quest_name")]
		[Description("English localization of the quest name.")]
		public string QuestName { get; set; }

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x0600B715 RID: 46869 RVA: 0x0043E5B1 File Offset: 0x0043C7B1
		// (set) Token: 0x0600B716 RID: 46870 RVA: 0x0043E5B9 File Offset: 0x0043C7B9
		[JsonProperty(PropertyName = "quest_group")]
		public string QuestGroup { get; set; }

		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x0600B717 RID: 46871 RVA: 0x0043E5C2 File Offset: 0x0043C7C2
		// (set) Token: 0x0600B718 RID: 46872 RVA: 0x0043E5CA File Offset: 0x0043C7CA
		[JsonProperty(PropertyName = "quest_category")]
		[Description("The category of the quest.")]
		public string QuestCategory { get; set; }

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x0600B719 RID: 46873 RVA: 0x0043E5D3 File Offset: 0x0043C7D3
		// (set) Token: 0x0600B71A RID: 46874 RVA: 0x0043E5DB File Offset: 0x0043C7DB
		[JsonProperty(PropertyName = "quest_tier")]
		public int? QuestTier { get; set; }

		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x0600B71B RID: 46875 RVA: 0x0043E5E4 File Offset: 0x0043C7E4
		// (set) Token: 0x0600B71C RID: 46876 RVA: 0x0043E5EC File Offset: 0x0043C7EC
		[JsonProperty(PropertyName = "quest_owner_provider_id")]
		[KrakenIdTransform("$.quest_owner_provider_id")]
		[Description("data_provider_user_id of the player who acquired the quest")]
		public string QuestOwnerProviderId { get; set; }

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x0600B71D RID: 46877 RVA: 0x0043E5F5 File Offset: 0x0043C7F5
		// (set) Token: 0x0600B71E RID: 46878 RVA: 0x0043E5FD File Offset: 0x0043C7FD
		[JsonProperty(PropertyName = "progress_type")]
		public string ProgressType { get; set; }

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x0600B71F RID: 46879 RVA: 0x0043E606 File Offset: 0x0043C806
		// (set) Token: 0x0600B720 RID: 46880 RVA: 0x0043E60E File Offset: 0x0043C80E
		[JsonProperty(PropertyName = "objective_type")]
		public string ObjectiveType { get; set; }

		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x0600B721 RID: 46881 RVA: 0x0043E617 File Offset: 0x0043C817
		// (set) Token: 0x0600B722 RID: 46882 RVA: 0x0043E61F File Offset: 0x0043C81F
		[JsonProperty(PropertyName = "objective_parameters")]
		public object ObjectiveParameters { get; set; }

		// Token: 0x0600B723 RID: 46883 RVA: 0x0043E628 File Offset: 0x0043C828
		public QuestProgressionEventData(BaseObjective objective)
		{
			PersistentPlayerData persistentPlayerData = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(objective.OwnerQuest.SharedOwnerID) ?? objective.OwnerQuest.OwnerJournal.OwnerPlayer.PersistentPlayerData;
			this.ServerId = Helper.GetServerId();
			this.SaveId = Helper.GetSaveId();
			this.QuestProgressionTimestamp = DateTime.UtcNow.ToString("O");
			this.QuestInstanceId = objective.OwnerQuest.QuestUniqueId;
			this.QuestId = objective.OwnerQuest.ID;
			this.QuestName = objective.OwnerQuest.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropNameKey);
			this.QuestGroup = objective.OwnerQuest.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropGroupNameKey);
			this.QuestCategory = objective.OwnerQuest.QuestClass.Properties.Values.GetValueOrDefault(QuestClass.PropCategory);
			this.QuestTier = ((objective.OwnerQuest.QuestClass.DifficultyTier != 0) ? new int?((int)objective.OwnerQuest.QuestClass.DifficultyTier) : null);
			this.QuestOwnerProviderId = persistentPlayerData.NativeId.ReadablePlatformUserIdentifier;
			this.ProgressType = (objective.Complete ? "Complete" : "Failed");
			this.ObjectiveType = objective.GetType().Name;
			this.ObjectiveParameters = objective.ToParametersDictionary();
		}

		// Token: 0x0600B724 RID: 46884 RVA: 0x0043D859 File Offset: 0x0043BA59
		public QuestProgressionEventData()
		{
		}
	}
}
