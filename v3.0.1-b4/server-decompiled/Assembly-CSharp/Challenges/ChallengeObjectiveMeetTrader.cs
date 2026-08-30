using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E4 RID: 6372
	[Preserve]
	public class ChallengeObjectiveMeetTrader : BaseChallengeObjective
	{
		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x0600C46E RID: 50286 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.MeetTrader;
			}
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x0600C46F RID: 50287 RVA: 0x00487480 File Offset: 0x00485680
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

		// Token: 0x0600C470 RID: 50288 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C471 RID: 50289 RVA: 0x004874CF File Offset: 0x004856CF
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.NPCMeet += this.Current_NPCMeet;
		}

		// Token: 0x0600C472 RID: 50290 RVA: 0x004874E7 File Offset: 0x004856E7
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.NPCMeet -= this.Current_NPCMeet;
		}

		// Token: 0x0600C473 RID: 50291 RVA: 0x00487500 File Offset: 0x00485700
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

		// Token: 0x0600C474 RID: 50292 RVA: 0x0048756D File Offset: 0x0048576D
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("trader_name"))
			{
				this.TraderName = e.GetAttribute("trader_name");
			}
		}

		// Token: 0x0600C475 RID: 50293 RVA: 0x0048759E File Offset: 0x0048579E
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveMeetTrader
			{
				TraderName = this.TraderName
			};
		}

		// Token: 0x0400943C RID: 37948
		[PublicizedFrom(EAccessModifier.Private)]
		public string TraderName = "";
	}
}
