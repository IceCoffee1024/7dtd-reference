using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200190A RID: 6410
	[Preserve]
	public class ChallengeObjectiveMeetTrader : BaseChallengeObjective
	{
		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x0600C5F5 RID: 50677 RVA: 0x000B93F5 File Offset: 0x000B75F5
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.MeetTrader;
			}
		}

		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x0600C5F6 RID: 50678 RVA: 0x0048D7CC File Offset: 0x0048B9CC
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

		// Token: 0x0600C5F7 RID: 50679 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C5F8 RID: 50680 RVA: 0x0048D81B File Offset: 0x0048BA1B
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.NPCMeet += this.Current_NPCMeet;
		}

		// Token: 0x0600C5F9 RID: 50681 RVA: 0x0048D833 File Offset: 0x0048BA33
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.NPCMeet -= this.Current_NPCMeet;
		}

		// Token: 0x0600C5FA RID: 50682 RVA: 0x0048D84C File Offset: 0x0048BA4C
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

		// Token: 0x0600C5FB RID: 50683 RVA: 0x0048D8B9 File Offset: 0x0048BAB9
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("trader_name"))
			{
				this.TraderName = e.GetAttribute("trader_name");
			}
		}

		// Token: 0x0600C5FC RID: 50684 RVA: 0x0048D8EA File Offset: 0x0048BAEA
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveMeetTrader
			{
				TraderName = this.TraderName
			};
		}

		// Token: 0x04009565 RID: 38245
		[PublicizedFrom(EAccessModifier.Private)]
		public string TraderName = "";
	}
}
