using System;
using UnityEngine.Scripting;

// Token: 0x02000FD3 RID: 4051
[Preserve]
public class XUiC_PoweredGenericWindowGroup : XUiController
{
	// Token: 0x17000F04 RID: 3844
	// (get) Token: 0x06007F58 RID: 32600 RVA: 0x0030762B File Offset: 0x0030582B
	// (set) Token: 0x06007F59 RID: 32601 RVA: 0x00307633 File Offset: 0x00305833
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

	// Token: 0x06007F5A RID: 32602 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setupWindowTileEntities()
	{
	}

	// Token: 0x06007F5B RID: 32603 RVA: 0x00307644 File Offset: 0x00305844
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

	// Token: 0x06007F5C RID: 32604 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007F5D RID: 32605 RVA: 0x00307722 File Offset: 0x00305922
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

	// Token: 0x04005E97 RID: 24215
	[PublicizedFrom(EAccessModifier.Protected)]
	public TileEntityPowered tileEntity;

	// Token: 0x04005E98 RID: 24216
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E99 RID: 24217
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
