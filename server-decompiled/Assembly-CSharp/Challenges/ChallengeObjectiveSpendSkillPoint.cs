using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E7 RID: 6375
	[Preserve]
	public class ChallengeObjectiveSpendSkillPoint : BaseChallengeObjective
	{
		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x0600C489 RID: 50313 RVA: 0x0048791F File Offset: 0x00485B1F
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.SpendSkillPoint;
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x0600C48A RID: 50314 RVA: 0x00487923 File Offset: 0x00485B23
		public override string DescriptionText
		{
			get
			{
				return string.Format(Localization.Get("ObjectiveSpendSkillPoints_keyword", false, null), Localization.Get("goAnyValue", false, null)) + ":";
			}
		}

		// Token: 0x0600C48B RID: 50315 RVA: 0x0048794C File Offset: 0x00485B4C
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C48C RID: 50316 RVA: 0x0048795A File Offset: 0x00485B5A
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupWindowOpen("Skills"));
		}

		// Token: 0x0600C48D RID: 50317 RVA: 0x0048797A File Offset: 0x00485B7A
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.SkillPointSpent += this.Current_SkillPointSpent;
		}

		// Token: 0x0600C48E RID: 50318 RVA: 0x00487992 File Offset: 0x00485B92
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.SkillPointSpent -= this.Current_SkillPointSpent;
		}

		// Token: 0x0600C48F RID: 50319 RVA: 0x004879AC File Offset: 0x00485BAC
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_SkillPointSpent(string skillName)
		{
			if (this.progressionName == "" || this.progressionName.EqualsCaseInsensitive(skillName))
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

		// Token: 0x0600C490 RID: 50320 RVA: 0x00487A0B File Offset: 0x00485C0B
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("skill_name"))
			{
				this.progressionName = e.GetAttribute("skill_name");
			}
		}

		// Token: 0x0600C491 RID: 50321 RVA: 0x00487A3C File Offset: 0x00485C3C
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveSpendSkillPoint
			{
				progressionName = this.progressionName
			};
		}

		// Token: 0x04009444 RID: 37956
		[PublicizedFrom(EAccessModifier.Private)]
		public string progressionName = "";
	}
}
