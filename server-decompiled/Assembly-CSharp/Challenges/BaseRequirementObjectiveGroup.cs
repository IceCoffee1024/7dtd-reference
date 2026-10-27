using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001973 RID: 6515
	[Preserve]
	public class BaseRequirementObjectiveGroup
	{
		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x0600C8CF RID: 51407 RVA: 0x00496B2C File Offset: 0x00494D2C
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

		// Token: 0x0600C8D0 RID: 51408 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void CreateRequirements()
		{
		}

		// Token: 0x17001983 RID: 6531
		// (get) Token: 0x0600C8D1 RID: 51409 RVA: 0x00496B53 File Offset: 0x00494D53
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

		// Token: 0x0600C8D2 RID: 51410 RVA: 0x00496B78 File Offset: 0x00494D78
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

		// Token: 0x0600C8D3 RID: 51411 RVA: 0x00496BC8 File Offset: 0x00494DC8
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

		// Token: 0x0600C8D4 RID: 51412 RVA: 0x00496C28 File Offset: 0x00494E28
		public void HandleRemoveHooks()
		{
			for (int i = 0; i < this.PhaseList.Count; i++)
			{
				this.PhaseList[i].HandleRemoveHooks();
			}
		}

		// Token: 0x0600C8D5 RID: 51413 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool HasPrerequisiteCondition()
		{
			return false;
		}

		// Token: 0x0600C8D6 RID: 51414 RVA: 0x00496C5C File Offset: 0x00494E5C
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

		// Token: 0x0600C8D7 RID: 51415 RVA: 0x00496C98 File Offset: 0x00494E98
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

		// Token: 0x0600C8D8 RID: 51416 RVA: 0x00496CE4 File Offset: 0x00494EE4
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

		// Token: 0x0600C8D9 RID: 51417 RVA: 0x00020119 File Offset: 0x0001E319
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool CheckPhaseStatus(int index)
		{
			return true;
		}

		// Token: 0x0600C8DA RID: 51418 RVA: 0x00496D60 File Offset: 0x00494F60
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

		// Token: 0x0600C8DB RID: 51419 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual BaseRequirementObjectiveGroup Clone()
		{
			return null;
		}

		// Token: 0x0600C8DC RID: 51420 RVA: 0x00496DA7 File Offset: 0x00494FA7
		public Recipe GetItemRecipe()
		{
			return this.PhaseList[this.currentIndex].GetItemRecipe();
		}

		// Token: 0x0400973D RID: 38717
		public Challenge Owner;

		// Token: 0x0400973E RID: 38718
		public List<RequirementGroupPhase> PhaseList = new List<RequirementGroupPhase>();

		// Token: 0x0400973F RID: 38719
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentIndex;

		// Token: 0x04009740 RID: 38720
		public bool NeedsPreRequisites;
	}
}
