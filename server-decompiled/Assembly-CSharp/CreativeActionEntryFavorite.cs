using System;
using UnityEngine.Scripting;

// Token: 0x02000D63 RID: 3427
[Preserve]
public class CreativeActionEntryFavorite : BaseItemActionEntry
{
	// Token: 0x060067F7 RID: 26615 RVA: 0x002927A2 File Offset: 0x002909A2
	public CreativeActionEntryFavorite(XUiController _controller, int _stackID)
		: base(_controller, "lblContextActionFavorite", "server_favorite", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.stackID = (ushort)_stackID;
	}

	// Token: 0x060067F8 RID: 26616 RVA: 0x002927C8 File Offset: 0x002909C8
	public override void OnActivated()
	{
		EntityPlayer entityPlayer = base.ItemController.xui.playerUI.entityPlayer;
		if (entityPlayer.favoriteCreativeStacks.Contains(this.stackID))
		{
			entityPlayer.favoriteCreativeStacks.Remove(this.stackID);
		}
		else
		{
			entityPlayer.favoriteCreativeStacks.Add(this.stackID);
		}
		XUiC_Creative2Window childByType = base.ItemController.WindowGroup.Controller.GetChildByType<XUiC_Creative2Window>();
		if (childByType == null)
		{
			return;
		}
		childByType.RefreshView();
	}

	// Token: 0x04005023 RID: 20515
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ushort stackID;
}
