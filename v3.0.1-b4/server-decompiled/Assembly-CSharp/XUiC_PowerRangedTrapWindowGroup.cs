using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F9F RID: 3999
[Preserve]
public class XUiC_PowerRangedTrapWindowGroup : XUiController
{
	// Token: 0x06007E37 RID: 32311 RVA: 0x002FF0C0 File Offset: 0x002FD2C0
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		if (xuiController != null)
		{
			this.nonPagingHeader = (XUiC_WindowNonPagingHeader)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_PowerRangedAmmoSlots>();
		if (xuiController != null)
		{
			this.ammoWindow = (XUiC_PowerRangedAmmoSlots)xuiController;
			this.ammoWindow.Owner = this;
		}
		xuiController = base.GetChildByType<XUiC_PowerRangedTrapOptions>();
		if (xuiController != null)
		{
			this.optionsWindow = (XUiC_PowerRangedTrapOptions)xuiController;
			this.optionsWindow.Owner = this;
		}
		xuiController = base.GetChildById("windowPowerCameraControlPreview");
		if (xuiController != null)
		{
			this.cameraWindowPreview = (XUiC_CameraWindow)xuiController;
			this.cameraWindowPreview.Owner = this;
		}
	}

	// Token: 0x17000E9F RID: 3743
	// (get) Token: 0x06007E38 RID: 32312 RVA: 0x002FF154 File Offset: 0x002FD354
	// (set) Token: 0x06007E39 RID: 32313 RVA: 0x002FF15C File Offset: 0x002FD35C
	public TileEntityPoweredRangedTrap TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.ammoWindow.TileEntity = this.tileEntity;
			if (this.tileEntity.PowerItemType == PowerItem.PowerItemTypes.RangedTrap)
			{
				this.optionsWindow.TileEntity = this.tileEntity;
			}
			this.cameraWindowPreview.TileEntity = this.tileEntity;
		}
	}

	// Token: 0x06007E3A RID: 32314 RVA: 0x002FF1B4 File Offset: 0x002FD3B4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.windowGroup.isShowing)
		{
			if (!this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
			{
				this.wasReleased = true;
			}
			if (this.wasReleased)
			{
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
				{
					this.activeKeyDown = true;
				}
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.WasReleased && this.activeKeyDown)
				{
					this.activeKeyDown = false;
					if (!this.xui.playerUI.windowManager.IsInputActive())
					{
						this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
					}
				}
			}
		}
	}

	// Token: 0x06007E3B RID: 32315 RVA: 0x002FF294 File Offset: 0x002FD494
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		if (this.nonPagingHeader != null)
		{
			World world = GameManager.Instance.World;
			string localizedBlockName = this.tileEntity.GetChunk().GetBlock(this.tileEntity.localChunkPos).Block.GetLocalizedBlockName();
			this.nonPagingHeader.SetHeader(localizedBlockName);
		}
		this.xui.RecenterWindowGroup(this.windowGroup, false);
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnOpen();
		}
		if (!this.tileEntity.ShowTargeting)
		{
			this.optionsWindow.ViewComponent.IsVisible = false;
		}
		Manager.BroadcastPlayByLocalPlayer(this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, "open_vending");
		this.IsDirty = true;
		this.TileEntity.Destroyed += this.TileEntity_Destroyed;
	}

	// Token: 0x06007E3C RID: 32316 RVA: 0x002FF3C4 File Offset: 0x002FD5C4
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		Vector3 vector = this.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f;
		if (!XUiC_CameraWindow.hackyIsOpeningMaximizedWindow)
		{
			Manager.BroadcastPlayByLocalPlayer(vector, "close_vending");
		}
		this.TileEntity.Destroyed -= this.TileEntity_Destroyed;
	}

	// Token: 0x06007E3D RID: 32317 RVA: 0x002FF438 File Offset: 0x002FD638
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_Destroyed(ITileEntity te)
	{
		if (this.TileEntity == te)
		{
			if (GameManager.Instance != null)
			{
				this.xui.playerUI.windowManager.Close("powerrangedtrap");
				this.xui.playerUI.windowManager.Close("powercamera");
				return;
			}
		}
		else
		{
			te.Destroyed -= this.TileEntity_Destroyed;
		}
	}

	// Token: 0x04005D86 RID: 23942
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeader;

	// Token: 0x04005D87 RID: 23943
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerRangedTrapOptions optionsWindow;

	// Token: 0x04005D88 RID: 23944
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PowerRangedAmmoSlots ammoWindow;

	// Token: 0x04005D89 RID: 23945
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CameraWindow cameraWindowPreview;

	// Token: 0x04005D8A RID: 23946
	public static string ID = "powerrangedtrap";

	// Token: 0x04005D8B RID: 23947
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005D8C RID: 23948
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04005D8D RID: 23949
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;
}
