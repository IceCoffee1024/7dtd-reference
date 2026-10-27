using System;
using UnityEngine.Scripting;

// Token: 0x02000FD6 RID: 4054
[Preserve]
public class XUiC_PowerRangedTrapOptions : XUiController
{
	// Token: 0x17000F08 RID: 3848
	// (get) Token: 0x06007F78 RID: 32632 RVA: 0x00307EDC File Offset: 0x003060DC
	// (set) Token: 0x06007F79 RID: 32633 RVA: 0x00307EE4 File Offset: 0x003060E4
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

	// Token: 0x17000F09 RID: 3849
	// (get) Token: 0x06007F7A RID: 32634 RVA: 0x00307EED File Offset: 0x003060ED
	// (set) Token: 0x06007F7B RID: 32635 RVA: 0x00307EF5 File Offset: 0x003060F5
	public XUiC_PowerRangedTrapWindowGroup Owner
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06007F7C RID: 32636 RVA: 0x00307F00 File Offset: 0x00306100
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

	// Token: 0x06007F7D RID: 32637 RVA: 0x00307FEC File Offset: 0x003061EC
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

	// Token: 0x06007F7E RID: 32638 RVA: 0x00308048 File Offset: 0x00306248
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

	// Token: 0x06007F7F RID: 32639 RVA: 0x003080A4 File Offset: 0x003062A4
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

	// Token: 0x06007F80 RID: 32640 RVA: 0x00308100 File Offset: 0x00306300
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

	// Token: 0x06007F81 RID: 32641 RVA: 0x0030815B File Offset: 0x0030635B
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

	// Token: 0x06007F82 RID: 32642 RVA: 0x00308187 File Offset: 0x00306387
	public override void OnOpen()
	{
		base.OnOpen();
		this.tileEntity.SetUserAccessing(true);
		this.SetupTargeting();
		this.tileEntity.SetModified();
	}

	// Token: 0x06007F83 RID: 32643 RVA: 0x003081AC File Offset: 0x003063AC
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

	// Token: 0x06007F84 RID: 32644 RVA: 0x003081FC File Offset: 0x003063FC
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

	// Token: 0x06007F85 RID: 32645 RVA: 0x0030836C File Offset: 0x0030656C
	public Vector3i GetBlockPos()
	{
		if (this.TileEntity != null)
		{
			return this.TileEntity.ToWorldPos();
		}
		return Vector3i.zero;
	}

	// Token: 0x04005EA8 RID: 24232
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005EA9 RID: 24233
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController pnlTargeting;

	// Token: 0x04005EAA RID: 24234
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetSelf;

	// Token: 0x04005EAB RID: 24235
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetAllies;

	// Token: 0x04005EAC RID: 24236
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetStrangers;

	// Token: 0x04005EAD RID: 24237
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetZombies;

	// Token: 0x04005EAE RID: 24238
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;

	// Token: 0x04005EB0 RID: 24240
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
