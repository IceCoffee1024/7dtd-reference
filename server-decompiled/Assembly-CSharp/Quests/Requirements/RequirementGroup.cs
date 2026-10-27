using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001903 RID: 6403
	[Preserve]
	public class RequirementGroup : BaseRequirement
	{
		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x0600C538 RID: 50488 RVA: 0x0048882B File Offset: 0x00486A2B
		// (set) Token: 0x0600C539 RID: 50489 RVA: 0x00488833 File Offset: 0x00486A33
		public RequirementGroup.GroupOperator Operator { get; set; }

		// Token: 0x0600C53A RID: 50490 RVA: 0x0048883C File Offset: 0x00486A3C
		public override void SetupRequirement()
		{
			this.Operator = EnumUtils.Parse<RequirementGroup.GroupOperator>(base.Value, false);
			for (int i = 0; i < this.ChildRequirements.Count; i++)
			{
				this.ChildRequirements[i].OwnerQuest = base.OwnerQuest;
				this.ChildRequirements[i].SetupRequirement();
			}
			if (string.IsNullOrEmpty(base.ID))
			{
				if (this.ChildRequirements.Count > 0)
				{
					base.Description = this.ChildRequirements[0].Description;
					return;
				}
			}
			else
			{
				base.Description = Localization.Get(base.ID, false, null);
			}
		}

		// Token: 0x0600C53B RID: 50491 RVA: 0x004888E0 File Offset: 0x00486AE0
		public override bool CheckRequirement()
		{
			if (!base.OwnerQuest.Active)
			{
				return true;
			}
			bool flag = this.Operator == RequirementGroup.GroupOperator.AND;
			for (int i = 0; i < this.ChildRequirements.Count; i++)
			{
				bool flag2 = this.ChildRequirements[i].CheckRequirement();
				if (this.Operator == RequirementGroup.GroupOperator.AND)
				{
					if (!flag2)
					{
						return false;
					}
				}
				else if (this.Operator == RequirementGroup.GroupOperator.OR && flag2)
				{
					return true;
				}
			}
			return flag;
		}

		// Token: 0x0600C53C RID: 50492 RVA: 0x0048894C File Offset: 0x00486B4C
		public override BaseRequirement Clone()
		{
			RequirementGroup requirementGroup = new RequirementGroup();
			requirementGroup.ID = base.ID;
			requirementGroup.Value = base.Value;
			requirementGroup.Phase = base.Phase;
			for (int i = 0; i < this.ChildRequirements.Count; i++)
			{
				requirementGroup.ChildRequirements.Add(this.ChildRequirements[i].Clone());
			}
			return requirementGroup;
		}

		// Token: 0x04009575 RID: 38261
		public List<BaseRequirement> ChildRequirements = new List<BaseRequirement>();

		// Token: 0x02001904 RID: 6404
		public enum GroupOperator
		{
			// Token: 0x04009578 RID: 38264
			AND,
			// Token: 0x04009579 RID: 38265
			OR
		}
	}
}
