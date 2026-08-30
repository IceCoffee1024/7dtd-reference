using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001880 RID: 6272
	[Preserve]
	public class RequirementGroup : BaseRequirement
	{
		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x0600C145 RID: 49477 RVA: 0x0047A817 File Offset: 0x00478A17
		// (set) Token: 0x0600C146 RID: 49478 RVA: 0x0047A81F File Offset: 0x00478A1F
		public RequirementGroup.GroupOperator Operator { get; set; }

		// Token: 0x0600C147 RID: 49479 RVA: 0x0047A828 File Offset: 0x00478A28
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

		// Token: 0x0600C148 RID: 49480 RVA: 0x0047A8CC File Offset: 0x00478ACC
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

		// Token: 0x0600C149 RID: 49481 RVA: 0x0047A938 File Offset: 0x00478B38
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

		// Token: 0x0400929D RID: 37533
		public List<BaseRequirement> ChildRequirements = new List<BaseRequirement>();

		// Token: 0x02001881 RID: 6273
		public enum GroupOperator
		{
			// Token: 0x040092A0 RID: 37536
			AND,
			// Token: 0x040092A1 RID: 37537
			OR
		}
	}
}
