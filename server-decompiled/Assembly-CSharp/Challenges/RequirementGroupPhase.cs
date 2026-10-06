using System;
using System.Collections.Generic;

namespace Challenges
{
	// Token: 0x02001943 RID: 6467
	public class RequirementGroupPhase
	{
		// Token: 0x0600C7CA RID: 51146 RVA: 0x004933DE File Offset: 0x004915DE
		public void AddChallengeObjective(BaseChallengeObjective obj)
		{
			this.RequirementObjectiveList.Add(obj);
		}

		// Token: 0x0600C7CB RID: 51147 RVA: 0x004933EC File Offset: 0x004915EC
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

		// Token: 0x0600C7CC RID: 51148 RVA: 0x00493438 File Offset: 0x00491638
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

		// Token: 0x0600C7CD RID: 51149 RVA: 0x00493484 File Offset: 0x00491684
		public void HandleRemoveHooks()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].HandleRemoveHooks();
			}
		}

		// Token: 0x0600C7CE RID: 51150 RVA: 0x004934B8 File Offset: 0x004916B8
		public void ResetComplete()
		{
			this.IsComplete = false;
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].ResetComplete();
			}
		}

		// Token: 0x0600C7CF RID: 51151 RVA: 0x004934F4 File Offset: 0x004916F4
		public virtual void UpdateStatus()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].UpdateStatus();
			}
		}

		// Token: 0x0600C7D0 RID: 51152 RVA: 0x00493528 File Offset: 0x00491728
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

		// Token: 0x0600C7D1 RID: 51153 RVA: 0x0049357C File Offset: 0x0049177C
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

		// Token: 0x0600C7D2 RID: 51154 RVA: 0x004935D8 File Offset: 0x004917D8
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

		// Token: 0x04009658 RID: 38488
		public List<BaseChallengeObjective> RequirementObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x04009659 RID: 38489
		public bool IsComplete;
	}
}
