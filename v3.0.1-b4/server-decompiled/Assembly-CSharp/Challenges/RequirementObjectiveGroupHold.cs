using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018F5 RID: 6389
	[Preserve]
	public class RequirementObjectiveGroupHold : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C505 RID: 50437 RVA: 0x004897D9 File Offset: 0x004879D9
		public RequirementObjectiveGroupHold(string itemID)
		{
			this.ItemID = itemID;
		}

		// Token: 0x0600C506 RID: 50438 RVA: 0x004897F4 File Offset: 0x004879F4
		public override void CreateRequirements()
		{
			if (this.PhaseList == null)
			{
				this.PhaseList = new List<RequirementGroupPhase>();
			}
			RequirementGroupPhase requirementGroupPhase = new RequirementGroupPhase();
			ChallengeObjectiveHold challengeObjectiveHold = new ChallengeObjectiveHold();
			challengeObjectiveHold.Owner = this.Owner;
			challengeObjectiveHold.itemClassID = this.ItemID;
			challengeObjectiveHold.IsRequirement = true;
			challengeObjectiveHold.MaxCount = 1;
			challengeObjectiveHold.Init();
			requirementGroupPhase.AddChallengeObjective(challengeObjectiveHold);
			this.PhaseList.Add(requirementGroupPhase);
		}

		// Token: 0x0600C507 RID: 50439 RVA: 0x00489860 File Offset: 0x00487A60
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			return playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && holdingItem.Name != this.ItemID;
		}

		// Token: 0x0600C508 RID: 50440 RVA: 0x004898E3 File Offset: 0x00487AE3
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupHold(this.ItemID);
		}

		// Token: 0x04009474 RID: 38004
		public string ItemID = "";
	}
}
