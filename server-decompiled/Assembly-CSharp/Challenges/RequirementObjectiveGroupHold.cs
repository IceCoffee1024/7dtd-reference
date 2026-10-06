using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001947 RID: 6471
	[Preserve]
	public class RequirementObjectiveGroupHold : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C7E4 RID: 51172 RVA: 0x00494011 File Offset: 0x00492211
		public RequirementObjectiveGroupHold(string itemID)
		{
			this.ItemID = itemID;
		}

		// Token: 0x0600C7E5 RID: 51173 RVA: 0x0049402C File Offset: 0x0049222C
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

		// Token: 0x0600C7E6 RID: 51174 RVA: 0x00494098 File Offset: 0x00492298
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			return playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && holdingItem.Name != this.ItemID;
		}

		// Token: 0x0600C7E7 RID: 51175 RVA: 0x0049411B File Offset: 0x0049231B
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupHold(this.ItemID);
		}

		// Token: 0x04009663 RID: 38499
		public string ItemID = "";
	}
}
