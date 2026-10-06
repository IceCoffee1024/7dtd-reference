using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001939 RID: 6457
	[Preserve]
	public class ChallengeObjectiveSpendSkillPoint : BaseChallengeObjective
	{
		// Token: 0x17001924 RID: 6436
		// (get) Token: 0x0600C768 RID: 51048 RVA: 0x00492157 File Offset: 0x00490357
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.SpendSkillPoint;
			}
		}

		// Token: 0x17001925 RID: 6437
		// (get) Token: 0x0600C769 RID: 51049 RVA: 0x0049215B File Offset: 0x0049035B
		public override string DescriptionText
		{
			get
			{
				return string.Format(Localization.Get("ObjectiveSpendSkillPoints_keyword", false, null), Localization.Get("goAnyValue", false, null)) + ":";
			}
		}

		// Token: 0x0600C76A RID: 51050 RVA: 0x00492184 File Offset: 0x00490384
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C76B RID: 51051 RVA: 0x00492192 File Offset: 0x00490392
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupWindowOpen("Skills"));
		}

		// Token: 0x0600C76C RID: 51052 RVA: 0x004921B2 File Offset: 0x004903B2
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.SkillPointSpent += this.Current_SkillPointSpent;
		}

		// Token: 0x0600C76D RID: 51053 RVA: 0x004921CA File Offset: 0x004903CA
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.SkillPointSpent -= this.Current_SkillPointSpent;
		}

		// Token: 0x0600C76E RID: 51054 RVA: 0x004921E4 File Offset: 0x004903E4
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

		// Token: 0x0600C76F RID: 51055 RVA: 0x00492243 File Offset: 0x00490443
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("skill_name"))
			{
				this.progressionName = e.GetAttribute("skill_name");
			}
		}

		// Token: 0x0600C770 RID: 51056 RVA: 0x00492274 File Offset: 0x00490474
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveSpendSkillPoint
			{
				progressionName = this.progressionName
			};
		}

		// Token: 0x04009633 RID: 38451
		[PublicizedFrom(EAccessModifier.Private)]
		public string progressionName = "";
	}
}
