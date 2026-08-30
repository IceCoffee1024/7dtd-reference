using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001882 RID: 6274
	[Preserve]
	public class RequirementHolding : BaseRequirement
	{
		// Token: 0x0600C14B RID: 49483 RVA: 0x0047A9B8 File Offset: 0x00478BB8
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

		// Token: 0x0600C14C RID: 49484 RVA: 0x0047AAAC File Offset: 0x00478CAC
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory.Toolbelt.holdingItemStack.itemValue.type == this.expectedItem.type;
		}

		// Token: 0x0600C14D RID: 49485 RVA: 0x0047AB08 File Offset: 0x00478D08
		public override BaseRequirement Clone()
		{
			return new RequirementHolding
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x040092A2 RID: 37538
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040092A3 RID: 37539
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;
	}
}
