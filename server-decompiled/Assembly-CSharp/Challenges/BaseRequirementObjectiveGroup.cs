using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001916 RID: 6422
	[Preserve]
	public class BaseRequirementObjectiveGroup
	{
		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x0600C663 RID: 50787 RVA: 0x0048EC4C File Offset: 0x0048CE4C
		public int Count
		{
			get
			{
				if (this.PhaseList == null)
				{
					return 0;
				}
				return this.PhaseList[this.currentIndex].RequirementObjectiveList.Count;
			}
		}

		// Token: 0x0600C664 RID: 50788 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void CreateRequirements()
		{
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x0600C665 RID: 50789 RVA: 0x0048EC73 File Offset: 0x0048CE73
		public List<BaseChallengeObjective> CurrentObjectiveList
		{
			get
			{
				if (this.PhaseList == null)
				{
					return null;
				}
				return this.PhaseList[this.currentIndex].RequirementObjectiveList;
			}
		}

		// Token: 0x0600C666 RID: 50790 RVA: 0x0048EC98 File Offset: 0x0048CE98
		public void HandleAddHooks()
		{
			if (this.PhaseList.Count == 0)
			{
				this.CreateRequirements();
			}
			for (int i = 0; i < this.PhaseList.Count; i++)
			{
				this.PhaseList[i].AddHooks();
			}
			this.CheckPrerequisites();
		}

		// Token: 0x0600C667 RID: 50791 RVA: 0x0048ECE8 File Offset: 0x0048CEE8
		public void CheckPrerequisites()
		{
			if (this.PhaseList.Count == 0)
			{
				this.CreateRequirements();
			}
			this.NeedsPreRequisites = false;
			for (int i = 0; i < this.PhaseList.Count; i++)
			{
				if (this.PhaseList[i].HandleCheckStatus())
				{
					this.currentIndex = i;
					this.NeedsPreRequisites = true;
					return;
				}
			}
		}

		// Token: 0x0600C668 RID: 50792 RVA: 0x0048ED48 File Offset: 0x0048CF48
		public void HandleRemoveHooks()
		{
			for (int i = 0; i < this.PhaseList.Count; i++)
			{
				this.PhaseList[i].HandleRemoveHooks();
			}
		}

		// Token: 0x0600C669 RID: 50793 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool HasPrerequisiteCondition()
		{
			return false;
		}

		// Token: 0x0600C66A RID: 50794 RVA: 0x0048ED7C File Offset: 0x0048CF7C
		public void ResetObjectives()
		{
			if (this.PhaseList != null)
			{
				for (int i = 0; i < this.PhaseList.Count; i++)
				{
					this.PhaseList[i].ResetComplete();
				}
			}
		}

		// Token: 0x0600C66B RID: 50795 RVA: 0x0048EDB8 File Offset: 0x0048CFB8
		public void ClonePhases(BaseRequirementObjectiveGroup group)
		{
			if (group.PhaseList != null)
			{
				for (int i = 0; i < group.PhaseList.Count; i++)
				{
					RequirementGroupPhase requirementGroupPhase = group.PhaseList[i].Clone();
					this.PhaseList.Add(requirementGroupPhase);
				}
			}
		}

		// Token: 0x0600C66C RID: 50796 RVA: 0x0048EE04 File Offset: 0x0048D004
		public virtual bool HandleCheckStatus()
		{
			if (this.PhaseList.Count == 0)
			{
				this.CreateRequirements();
			}
			this.ResetObjectives();
			for (int i = 0; i < this.PhaseList.Count; i++)
			{
				if (this.CheckPhaseStatus(i) && this.PhaseList[i].HandleCheckStatus())
				{
					this.currentIndex = i;
					this.NeedsPreRequisites = true;
					return true;
				}
				this.PhaseList[i].IsComplete = true;
			}
			return false;
		}

		// Token: 0x0600C66D RID: 50797 RVA: 0x00020021 File Offset: 0x0001E221
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckPhaseStatus(int index)
		{
			return true;
		}

		// Token: 0x0600C66E RID: 50798 RVA: 0x0048EE80 File Offset: 0x0048D080
		public virtual void UpdateStatus()
		{
			for (int i = 0; i < this.PhaseList.Count; i++)
			{
				if (!this.PhaseList[i].IsComplete)
				{
					this.PhaseList[i].UpdateStatus();
				}
			}
		}

		// Token: 0x0600C66F RID: 50799 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual BaseRequirementObjectiveGroup Clone()
		{
			return null;
		}

		// Token: 0x0600C670 RID: 50800 RVA: 0x0048EEC7 File Offset: 0x0048D0C7
		public Recipe GetItemRecipe()
		{
			return this.PhaseList[this.currentIndex].GetItemRecipe();
		}

		// Token: 0x0400958E RID: 38286
		public Challenge Owner;

		// Token: 0x0400958F RID: 38287
		public List<RequirementGroupPhase> PhaseList = new List<RequirementGroupPhase>();

		// Token: 0x04009590 RID: 38288
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentIndex;

		// Token: 0x04009591 RID: 38289
		public bool NeedsPreRequisites;
	}
}
