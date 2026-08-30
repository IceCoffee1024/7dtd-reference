using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200191D RID: 6429
	[Preserve]
	public class RequirementObjectiveGroupWindowOpen : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C697 RID: 50839 RVA: 0x00490037 File Offset: 0x0048E237
		public RequirementObjectiveGroupWindowOpen(string windowOpen)
		{
			this.WindowOpen = windowOpen;
		}

		// Token: 0x0600C698 RID: 50840 RVA: 0x00490054 File Offset: 0x0048E254
		public override void CreateRequirements()
		{
			if (this.PhaseList == null)
			{
				this.PhaseList = new List<RequirementGroupPhase>();
			}
			RequirementGroupPhase requirementGroupPhase = new RequirementGroupPhase();
			ChallengeObjectiveWindowOpen challengeObjectiveWindowOpen = new ChallengeObjectiveWindowOpen();
			challengeObjectiveWindowOpen.WindowName = this.WindowOpen;
			challengeObjectiveWindowOpen.Parent = this;
			challengeObjectiveWindowOpen.Owner = this.Owner;
			challengeObjectiveWindowOpen.IsRequirement = true;
			challengeObjectiveWindowOpen.Init();
			requirementGroupPhase.AddChallengeObjective(challengeObjectiveWindowOpen);
			this.PhaseList.Add(requirementGroupPhase);
		}

		// Token: 0x0600C699 RID: 50841 RVA: 0x004900C0 File Offset: 0x0048E2C0
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			GUIWindow window = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).windowManager.GetWindow(this.WindowOpen);
			return window != null && !window.isShowing;
		}

		// Token: 0x0600C69A RID: 50842 RVA: 0x00490112 File Offset: 0x0048E312
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupWindowOpen(this.WindowOpen);
		}

		// Token: 0x0400959F RID: 38303
		public string WindowOpen = "";
	}
}
