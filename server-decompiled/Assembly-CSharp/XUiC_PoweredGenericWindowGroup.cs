using System;
using UnityEngine.Scripting;

// Token: 0x02000F9B RID: 3995
[Preserve]
public class XUiC_PoweredGenericWindowGroup : XUiController
{
	// Token: 0x17000E99 RID: 3737
	// (get) Token: 0x06007E08 RID: 32264 RVA: 0x002FE363 File Offset: 0x002FC563
	// (set) Token: 0x06007E09 RID: 32265 RVA: 0x002FE36B File Offset: 0x002FC56B
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

	// Token: 0x06007E0A RID: 32266 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setupWindowTileEntities()
	{
	}

	// Token: 0x06007E0B RID: 32267 RVA: 0x002FE37C File Offset: 0x002FC57C
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

	// Token: 0x06007E0C RID: 32268 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007E0D RID: 32269 RVA: 0x002FE45A File Offset: 0x002FC65A
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

	// Token: 0x04005D6C RID: 23916
	[PublicizedFrom(EAccessModifier.Protected)]
	public TileEntityPowered tileEntity;

	// Token: 0x04005D6D RID: 23917
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005D6E RID: 23918
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
