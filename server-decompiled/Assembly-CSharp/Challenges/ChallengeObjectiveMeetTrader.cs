using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001967 RID: 6503
	[Preserve]
	public class ChallengeObjectiveMeetTrader : BaseChallengeObjective
	{
		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x0600C861 RID: 51297 RVA: 0x000F6AD5 File Offset: 0x000F4CD5
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.MeetTrader;
			}
		}

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x0600C862 RID: 51298 RVA: 0x004956AC File Offset: 0x004938AC
		public override string DescriptionText
		{
			get
			{
				if (string.IsNullOrEmpty(this.TraderName))
				{
					return Localization.Get("challengeObjectiveMeetAnyTrader", false, null);
				}
				return Localization.Get("challengeObjectiveMeet", false, null) + " " + Localization.Get(this.TraderName, false, null) + ":";
			}
		}

		// Token: 0x0600C863 RID: 51299 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C864 RID: 51300 RVA: 0x004956FB File Offset: 0x004938FB
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.NPCMeet += this.Current_NPCMeet;
		}

		// Token: 0x0600C865 RID: 51301 RVA: 0x00495713 File Offset: 0x00493913
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.NPCMeet -= this.Current_NPCMeet;
		}

		// Token: 0x0600C866 RID: 51302 RVA: 0x0049572C File Offset: 0x0049392C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_NPCMeet(EntityNPC npc)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.TraderName == "" || npc.EntityName == this.TraderName)
			{
				int num = base.Current;
				base.Current = num + 1;
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
				}
			}
		}

		// Token: 0x0600C867 RID: 51303 RVA: 0x00495799 File Offset: 0x00493999
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("trader_name"))
			{
				this.TraderName = e.GetAttribute("trader_name");
			}
		}

		// Token: 0x0600C868 RID: 51304 RVA: 0x004957CA File Offset: 0x004939CA
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveMeetTrader
			{
				TraderName = this.TraderName
			};
		}

		// Token: 0x04009714 RID: 38676
		[PublicizedFrom(EAccessModifier.Private)]
		public string TraderName = "";
	}
}
