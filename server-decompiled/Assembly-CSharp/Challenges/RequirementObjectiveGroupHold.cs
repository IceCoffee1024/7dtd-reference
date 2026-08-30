using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200191B RID: 6427
	[Preserve]
	public class RequirementObjectiveGroupHold : BaseRequirementObjectiveGroup
	{
		// Token: 0x0600C68C RID: 50828 RVA: 0x0048FB25 File Offset: 0x0048DD25
		public RequirementObjectiveGroupHold(string itemID)
		{
			this.ItemID = itemID;
		}

		// Token: 0x0600C68D RID: 50829 RVA: 0x0048FB40 File Offset: 0x0048DD40
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

		// Token: 0x0600C68E RID: 50830 RVA: 0x0048FBAC File Offset: 0x0048DDAC
		public override bool HasPrerequisiteCondition()
		{
			EntityPlayerLocal player = this.Owner.Owner.Player;
			LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(player).xui.PlayerInventory;
			ItemClass holdingItem = this.Owner.Owner.Player.inventory.holdingItem;
			return playerInventory.HasItem(ItemClass.GetItem(this.ItemID, false)) && holdingItem.Name != this.ItemID;
		}

		// Token: 0x0600C68F RID: 50831 RVA: 0x0048FC2F File Offset: 0x0048DE2F
		public override BaseRequirementObjectiveGroup Clone()
		{
			return new RequirementObjectiveGroupHold(this.ItemID);
		}

		// Token: 0x0400959D RID: 38301
		public string ItemID = "";
	}
}
