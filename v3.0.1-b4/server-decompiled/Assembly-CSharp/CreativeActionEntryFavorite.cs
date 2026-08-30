using System;
using UnityEngine.Scripting;

// Token: 0x02000D40 RID: 3392
[Preserve]
public class CreativeActionEntryFavorite : BaseItemActionEntry
{
	// Token: 0x06006764 RID: 26468 RVA: 0x0028C356 File Offset: 0x0028A556
	public CreativeActionEntryFavorite(XUiController _controller, int _stackID)
		: base(_controller, "lblContextActionFavorite", "server_favorite", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.stackID = (ushort)_stackID;
	}

	// Token: 0x06006765 RID: 26469 RVA: 0x0028C37C File Offset: 0x0028A57C
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

	// Token: 0x04004F46 RID: 20294
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ushort stackID;
}
