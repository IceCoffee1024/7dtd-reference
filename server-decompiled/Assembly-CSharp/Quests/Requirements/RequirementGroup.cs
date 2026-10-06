using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018D2 RID: 6354
	[Preserve]
	public class RequirementGroup : BaseRequirement
	{
		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x0600C424 RID: 50212 RVA: 0x00484E7F File Offset: 0x0048307F
		// (set) Token: 0x0600C425 RID: 50213 RVA: 0x00484E87 File Offset: 0x00483087
		public RequirementGroup.GroupOperator Operator { get; set; }

		// Token: 0x0600C426 RID: 50214 RVA: 0x00484E90 File Offset: 0x00483090
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

		// Token: 0x0600C427 RID: 50215 RVA: 0x00484F34 File Offset: 0x00483134
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

		// Token: 0x0600C428 RID: 50216 RVA: 0x00484FA0 File Offset: 0x004831A0
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

		// Token: 0x0400948C RID: 38028
		public List<BaseRequirement> ChildRequirements = new List<BaseRequirement>();

		// Token: 0x020018D3 RID: 6355
		public enum GroupOperator
		{
			// Token: 0x0400948F RID: 38031
			AND,
			// Token: 0x04009490 RID: 38032
			OR
		}
	}
}
