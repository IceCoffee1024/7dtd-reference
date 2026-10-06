using System;
using UnityEngine.Scripting;

// Token: 0x02000FC6 RID: 4038
[Preserve]
public class XUiC_PowerRangedTrapOptions : XUiController
{
	// Token: 0x17000EFD RID: 3837
	// (get) Token: 0x06007F29 RID: 32553 RVA: 0x003063C0 File Offset: 0x003045C0
	// (set) Token: 0x06007F2A RID: 32554 RVA: 0x003063C8 File Offset: 0x003045C8
	public TileEntityPoweredRangedTrap TileEntity
	{
		get
		{
			return this.tileEntity;
		}
		set
		{
			this.tileEntity = value;
		}
	}

	// Token: 0x17000EFE RID: 3838
	// (get) Token: 0x06007F2B RID: 32555 RVA: 0x003063D1 File Offset: 0x003045D1
	// (set) Token: 0x06007F2C RID: 32556 RVA: 0x003063D9 File Offset: 0x003045D9
	public XUiC_PowerRangedTrapWindowGroup Owner
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06007F2D RID: 32557 RVA: 0x003063E4 File Offset: 0x003045E4
	public override void Init()
	{
		base.Init();
		this.pnlTargeting = base.GetChildById("pnlTargeting");
		this.btnTargetSelf = base.GetChildById("btnTargetSelf");
		this.btnTargetAllies = base.GetChildById("btnTargetAllies");
		this.btnTargetStrangers = base.GetChildById("btnTargetStrangers");
		this.btnTargetZombies = base.GetChildById("btnTargetZombies");
		if (this.btnTargetSelf != null)
		{
			this.btnTargetSelf.OnPress += this.btnTargetSelf_OnPress;
		}
		if (this.btnTargetAllies != null)
		{
			this.btnTargetAllies.OnPress += this.btnTargetAllies_OnPress;
		}
		if (this.btnTargetStrangers != null)
		{
			this.btnTargetStrangers.OnPress += this.btnTargetStrangers_OnPress;
		}
		if (this.btnTargetZombies != null)
		{
			this.btnTargetZombies.OnPress += this.btnTargetZombies_OnPress;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007F2E RID: 32558 RVA: 0x003064D0 File Offset: 0x003046D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTargetSelf_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiV_Button xuiV_Button = this.btnTargetSelf.ViewComponent as XUiV_Button;
		xuiV_Button.Selected = !xuiV_Button.Selected;
		if (xuiV_Button.Selected)
		{
			this.TileEntity.TargetType |= 1;
			return;
		}
		this.TileEntity.TargetType &= -2;
	}

	// Token: 0x06007F2F RID: 32559 RVA: 0x0030652C File Offset: 0x0030472C
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTargetAllies_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiV_Button xuiV_Button = this.btnTargetAllies.ViewComponent as XUiV_Button;
		xuiV_Button.Selected = !xuiV_Button.Selected;
		if (xuiV_Button.Selected)
		{
			this.TileEntity.TargetType |= 2;
			return;
		}
		this.TileEntity.TargetType &= -3;
	}

	// Token: 0x06007F30 RID: 32560 RVA: 0x00306588 File Offset: 0x00304788
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTargetStrangers_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiV_Button xuiV_Button = this.btnTargetStrangers.ViewComponent as XUiV_Button;
		xuiV_Button.Selected = !xuiV_Button.Selected;
		if (xuiV_Button.Selected)
		{
			this.TileEntity.TargetType |= 4;
			return;
		}
		this.TileEntity.TargetType &= -5;
	}

	// Token: 0x06007F31 RID: 32561 RVA: 0x003065E4 File Offset: 0x003047E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTargetZombies_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiV_Button xuiV_Button = this.btnTargetZombies.ViewComponent as XUiV_Button;
		xuiV_Button.Selected = !xuiV_Button.Selected;
		if (xuiV_Button.Selected)
		{
			this.TileEntity.TargetType |= 8;
			return;
		}
		this.TileEntity.TargetType &= -9;
	}

	// Token: 0x06007F32 RID: 32562 RVA: 0x0030663F File Offset: 0x0030483F
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		if (this.tileEntity == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x06007F33 RID: 32563 RVA: 0x0030666B File Offset: 0x0030486B
	public override void OnOpen()
	{
		base.OnOpen();
		this.tileEntity.SetUserAccessing(true);
		this.SetupTargeting();
		this.tileEntity.SetModified();
	}

	// Token: 0x06007F34 RID: 32564 RVA: 0x00306690 File Offset: 0x00304890
	public override void OnClose()
	{
		GameManager instance = GameManager.Instance;
		this.tileEntity.ToWorldPos();
		if (!XUiC_CameraWindow.hackyIsOpeningMaximizedWindow)
		{
			this.tileEntity.SetUserAccessing(false);
			LockManager.Instance.UnlockRequestLocal();
			this.tileEntity.SetModified();
		}
		base.OnClose();
	}

	// Token: 0x06007F35 RID: 32565 RVA: 0x003066E0 File Offset: 0x003048E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupTargeting()
	{
		if (this.pnlTargeting != null)
		{
			if (this.btnTargetSelf != null)
			{
				this.btnTargetSelf.OnPress -= this.btnTargetSelf_OnPress;
				((XUiV_Button)this.btnTargetSelf.ViewComponent).Selected = this.TileEntity.TargetSelf;
				this.btnTargetSelf.OnPress += this.btnTargetSelf_OnPress;
			}
			if (this.btnTargetAllies != null)
			{
				this.btnTargetAllies.OnPress -= this.btnTargetAllies_OnPress;
				((XUiV_Button)this.btnTargetAllies.ViewComponent).Selected = this.TileEntity.TargetAllies;
				this.btnTargetAllies.OnPress += this.btnTargetAllies_OnPress;
			}
			if (this.btnTargetStrangers != null)
			{
				this.btnTargetStrangers.OnPress -= this.btnTargetStrangers_OnPress;
				((XUiV_Button)this.btnTargetStrangers.ViewComponent).Selected = this.TileEntity.TargetStrangers;
				this.btnTargetStrangers.OnPress += this.btnTargetStrangers_OnPress;
			}
			if (this.btnTargetZombies != null)
			{
				this.btnTargetZombies.OnPress -= this.btnTargetZombies_OnPress;
				((XUiV_Button)this.btnTargetZombies.ViewComponent).Selected = this.TileEntity.TargetZombies;
				this.btnTargetZombies.OnPress += this.btnTargetZombies_OnPress;
			}
		}
	}

	// Token: 0x06007F36 RID: 32566 RVA: 0x00306850 File Offset: 0x00304A50
	public Vector3i GetBlockPos()
	{
		if (this.TileEntity != null)
		{
			return this.TileEntity.ToWorldPos();
		}
		return Vector3i.zero;
	}

	// Token: 0x04005E6D RID: 24173
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005E6E RID: 24174
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController pnlTargeting;

	// Token: 0x04005E6F RID: 24175
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetSelf;

	// Token: 0x04005E70 RID: 24176
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetAllies;

	// Token: 0x04005E71 RID: 24177
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetStrangers;

	// Token: 0x04005E72 RID: 24178
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetZombies;

	// Token: 0x04005E73 RID: 24179
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;

	// Token: 0x04005E75 RID: 24181
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
