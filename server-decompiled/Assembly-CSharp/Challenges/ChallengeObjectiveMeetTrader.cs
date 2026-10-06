using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001936 RID: 6454
	[Preserve]
	public class ChallengeObjectiveMeetTrader : BaseChallengeObjective
	{
		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x0600C74D RID: 51021 RVA: 0x000F659D File Offset: 0x000F479D
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.MeetTrader;
			}
		}

		// Token: 0x1700191F RID: 6431
		// (get) Token: 0x0600C74E RID: 51022 RVA: 0x00491CB8 File Offset: 0x0048FEB8
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

		// Token: 0x0600C74F RID: 51023 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C750 RID: 51024 RVA: 0x00491D07 File Offset: 0x0048FF07
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.NPCMeet += this.Current_NPCMeet;
		}

		// Token: 0x0600C751 RID: 51025 RVA: 0x00491D1F File Offset: 0x0048FF1F
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.NPCMeet -= this.Current_NPCMeet;
		}

		// Token: 0x0600C752 RID: 51026 RVA: 0x00491D38 File Offset: 0x0048FF38
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

		// Token: 0x0600C753 RID: 51027 RVA: 0x00491DA5 File Offset: 0x0048FFA5
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("trader_name"))
			{
				this.TraderName = e.GetAttribute("trader_name");
			}
		}

		// Token: 0x0600C754 RID: 51028 RVA: 0x00491DD6 File Offset: 0x0048FFD6
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveMeetTrader
			{
				TraderName = this.TraderName
			};
		}

		// Token: 0x0400962B RID: 38443
		[PublicizedFrom(EAccessModifier.Private)]
		public string TraderName = "";
	}
}
