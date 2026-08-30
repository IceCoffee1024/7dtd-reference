using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001912 RID: 6418
	[Preserve]
	public class ChallengeObjectiveTwitch : BaseChallengeObjective
	{
		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x0600C63E RID: 50750 RVA: 0x0048E3D2 File Offset: 0x0048C5D2
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Twitch;
			}
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x0600C63F RID: 50751 RVA: 0x0048E3D8 File Offset: 0x0048C5D8
		public override string DescriptionText
		{
			get
			{
				switch (this.TwitchObjectiveType)
				{
				case TwitchObjectiveTypes.Enabled:
					return Localization.Get("challengeObjectiveTwitchEnabled", false, null);
				case TwitchObjectiveTypes.EnableExtras:
					return Localization.Get("challengeObjectiveTwitchEnableExtras", false, null);
				case TwitchObjectiveTypes.HelperReward:
					return Localization.Get("challengeObjectiveTwitchHelperRewards", false, null);
				case TwitchObjectiveTypes.ChannelPointRedeems:
					return Localization.Get("challengeObjectiveTwitchChannelPointRedeems", false, null);
				case TwitchObjectiveTypes.VoteComplete:
					return Localization.Get("challengeObjectiveTwitchVotesCompleted", false, null);
				case TwitchObjectiveTypes.PimpPot:
					return Localization.Get("challengeObjectiveTwitchPimpPotRewarded", false, null);
				case TwitchObjectiveTypes.BitPot:
					return Localization.Get("challengeObjectiveTwitchBitPotRewarded", false, null);
				case TwitchObjectiveTypes.DefeatBossHorde:
					return Localization.Get("challengeObjectiveTwitchBossHordesDefeated", false, null);
				case TwitchObjectiveTypes.GoodAction:
					return Localization.Get("challengeObjectiveTwitchGoodActions", false, null);
				case TwitchObjectiveTypes.BadAction:
					return Localization.Get("challengeObjectiveTwitchBadActions", false, null);
				default:
					return "";
				}
			}
		}

		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x0600C640 RID: 50752 RVA: 0x0048E4A6 File Offset: 0x0048C6A6
		public override ChallengeClass.UINavTypes NavType
		{
			get
			{
				if (this.TwitchObjectiveType == TwitchObjectiveTypes.EnableExtras)
				{
					return ChallengeClass.UINavTypes.TwitchActions;
				}
				return ChallengeClass.UINavTypes.None;
			}
		}

		// Token: 0x0600C641 RID: 50753 RVA: 0x0048E4B4 File Offset: 0x0048C6B4
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.TwitchEventReceive += this.Current_TwitchEventReceive;
		}

		// Token: 0x0600C642 RID: 50754 RVA: 0x0048E4CC File Offset: 0x0048C6CC
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.TwitchEventReceive -= this.Current_TwitchEventReceive;
		}

		// Token: 0x0600C643 RID: 50755 RVA: 0x0048E4E4 File Offset: 0x0048C6E4
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_TwitchEventReceive(TwitchObjectiveTypes action, string param)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (action == this.TwitchObjectiveType && (this.Param == "" || this.Param.EqualsCaseInsensitive(param)))
			{
				int num = base.Current;
				base.Current = num + 1;
			}
			if (base.Current >= this.MaxCount)
			{
				base.Current = this.MaxCount;
				this.CheckObjectiveComplete(true);
			}
		}

		// Token: 0x0600C644 RID: 50756 RVA: 0x0048E558 File Offset: 0x0048C758
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("objective_type"))
			{
				this.TwitchObjectiveType = (TwitchObjectiveTypes)Enum.Parse(typeof(TwitchObjectiveTypes), e.GetAttribute("objective_type"), true);
			}
			if (e.HasAttribute("objective_param"))
			{
				this.Param = e.GetAttribute("objective_param");
			}
		}

		// Token: 0x0600C645 RID: 50757 RVA: 0x0048E5D1 File Offset: 0x0048C7D1
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveTwitch
			{
				TwitchObjectiveType = this.TwitchObjectiveType,
				Param = this.Param
			};
		}

		// Token: 0x04009580 RID: 38272
		public TwitchObjectiveTypes TwitchObjectiveType;

		// Token: 0x04009581 RID: 38273
		public string Param = "";
	}
}
