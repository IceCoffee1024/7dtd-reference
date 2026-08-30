using System;
using UnityEngine.Scripting;

// Token: 0x02000FB4 RID: 4020
[Preserve]
public class XUiC_PoweredGenericWindowGroup : XUiController
{
	// Token: 0x17000ED2 RID: 3794
	// (get) Token: 0x06007EFA RID: 32506 RVA: 0x00302B37 File Offset: 0x00300D37
	// (set) Token: 0x06007EFB RID: 32507 RVA: 0x00302B3F File Offset: 0x00300D3F
	public TileEntityPowered TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
			this.setupWindowTileEntities();
		}
	}

	// Token: 0x06007EFC RID: 32508 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setupWindowTileEntities()
	{
	}

	// Token: 0x06007EFD RID: 32509 RVA: 0x00302B50 File Offset: 0x00300D50
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

	// Token: 0x06007EFE RID: 32510 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007EFF RID: 32511 RVA: 0x00302C2E File Offset: 0x00300E2E
	public override void OnClose()
	{
		base.OnClose();
		this.wasReleased = false;
		this.activeKeyDown = false;
		if (this.tileEntity != null && !XUiC_CameraWindow.hackyIsOpeningMaximizedWindow)
		{
			LockManager.Instance.UnlockRequestLocal();
			this.tileEntity = null;
		}
	}

	// Token: 0x04005E33 RID: 24115
	[PublicizedFrom(EAccessModifier.Protected)]
	public TileEntityPowered tileEntity;

	// Token: 0x04005E34 RID: 24116
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E35 RID: 24117
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
