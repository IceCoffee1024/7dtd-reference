using System;
using System.Collections.Generic;

namespace Challenges
{
	// Token: 0x02001917 RID: 6423
	public class RequirementGroupPhase
	{
		// Token: 0x0600C672 RID: 50802 RVA: 0x0048EEF2 File Offset: 0x0048D0F2
		public void AddChallengeObjective(BaseChallengeObjective obj)
		{
			this.RequirementObjectiveList.Add(obj);
		}

		// Token: 0x0600C673 RID: 50803 RVA: 0x0048EF00 File Offset: 0x0048D100
		public void AddHooks()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				for (int j = 0; j < this.RequirementObjectiveList.Count; j++)
				{
					this.RequirementObjectiveList[j].HandleAddHooks();
				}
			}
		}

		// Token: 0x0600C674 RID: 50804 RVA: 0x0048EF4C File Offset: 0x0048D14C
		public bool HandleCheckStatus()
		{
			bool flag = false;
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				if (!this.RequirementObjectiveList[i].HandleCheckStatus())
				{
					flag = true;
				}
				this.RequirementObjectiveList[i].UpdateStatus();
			}
			return flag;
		}

		// Token: 0x0600C675 RID: 50805 RVA: 0x0048EF98 File Offset: 0x0048D198
		public void HandleRemoveHooks()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].HandleRemoveHooks();
			}
		}

		// Token: 0x0600C676 RID: 50806 RVA: 0x0048EFCC File Offset: 0x0048D1CC
		public void ResetComplete()
		{
			this.IsComplete = false;
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].ResetComplete();
			}
		}

		// Token: 0x0600C677 RID: 50807 RVA: 0x0048F008 File Offset: 0x0048D208
		public virtual void UpdateStatus()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].UpdateStatus();
			}
		}

		// Token: 0x0600C678 RID: 50808 RVA: 0x0048F03C File Offset: 0x0048D23C
		public void Clone(RequirementGroupPhase phase)
		{
			for (int i = 0; i < phase.RequirementObjectiveList.Count; i++)
			{
				BaseChallengeObjective baseChallengeObjective = phase.RequirementObjectiveList[i].Clone();
				if (this.RequirementObjectiveList == null)
				{
					this.RequirementObjectiveList = new List<BaseChallengeObjective>();
				}
				this.RequirementObjectiveList.Add(baseChallengeObjective);
			}
		}

		// Token: 0x0600C679 RID: 50809 RVA: 0x0048F090 File Offset: 0x0048D290
		public RequirementGroupPhase Clone()
		{
			RequirementGroupPhase requirementGroupPhase = new RequirementGroupPhase();
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				BaseChallengeObjective baseChallengeObjective = this.RequirementObjectiveList[i].Clone();
				if (this.RequirementObjectiveList == null)
				{
					this.RequirementObjectiveList = new List<BaseChallengeObjective>();
				}
				requirementGroupPhase.RequirementObjectiveList.Add(baseChallengeObjective);
			}
			return requirementGroupPhase;
		}

		// Token: 0x0600C67A RID: 50810 RVA: 0x0048F0EC File Offset: 0x0048D2EC
		public Recipe GetItemRecipe()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				Recipe recipeItem = this.RequirementObjectiveList[i].GetRecipeItem();
				if (recipeItem != null)
				{
					return recipeItem;
				}
			}
			return null;
		}

		// Token: 0x04009592 RID: 38290
		public List<BaseChallengeObjective> RequirementObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x04009593 RID: 38291
		public bool IsComplete;
	}
}
