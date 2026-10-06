using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018D4 RID: 6356
	[Preserve]
	public class RequirementHolding : BaseRequirement
	{
		// Token: 0x0600C42A RID: 50218 RVA: 0x00485020 File Offset: 0x00483220
		public override void SetupRequirement()
		{
			XUi xui = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui;
			string text = Localization.Get("RequirementHolding_keyword", false, null);
			this.expectedItem = ((base.ID != "" && base.ID != null) ? ItemClass.GetItem(base.ID, false) : xui.PlayerInventory.Toolbelt.Hand.BareHandItemValue);
			this.expectedItemClass = ((base.ID != "" && base.ID != null) ? ItemClass.GetItemClass(base.ID, false) : xui.PlayerInventory.Toolbelt.Hand.BareHandItem);
			if (base.ID == "" || base.ID == null)
			{
				base.Description = "Bare Hands";
				return;
			}
			base.Description = string.Format("{0} {1}", text, this.expectedItemClass.GetLocalizedItemName());
		}

		// Token: 0x0600C42B RID: 50219 RVA: 0x00485120 File Offset: 0x00483320
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory.Toolbelt.holdingItemStack.itemValue.type == this.expectedItem.type;
		}

		// Token: 0x0600C42C RID: 50220 RVA: 0x0048517C File Offset: 0x0048337C
		public override BaseRequirement Clone()
		{
			return new RequirementHolding
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x04009491 RID: 38033
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009492 RID: 38034
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;
	}
}
