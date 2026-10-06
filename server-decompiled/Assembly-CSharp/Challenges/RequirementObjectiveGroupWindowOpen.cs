using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001949 RID: 6473
	[Preserve]
	public class RequirementObjectiveGroupWindowOpen : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C7EF RID: 51183 RVA: 0x00494523 File Offset: 0x00492723
		public RequirementObjectiveGroupWindowOpen(string windowOpen)
		{
			this.WindowOpen = windowOpen;
		}

		// Token: 0x0600C7F0 RID: 51184 RVA: 0x00494540 File Offset: 0x00492740
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

		// Token: 0x0600C7F1 RID: 51185 RVA: 0x004945AC File Offset: 0x004927AC
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			GUIWindow window = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).windowManager.GetWindow(this.WindowOpen);
			return window != null && !window.isShowing;
		}

		// Token: 0x0600C7F2 RID: 51186 RVA: 0x004945FE File Offset: 0x004927FE
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupWindowOpen(this.WindowOpen);
		}

		// Token: 0x04009665 RID: 38501
		public string WindowOpen = "";
	}
}
