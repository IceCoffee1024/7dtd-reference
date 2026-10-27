using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200196A RID: 6506
	[Preserve]
	public class ChallengeObjectiveSpendSkillPoint : BaseChallengeObjective
	{
		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x0600C87C RID: 51324 RVA: 0x00495B4B File Offset: 0x00493D4B
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.SpendSkillPoint;
			}
		}

		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x0600C87D RID: 51325 RVA: 0x00495B4F File Offset: 0x00493D4F
		public override string DescriptionText
		{
			get
			{
				return string.Format(Localization.Get("ObjectiveSpendSkillPoints_keyword", false, null), Localization.Get("goAnyValue", false, null)) + ":";
			}
		}

		// Token: 0x0600C87E RID: 51326 RVA: 0x00495B78 File Offset: 0x00493D78
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C87F RID: 51327 RVA: 0x00495B86 File Offset: 0x00493D86
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupWindowOpen("Skills"));
		}

		// Token: 0x0600C880 RID: 51328 RVA: 0x00495BA6 File Offset: 0x00493DA6
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.SkillPointSpent += this.Current_SkillPointSpent;
		}

		// Token: 0x0600C881 RID: 51329 RVA: 0x00495BBE File Offset: 0x00493DBE
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.SkillPointSpent -= this.Current_SkillPointSpent;
		}

		// Token: 0x0600C882 RID: 51330 RVA: 0x00495BD8 File Offset: 0x00493DD8
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

		// Token: 0x0600C883 RID: 51331 RVA: 0x00495C37 File Offset: 0x00493E37
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("skill_name"))
			{
				this.progressionName = e.GetAttribute("skill_name");
			}
		}

		// Token: 0x0600C884 RID: 51332 RVA: 0x00495C68 File Offset: 0x00493E68
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveSpendSkillPoint
			{
				progressionName = this.progressionName
			};
		}

		// Token: 0x0400971C RID: 38684
		[PublicizedFrom(EAccessModifier.Private)]
		public string progressionName = "";
	}
}
