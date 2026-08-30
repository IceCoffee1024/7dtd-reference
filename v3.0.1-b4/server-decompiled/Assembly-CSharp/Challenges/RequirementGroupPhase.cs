using System;
using System.Collections.Generic;

namespace Challenges
{
	// Token: 0x020018F1 RID: 6385
	public class RequirementGroupPhase
	{
		// Token: 0x0600C4EB RID: 50411 RVA: 0x00488BA6 File Offset: 0x00486DA6
		public void AddChallengeObjective(BaseChallengeObjective obj)
		{
			this.RequirementObjectiveList.Add(obj);
		}

		// Token: 0x0600C4EC RID: 50412 RVA: 0x00488BB4 File Offset: 0x00486DB4
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

		// Token: 0x0600C4ED RID: 50413 RVA: 0x00488C00 File Offset: 0x00486E00
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

		// Token: 0x0600C4EE RID: 50414 RVA: 0x00488C4C File Offset: 0x00486E4C
		public void HandleRemoveHooks()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].HandleRemoveHooks();
			}
		}

		// Token: 0x0600C4EF RID: 50415 RVA: 0x00488C80 File Offset: 0x00486E80
		public void ResetComplete()
		{
			this.IsComplete = false;
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].ResetComplete();
			}
		}

		// Token: 0x0600C4F0 RID: 50416 RVA: 0x00488CBC File Offset: 0x00486EBC
		public virtual void UpdateStatus()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].UpdateStatus();
			}
		}

		// Token: 0x0600C4F1 RID: 50417 RVA: 0x00488CF0 File Offset: 0x00486EF0
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

		// Token: 0x0600C4F2 RID: 50418 RVA: 0x00488D44 File Offset: 0x00486F44
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

		// Token: 0x0600C4F3 RID: 50419 RVA: 0x00488DA0 File Offset: 0x00486FA0
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

		// Token: 0x04009469 RID: 37993
		public List<BaseChallengeObjective> RequirementObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x0400946A RID: 37994
		public bool IsComplete;
	}
}
