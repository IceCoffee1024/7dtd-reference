using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200193E RID: 6462
	[Preserve]
	public class ChallengeObjectiveTwitch : BaseChallengeObjective
	{
		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x0600C796 RID: 51094 RVA: 0x004928BE File Offset: 0x00490ABE
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Twitch;
			}
		}

		// Token: 0x17001932 RID: 6450
		// (get) Token: 0x0600C797 RID: 51095 RVA: 0x004928C4 File Offset: 0x00490AC4
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

		// Token: 0x17001933 RID: 6451
		// (get) Token: 0x0600C798 RID: 51096 RVA: 0x00492992 File Offset: 0x00490B92
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

		// Token: 0x0600C799 RID: 51097 RVA: 0x004929A0 File Offset: 0x00490BA0
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.TwitchEventReceive += this.Current_TwitchEventReceive;
		}

		// Token: 0x0600C79A RID: 51098 RVA: 0x004929B8 File Offset: 0x00490BB8
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.TwitchEventReceive -= this.Current_TwitchEventReceive;
		}

		// Token: 0x0600C79B RID: 51099 RVA: 0x004929D0 File Offset: 0x00490BD0
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

		// Token: 0x0600C79C RID: 51100 RVA: 0x00492A44 File Offset: 0x00490C44
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

		// Token: 0x0600C79D RID: 51101 RVA: 0x00492ABD File Offset: 0x00490CBD
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveTwitch
			{
				TwitchObjectiveType = this.TwitchObjectiveType,
				Param = this.Param
			};
		}

		// Token: 0x04009646 RID: 38470
		public TwitchObjectiveTypes TwitchObjectiveType;

		// Token: 0x04009647 RID: 38471
		public string Param = "";
	}
}
