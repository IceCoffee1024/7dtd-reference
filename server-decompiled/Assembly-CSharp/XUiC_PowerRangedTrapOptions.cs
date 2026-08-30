using System;
using UnityEngine.Scripting;

// Token: 0x02000FB7 RID: 4023
[Preserve]
public class XUiC_PowerRangedTrapOptions : XUiController
{
	// Token: 0x17000ED6 RID: 3798
	// (get) Token: 0x06007F1A RID: 32538 RVA: 0x003033E8 File Offset: 0x003015E8
	// (set) Token: 0x06007F1B RID: 32539 RVA: 0x003033F0 File Offset: 0x003015F0
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

	// Token: 0x17000ED7 RID: 3799
	// (get) Token: 0x06007F1C RID: 32540 RVA: 0x003033F9 File Offset: 0x003015F9
	// (set) Token: 0x06007F1D RID: 32541 RVA: 0x00303401 File Offset: 0x00301601
	public XUiC_PowerRangedTrapWindowGroup Owner
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06007F1E RID: 32542 RVA: 0x0030340C File Offset: 0x0030160C
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

	// Token: 0x06007F1F RID: 32543 RVA: 0x003034F8 File Offset: 0x003016F8
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

	// Token: 0x06007F20 RID: 32544 RVA: 0x00303554 File Offset: 0x00301754
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

	// Token: 0x06007F21 RID: 32545 RVA: 0x003035B0 File Offset: 0x003017B0
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

	// Token: 0x06007F22 RID: 32546 RVA: 0x0030360C File Offset: 0x0030180C
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

	// Token: 0x06007F23 RID: 32547 RVA: 0x00303667 File Offset: 0x00301867
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

	// Token: 0x06007F24 RID: 32548 RVA: 0x00303693 File Offset: 0x00301893
	public override void OnOpen()
	{
		base.OnOpen();
		this.tileEntity.SetUserAccessing(true);
		this.SetupTargeting();
		this.tileEntity.SetModified();
	}

	// Token: 0x06007F25 RID: 32549 RVA: 0x003036B8 File Offset: 0x003018B8
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

	// Token: 0x06007F26 RID: 32550 RVA: 0x00303708 File Offset: 0x00301908
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

	// Token: 0x06007F27 RID: 32551 RVA: 0x00303878 File Offset: 0x00301A78
	public Vector3i GetBlockPos()
	{
		if (this.TileEntity != null)
		{
			return this.TileEntity.ToWorldPos();
		}
		return Vector3i.zero;
	}

	// Token: 0x04005E44 RID: 24132
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005E45 RID: 24133
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController pnlTargeting;

	// Token: 0x04005E46 RID: 24134
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetSelf;

	// Token: 0x04005E47 RID: 24135
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetAllies;

	// Token: 0x04005E48 RID: 24136
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetStrangers;

	// Token: 0x04005E49 RID: 24137
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetZombies;

	// Token: 0x04005E4A RID: 24138
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;

	// Token: 0x04005E4C RID: 24140
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
