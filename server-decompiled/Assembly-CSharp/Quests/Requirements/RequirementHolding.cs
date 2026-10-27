using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001905 RID: 6405
	[Preserve]
	public class RequirementHolding : BaseRequirement
	{
		// Token: 0x0600C53E RID: 50494 RVA: 0x004889CC File Offset: 0x00486BCC
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

		// Token: 0x0600C53F RID: 50495 RVA: 0x00488ACC File Offset: 0x00486CCC
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory.Toolbelt.holdingItemStack.itemValue.type == this.expectedItem.type;
		}

		// Token: 0x0600C540 RID: 50496 RVA: 0x00488B28 File Offset: 0x00486D28
		public override BaseRequirement Clone()
		{
			return new RequirementHolding
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x0400957A RID: 38266
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x0400957B RID: 38267
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;
	}
}
