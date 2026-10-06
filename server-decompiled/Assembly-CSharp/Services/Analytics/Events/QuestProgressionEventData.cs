using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016B7 RID: 5815
	[EventTitle("QuestProgression")]
	public class QuestProgressionEventData : BaseEventData
	{
		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x0600B63F RID: 46655 RVA: 0x0043BD1C File Offset: 0x00439F1C
		public override string EventType
		{
			get
			{
				return "quest_progression";
			}
		}

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x0600B640 RID: 46656 RVA: 0x0043BD23 File Offset: 0x00439F23
		// (set) Token: 0x0600B641 RID: 46657 RVA: 0x0043BD2B File Offset: 0x00439F2B
		[JsonProperty(PropertyName = "server_id")]
		[Description("Server identity")]
		public string ServerId { get; set; }

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x0600B642 RID: 46658 RVA: 0x0043BD34 File Offset: 0x00439F34
		// (set) Token: 0x0600B643 RID: 46659 RVA: 0x0043BD3C File Offset: 0x00439F3C
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId { get; set; }

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x0600B644 RID: 46660 RVA: 0x0043BD45 File Offset: 0x00439F45
		// (set) Token: 0x0600B645 RID: 46661 RVA: 0x0043BD4D File Offset: 0x00439F4D
		[JsonProperty(PropertyName = "quest_progression_ts")]
		[DateTimeFormat]
		public string QuestProgressionTimestamp { get; set; }

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x0600B646 RID: 46662 RVA: 0x0043BD56 File Offset: 0x00439F56
		// (set) Token: 0x0600B647 RID: 46663 RVA: 0x0043BD5E File Offset: 0x00439F5E
		[JsonProperty(PropertyName = "quest_instance_id")]
		[Description("Primary join key — unique per accepted instance; identical across quest_started → quest_progression → quest_reward.")]
		public string QuestInstanceId { get; set; }

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x0600B648 RID: 46664 RVA: 0x0043BD67 File Offset: 0x00439F67
		// (set) Token: 0x0600B649 RID: 46665 RVA: 0x0043BD6F File Offset: 0x00439F6F
		[JsonProperty(PropertyName = "quest_id")]
		public string QuestId { get; set; }

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x0600B64A RID: 46666 RVA: 0x0043BD78 File Offset: 0x00439F78
		// (set) Token: 0x0600B64B RID: 46667 RVA: 0x0043BD80 File Offset: 0x00439F80
		[JsonProperty(PropertyName = "quest_name")]
		[Description("English localization of the quest name.")]
		public string QuestName { get; set; }

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x0600B64C RID: 46668 RVA: 0x0043BD89 File Offset: 0x00439F89
		// (set) Token: 0x0600B64D RID: 46669 RVA: 0x0043BD91 File Offset: 0x00439F91
		[JsonProperty(PropertyName = "quest_group")]
		public string QuestGroup { get; set; }

		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x0600B64E RID: 46670 RVA: 0x0043BD9A File Offset: 0x00439F9A
		// (set) Token: 0x0600B64F RID: 46671 RVA: 0x0043BDA2 File Offset: 0x00439FA2
		[JsonProperty(PropertyName = "quest_category")]
		[Description("The category of the quest.")]
		public string QuestCategory { get; set; }

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x0600B650 RID: 46672 RVA: 0x0043BDAB File Offset: 0x00439FAB
		// (set) Token: 0x0600B651 RID: 46673 RVA: 0x0043BDB3 File Offset: 0x00439FB3
		[JsonProperty(PropertyName = "quest_tier")]
		public int? QuestTier { get; set; }

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x0600B652 RID: 46674 RVA: 0x0043BDBC File Offset: 0x00439FBC
		// (set) Token: 0x0600B653 RID: 46675 RVA: 0x0043BDC4 File Offset: 0x00439FC4
		[JsonProperty(PropertyName = "quest_owner_provider_id")]
		[KrakenIdTransform("$.quest_owner_provider_id")]
		[Description("data_provider_user_id of the player who acquired the quest")]
		public string QuestOwnerProviderId { get; set; }

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x0600B654 RID: 46676 RVA: 0x0043BDCD File Offset: 0x00439FCD
		// (set) Token: 0x0600B655 RID: 46677 RVA: 0x0043BDD5 File Offset: 0x00439FD5
		[JsonProperty(PropertyName = "progress_type")]
		public string ProgressType { get; set; }

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x0600B656 RID: 46678 RVA: 0x0043BDDE File Offset: 0x00439FDE
		// (set) Token: 0x0600B657 RID: 46679 RVA: 0x0043BDE6 File Offset: 0x00439FE6
		[JsonProperty(PropertyName = "objective_type")]
		public string ObjectiveType { get; set; }

		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x0600B658 RID: 46680 RVA: 0x0043BDEF File Offset: 0x00439FEF
		// (set) Token: 0x0600B659 RID: 46681 RVA: 0x0043BDF7 File Offset: 0x00439FF7
		[JsonProperty(PropertyName = "objective_parameters")]
		public object ObjectiveParameters { get; set; }

		// Token: 0x0600B65A RID: 46682 RVA: 0x0043BE00 File Offset: 0x0043A000
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

		// Token: 0x0600B65B RID: 46683 RVA: 0x0043B40B File Offset: 0x0043960B
		public QuestProgressionEventData()
		{
		}
	}
}
