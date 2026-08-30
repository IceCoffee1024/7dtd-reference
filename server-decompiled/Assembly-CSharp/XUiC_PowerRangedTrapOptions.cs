using System;
using UnityEngine.Scripting;

// Token: 0x02000F9E RID: 3998
[Preserve]
public class XUiC_PowerRangedTrapOptions : XUiController
{
	// Token: 0x17000E9D RID: 3741
	// (get) Token: 0x06007E28 RID: 32296 RVA: 0x002FEC14 File Offset: 0x002FCE14
	// (set) Token: 0x06007E29 RID: 32297 RVA: 0x002FEC1C File Offset: 0x002FCE1C
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

	// Token: 0x17000E9E RID: 3742
	// (get) Token: 0x06007E2A RID: 32298 RVA: 0x002FEC25 File Offset: 0x002FCE25
	// (set) Token: 0x06007E2B RID: 32299 RVA: 0x002FEC2D File Offset: 0x002FCE2D
	public XUiC_PowerRangedTrapWindowGroup Owner
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06007E2C RID: 32300 RVA: 0x002FEC38 File Offset: 0x002FCE38
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

	// Token: 0x06007E2D RID: 32301 RVA: 0x002FED24 File Offset: 0x002FCF24
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

	// Token: 0x06007E2E RID: 32302 RVA: 0x002FED80 File Offset: 0x002FCF80
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

	// Token: 0x06007E2F RID: 32303 RVA: 0x002FEDDC File Offset: 0x002FCFDC
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

	// Token: 0x06007E30 RID: 32304 RVA: 0x002FEE38 File Offset: 0x002FD038
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

	// Token: 0x06007E31 RID: 32305 RVA: 0x002FEE93 File Offset: 0x002FD093
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

	// Token: 0x06007E32 RID: 32306 RVA: 0x002FEEBF File Offset: 0x002FD0BF
	public override void OnOpen()
	{
		base.OnOpen();
		this.tileEntity.SetUserAccessing(true);
		this.SetupTargeting();
		this.tileEntity.SetModified();
	}

	// Token: 0x06007E33 RID: 32307 RVA: 0x002FEEE4 File Offset: 0x002FD0E4
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

	// Token: 0x06007E34 RID: 32308 RVA: 0x002FEF34 File Offset: 0x002FD134
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

	// Token: 0x06007E35 RID: 32309 RVA: 0x002FF0A4 File Offset: 0x002FD2A4
	public Vector3i GetBlockPos()
	{
		if (this.TileEntity != null)
		{
			return this.TileEntity.ToWorldPos();
		}
		return Vector3i.zero;
	}

	// Token: 0x04005D7D RID: 23933
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005D7E RID: 23934
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController pnlTargeting;

	// Token: 0x04005D7F RID: 23935
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetSelf;

	// Token: 0x04005D80 RID: 23936
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetAllies;

	// Token: 0x04005D81 RID: 23937
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetStrangers;

	// Token: 0x04005D82 RID: 23938
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetZombies;

	// Token: 0x04005D83 RID: 23939
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredRangedTrap tileEntity;

	// Token: 0x04005D85 RID: 23941
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
