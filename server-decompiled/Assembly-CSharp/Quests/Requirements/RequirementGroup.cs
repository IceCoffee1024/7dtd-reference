using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018A6 RID: 6310
	[Preserve]
	public class RequirementGroup : BaseRequirement
	{
		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x0600C2CC RID: 49868 RVA: 0x00480943 File Offset: 0x0047EB43
		// (set) Token: 0x0600C2CD RID: 49869 RVA: 0x0048094B File Offset: 0x0047EB4B
		public RequirementGroup.GroupOperator Operator { get; set; }

		// Token: 0x0600C2CE RID: 49870 RVA: 0x00480954 File Offset: 0x0047EB54
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

		// Token: 0x0600C2CF RID: 49871 RVA: 0x004809F8 File Offset: 0x0047EBF8
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

		// Token: 0x0600C2D0 RID: 49872 RVA: 0x00480A64 File Offset: 0x0047EC64
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

		// Token: 0x040093C6 RID: 37830
		public List<BaseRequirement> ChildRequirements = new List<BaseRequirement>();

		// Token: 0x020018A7 RID: 6311
		public enum GroupOperator
		{
			// Token: 0x040093C9 RID: 37833
			AND,
			// Token: 0x040093CA RID: 37834
			OR
		}
	}
}
