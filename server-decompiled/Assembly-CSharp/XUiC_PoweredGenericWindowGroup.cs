using System;
using UnityEngine.Scripting;

// Token: 0x02000FC3 RID: 4035
[Preserve]
public class XUiC_PoweredGenericWindowGroup : XUiController
{
	// Token: 0x17000EF9 RID: 3833
	// (get) Token: 0x06007F09 RID: 32521 RVA: 0x00305B0F File Offset: 0x00303D0F
	// (set) Token: 0x06007F0A RID: 32522 RVA: 0x00305B17 File Offset: 0x00303D17
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

	// Token: 0x06007F0B RID: 32523 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setupWindowTileEntities()
	{
	}

	// Token: 0x06007F0C RID: 32524 RVA: 0x00305B28 File Offset: 0x00303D28
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

	// Token: 0x06007F0D RID: 32525 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007F0E RID: 32526 RVA: 0x00305C06 File Offset: 0x00303E06
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

	// Token: 0x04005E5C RID: 24156
	[PublicizedFrom(EAccessModifier.Protected)]
	public TileEntityPowered tileEntity;

	// Token: 0x04005E5D RID: 24157
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005E5E RID: 24158
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
