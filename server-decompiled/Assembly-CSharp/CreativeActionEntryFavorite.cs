using System;
using UnityEngine.Scripting;

// Token: 0x02000D6E RID: 3438
[Preserve]
public class CreativeActionEntryFavorite : BaseItemActionEntry
{
	// Token: 0x0600681C RID: 26652 RVA: 0x00293CCA File Offset: 0x00291ECA
	public CreativeActionEntryFavorite(XUiController _controller, int _stackID)
		: base(_controller, "lblContextActionFavorite", "server_favorite", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
		this.stackID = (ushort)_stackID;
	}

	// Token: 0x0600681D RID: 26653 RVA: 0x00293CF0 File Offset: 0x00291EF0
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

	// Token: 0x04005048 RID: 20552
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ushort stackID;
}
