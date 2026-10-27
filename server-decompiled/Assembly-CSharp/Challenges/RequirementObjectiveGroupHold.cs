using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001978 RID: 6520
	[Preserve]
	public class RequirementObjectiveGroupHold : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C8F8 RID: 51448 RVA: 0x00497A05 File Offset: 0x00495C05
		public RequirementObjectiveGroupHold(string itemID)
		{
			this.ItemID = itemID;
		}

		// Token: 0x0600C8F9 RID: 51449 RVA: 0x00497A20 File Offset: 0x00495C20
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

		// Token: 0x0600C8FA RID: 51450 RVA: 0x00497A8C File Offset: 0x00495C8C
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			return playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && holdingItem.Name != this.ItemID;
		}

		// Token: 0x0600C8FB RID: 51451 RVA: 0x00497B0F File Offset: 0x00495D0F
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupHold(this.ItemID);
		}

		// Token: 0x0400974C RID: 38732
		public string ItemID = "";
	}
}
