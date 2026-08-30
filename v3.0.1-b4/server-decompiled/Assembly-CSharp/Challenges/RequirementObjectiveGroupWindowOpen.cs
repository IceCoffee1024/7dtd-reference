using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018F7 RID: 6391
	[Preserve]
	public class RequirementObjectiveGroupWindowOpen : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C510 RID: 50448 RVA: 0x00489CEB File Offset: 0x00487EEB
		public RequirementObjectiveGroupWindowOpen(string windowOpen)
		{
			this.WindowOpen = windowOpen;
		}

		// Token: 0x0600C511 RID: 50449 RVA: 0x00489D08 File Offset: 0x00487F08
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

		// Token: 0x0600C512 RID: 50450 RVA: 0x00489D74 File Offset: 0x00487F74
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			GUIWindow window = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player).windowManager.GetWindow(this.WindowOpen);
			return window != null && !window.isShowing;
		}

		// Token: 0x0600C513 RID: 50451 RVA: 0x00489DC6 File Offset: 0x00487FC6
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupWindowOpen(this.WindowOpen);
		}

		// Token: 0x04009476 RID: 38006
		public string WindowOpen = "";
	}
}
