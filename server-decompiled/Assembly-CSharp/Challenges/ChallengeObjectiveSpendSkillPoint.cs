using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200190D RID: 6413
	[Preserve]
	public class ChallengeObjectiveSpendSkillPoint : BaseChallengeObjective
	{
		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x0600C610 RID: 50704 RVA: 0x0048DC6B File Offset: 0x0048BE6B
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.SpendSkillPoint;
			}
		}

		// Token: 0x17001883 RID: 6275
		// (get) Token: 0x0600C611 RID: 50705 RVA: 0x0048DC6F File Offset: 0x0048BE6F
		public override string DescriptionText
		{
			get
			{
				return string.Format(Localization.Get("ObjectiveSpendSkillPoints_keyword", false, null), Localization.Get("goAnyValue", false, null)) + ":";
			}
		}

		// Token: 0x0600C612 RID: 50706 RVA: 0x0048DC98 File Offset: 0x0048BE98
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C613 RID: 50707 RVA: 0x0048DCA6 File Offset: 0x0048BEA6
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupWindowOpen("Skills"));
		}

		// Token: 0x0600C614 RID: 50708 RVA: 0x0048DCC6 File Offset: 0x0048BEC6
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.SkillPointSpent += this.Current_SkillPointSpent;
		}

		// Token: 0x0600C615 RID: 50709 RVA: 0x0048DCDE File Offset: 0x0048BEDE
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.SkillPointSpent -= this.Current_SkillPointSpent;
		}

		// Token: 0x0600C616 RID: 50710 RVA: 0x0048DCF8 File Offset: 0x0048BEF8
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

		// Token: 0x0600C617 RID: 50711 RVA: 0x0048DD57 File Offset: 0x0048BF57
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("skill_name"))
			{
				this.progressionName = e.GetAttribute("skill_name");
			}
		}

		// Token: 0x0600C618 RID: 50712 RVA: 0x0048DD88 File Offset: 0x0048BF88
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveSpendSkillPoint
			{
				progressionName = this.progressionName
			};
		}

		// Token: 0x0400956D RID: 38253
		[PublicizedFrom(EAccessModifier.Private)]
		public string progressionName = "";
	}
}
