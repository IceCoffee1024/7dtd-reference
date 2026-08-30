using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018A8 RID: 6312
	[Preserve]
	public class RequirementHolding : BaseRequirement
	{
		// Token: 0x0600C2D2 RID: 49874 RVA: 0x00480AE4 File Offset: 0x0047ECE4
		public override void SetupRequirement()
		{
			XUi xui = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui;
			string text = Localization.Get("RequirementHolding_keyword", false, null);
			this.expectedItem = ((base.ID != "" && base.ID != null) ? ItemClass.GetItem(base.ID, false) : xui.PlayerInventory.Toolbelt.GetBareHandItemValue());
			this.expectedItemClass = ((base.ID != "" && base.ID != null) ? ItemClass.GetItemClass(base.ID, false) : xui.PlayerInventory.Toolbelt.GetBareHandItem());
			if (base.ID == "" || base.ID == null)
			{
				base.Description = "Bare Hands";
				return;
			}
			base.Description = string.Format("{0} {1}", text, this.expectedItemClass.GetLocalizedItemName());
		}

		// Token: 0x0600C2D3 RID: 49875 RVA: 0x00480BD8 File Offset: 0x0047EDD8
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory.Toolbelt.holdingItemStack.itemValue.type == this.expectedItem.type;
		}

		// Token: 0x0600C2D4 RID: 49876 RVA: 0x00480C34 File Offset: 0x0047EE34
		public override BaseRequirement Clone()
		{
			return new RequirementHolding
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x040093CB RID: 37835
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040093CC RID: 37836
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;
	}
}
