using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001942 RID: 6466
	[Preserve]
	public class BaseRequirementObjectiveGroup
	{
		// Token: 0x1700193A RID: 6458
		// (get) Token: 0x0600C7BB RID: 51131 RVA: 0x00493138 File Offset: 0x00491338
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

		// Token: 0x0600C7BC RID: 51132 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void CreateRequirements()
		{
		}

		// Token: 0x1700193B RID: 6459
		// (get) Token: 0x0600C7BD RID: 51133 RVA: 0x0049315F File Offset: 0x0049135F
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

		// Token: 0x0600C7BE RID: 51134 RVA: 0x00493184 File Offset: 0x00491384
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

		// Token: 0x0600C7BF RID: 51135 RVA: 0x004931D4 File Offset: 0x004913D4
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

		// Token: 0x0600C7C0 RID: 51136 RVA: 0x00493234 File Offset: 0x00491434
		public void HandleRemoveHooks()
		{
			for (int i = 0; i < this.PhaseList.Count; i++)
			{
				this.PhaseList[i].HandleRemoveHooks();
			}
		}

		// Token: 0x0600C7C1 RID: 51137 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool HasPrerequisiteCondition()
		{
			return false;
		}

		// Token: 0x0600C7C2 RID: 51138 RVA: 0x00493268 File Offset: 0x00491468
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

		// Token: 0x0600C7C3 RID: 51139 RVA: 0x004932A4 File Offset: 0x004914A4
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

		// Token: 0x0600C7C4 RID: 51140 RVA: 0x004932F0 File Offset: 0x004914F0
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

		// Token: 0x0600C7C5 RID: 51141 RVA: 0x00020119 File Offset: 0x0001E319
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckPhaseStatus(int index)
		{
			return true;
		}

		// Token: 0x0600C7C6 RID: 51142 RVA: 0x0049336C File Offset: 0x0049156C
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

		// Token: 0x0600C7C7 RID: 51143 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual BaseRequirementObjectiveGroup Clone()
		{
			return null;
		}

		// Token: 0x0600C7C8 RID: 51144 RVA: 0x004933B3 File Offset: 0x004915B3
		public Recipe GetItemRecipe()
		{
			return this.PhaseList[this.currentIndex].GetItemRecipe();
		}

		// Token: 0x04009654 RID: 38484
		public Challenge Owner;

		// Token: 0x04009655 RID: 38485
		public List<RequirementGroupPhase> PhaseList = new List<RequirementGroupPhase>();

		// Token: 0x04009656 RID: 38486
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentIndex;

		// Token: 0x04009657 RID: 38487
		public bool NeedsPreRequisites;
	}
}
