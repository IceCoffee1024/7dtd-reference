using System;
using System.Collections.Generic;

namespace Challenges
{
	// Token: 0x02001974 RID: 6516
	public class RequirementGroupPhase
	{
		// Token: 0x0600C8DE RID: 51422 RVA: 0x00496DD2 File Offset: 0x00494FD2
		public void AddChallengeObjective(BaseChallengeObjective obj)
		{
			this.RequirementObjectiveList.Add(obj);
		}

		// Token: 0x0600C8DF RID: 51423 RVA: 0x00496DE0 File Offset: 0x00494FE0
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

		// Token: 0x0600C8E0 RID: 51424 RVA: 0x00496E2C File Offset: 0x0049502C
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

		// Token: 0x0600C8E1 RID: 51425 RVA: 0x00496E78 File Offset: 0x00495078
		public void HandleRemoveHooks()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].HandleRemoveHooks();
			}
		}

		// Token: 0x0600C8E2 RID: 51426 RVA: 0x00496EAC File Offset: 0x004950AC
		public void ResetComplete()
		{
			this.IsComplete = false;
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].ResetComplete();
			}
		}

		// Token: 0x0600C8E3 RID: 51427 RVA: 0x00496EE8 File Offset: 0x004950E8
		public virtual void UpdateStatus()
		{
			for (int i = 0; i < this.RequirementObjectiveList.Count; i++)
			{
				this.RequirementObjectiveList[i].UpdateStatus();
			}
		}

		// Token: 0x0600C8E4 RID: 51428 RVA: 0x00496F1C File Offset: 0x0049511C
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

		// Token: 0x0600C8E5 RID: 51429 RVA: 0x00496F70 File Offset: 0x00495170
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

		// Token: 0x0600C8E6 RID: 51430 RVA: 0x00496FCC File Offset: 0x004951CC
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

		// Token: 0x04009741 RID: 38721
		public List<BaseChallengeObjective> RequirementObjectiveList = new List<BaseChallengeObjective>();

		// Token: 0x04009742 RID: 38722
		public bool IsComplete;
	}
}
