using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018F0 RID: 6384
	[Preserve]
	public class BaseRequirementObjectiveGroup
	{
		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x0600C4DC RID: 50396 RVA: 0x00488900 File Offset: 0x00486B00
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

		// Token: 0x0600C4DD RID: 50397 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void CreateRequirements()
		{
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x0600C4DE RID: 50398 RVA: 0x00488927 File Offset: 0x00486B27
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

		// Token: 0x0600C4DF RID: 50399 RVA: 0x0048894C File Offset: 0x00486B4C
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

		// Token: 0x0600C4E0 RID: 50400 RVA: 0x0048899C File Offset: 0x00486B9C
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

		// Token: 0x0600C4E1 RID: 50401 RVA: 0x004889FC File Offset: 0x00486BFC
		public void HandleRemoveHooks()
		{
			for (int i = 0; i < this.PhaseList.Count; i++)
			{
				this.PhaseList[i].HandleRemoveHooks();
			}
		}

		// Token: 0x0600C4E2 RID: 50402 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool HasPrerequisiteCondition()
		{
			return false;
		}

		// Token: 0x0600C4E3 RID: 50403 RVA: 0x00488A30 File Offset: 0x00486C30
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

		// Token: 0x0600C4E4 RID: 50404 RVA: 0x00488A6C File Offset: 0x00486C6C
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

		// Token: 0x0600C4E5 RID: 50405 RVA: 0x00488AB8 File Offset: 0x00486CB8
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

		// Token: 0x0600C4E6 RID: 50406 RVA: 0x0001FF5D File Offset: 0x0001E15D
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckPhaseStatus(int index)
		{
			return true;
		}

		// Token: 0x0600C4E7 RID: 50407 RVA: 0x00488B34 File Offset: 0x00486D34
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

		// Token: 0x0600C4E8 RID: 50408 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual BaseRequirementObjectiveGroup Clone()
		{
			return null;
		}

		// Token: 0x0600C4E9 RID: 50409 RVA: 0x00488B7B File Offset: 0x00486D7B
		public Recipe GetItemRecipe()
		{
			return this.PhaseList[this.currentIndex].GetItemRecipe();
		}

		// Token: 0x04009465 RID: 37989
		public Challenge Owner;

		// Token: 0x04009466 RID: 37990
		public List<RequirementGroupPhase> PhaseList = new List<RequirementGroupPhase>();

		// Token: 0x04009467 RID: 37991
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentIndex;

		// Token: 0x04009468 RID: 37992
		public bool NeedsPreRequisites;
	}
}
