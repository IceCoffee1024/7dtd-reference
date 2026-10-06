using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FCC RID: 4044
[Preserve]
public class XUiC_PowerTriggerOptions : XUiController
{
	// Token: 0x17000F07 RID: 3847
	// (get) Token: 0x06007F72 RID: 32626 RVA: 0x00307F4C File Offset: 0x0030614C
	// (set) Token: 0x06007F73 RID: 32627 RVA: 0x00307F54 File Offset: 0x00306154
	public TileEntityPoweredTrigger TileEntity
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

	// Token: 0x17000F08 RID: 3848
	// (get) Token: 0x06007F74 RID: 32628 RVA: 0x00307F5D File Offset: 0x0030615D
	// (set) Token: 0x06007F75 RID: 32629 RVA: 0x00307F65 File Offset: 0x00306165
	public XUiC_PowerTriggerWindowGroup Owner
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06007F76 RID: 32630 RVA: 0x00307F70 File Offset: 0x00306170
	public override void Init()
	{
		base.Init();
		this.optionSelector1 = base.GetChildById("optionSelector1") as XUiC_OptionsSelector;
		this.optionSelector2 = base.GetChildById("optionSelector2") as XUiC_OptionsSelector;
		this.pnlTargeting = base.GetChildById("pnlTargeting");
		this.btnOn = base.GetChildById("btnOn");
		this.btnOn_Background = (XUiV_Button)this.btnOn.GetChildById("clickable").ViewComponent;
		this.btnOn_Background.Controller.OnPress += this.btnOn_OnPress;
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
		this.startTimeText = Localization.Get("xuiStartTime", false, null);
		this.endTimeText = Localization.Get("xuiEndTime", false, null);
		string text = Localization.Get("goSecond", false, null);
		string text2 = Localization.Get("goSeconds", false, null);
		string text3 = Localization.Get("goMinute", false, null);
		string text4 = Localization.Get("goMinutes", false, null);
		this.delayStrings.Add(Localization.Get("xuiInstant", false, null));
		this.delayStrings.Add(string.Format(text, 1));
		this.delayStrings.Add(string.Format(text2, 2));
		this.delayStrings.Add(string.Format(text2, 3));
		this.delayStrings.Add(string.Format(text2, 4));
		this.delayStrings.Add(string.Format(text2, 5));
		this.durationStrings.Add(Localization.Get("xuiAlways", false, null));
		this.durationStrings.Add(Localization.Get("xuiTriggered", false, null));
		this.durationStrings.Add(string.Format(text, 1));
		this.durationStrings.Add(string.Format(text2, 2));
		this.durationStrings.Add(string.Format(text2, 3));
		this.durationStrings.Add(string.Format(text2, 4));
		this.durationStrings.Add(string.Format(text2, 5));
		this.durationStrings.Add(string.Format(text2, 6));
		this.durationStrings.Add(string.Format(text2, 7));
		this.durationStrings.Add(string.Format(text2, 8));
		this.durationStrings.Add(string.Format(text2, 9));
		this.durationStrings.Add(string.Format(text2, 10));
		this.durationStrings.Add(string.Format(text2, 15));
		this.durationStrings.Add(string.Format(text2, 30));
		this.durationStrings.Add(string.Format(text2, 45));
		this.durationStrings.Add(string.Format(text3, 1));
		this.durationStrings.Add(string.Format(text4, 5));
		this.durationStrings.Add(string.Format(text4, 10));
		this.durationStrings.Add(string.Format(text4, 30));
		this.durationStrings.Add(string.Format(text4, 60));
		this.btnTargetAllies.ViewComponent.NavDownTarget = (this.btnTargetSelf.ViewComponent.NavDownTarget = (this.btnTargetStrangers.ViewComponent.NavDownTarget = (this.btnTargetZombies.ViewComponent.NavDownTarget = this.optionSelector1.ViewComponent)));
	}

	// Token: 0x06007F77 RID: 32631 RVA: 0x003083E4 File Offset: 0x003065E4
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

	// Token: 0x06007F78 RID: 32632 RVA: 0x00308440 File Offset: 0x00306640
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

	// Token: 0x06007F79 RID: 32633 RVA: 0x0030849C File Offset: 0x0030669C
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

	// Token: 0x06007F7A RID: 32634 RVA: 0x003084F8 File Offset: 0x003066F8
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

	// Token: 0x06007F7B RID: 32635 RVA: 0x00308553 File Offset: 0x00306753
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOn_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x06007F7C RID: 32636 RVA: 0x00308560 File Offset: 0x00306760
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
		if (this.xui.playerUI.playerInput.GUIActions.WindowPagingRight.WasPressed && PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			this.btnOn_OnPress(this, 0);
		}
		base.Update(_dt);
	}

	// Token: 0x06007F7D RID: 32637 RVA: 0x003085D4 File Offset: 0x003067D4
	public override void OnOpen()
	{
		base.OnOpen();
		this.tileEntity.SetUserAccessing(true);
		this.SetupSliders();
		base.RefreshBindings();
		this.tileEntity.SetModified();
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightBumper, "igcoPoweredTriggerReset", XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts, 0f);
	}

	// Token: 0x06007F7E RID: 32638 RVA: 0x00308638 File Offset: 0x00306838
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
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
		base.OnClose();
	}

	// Token: 0x06007F7F RID: 32639 RVA: 0x00308698 File Offset: 0x00306898
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupSliders()
	{
		if (this.pnlTargeting != null)
		{
			this.pnlTargeting.ViewComponent.IsVisible = this.tileEntity.TriggerType == PowerTrigger.TriggerTypes.Motion;
		}
		switch (this.tileEntity.TriggerType)
		{
		case PowerTrigger.TriggerTypes.PressurePlate:
		case PowerTrigger.TriggerTypes.Motion:
		case PowerTrigger.TriggerTypes.TripWire:
		{
			this.optionSelector1.Title = Localization.Get("xuiPowerDelay", false, null);
			this.optionSelector1.ClearItems();
			for (int i = 0; i < this.delayStrings.Count; i++)
			{
				this.optionSelector1.AddItem(this.delayStrings[i]);
			}
			this.optionSelector2.Title = Localization.Get("xuiPowerDuration", false, null);
			this.optionSelector2.ClearItems();
			for (int j = 0; j < this.durationStrings.Count; j++)
			{
				this.optionSelector2.AddItem(this.durationStrings[j]);
			}
			this.optionSelector1.OnSelectionChanged -= this.OptionSelector1_OnSelectionChanged;
			this.optionSelector2.OnSelectionChanged -= this.OptionSelector2_OnSelectionChanged;
			this.optionSelector1.SetIndex((int)this.TileEntity.Property1);
			this.optionSelector2.SetIndex((int)this.TileEntity.Property2);
			this.optionSelector2.OnSelectionChanged += this.OptionSelector2_OnSelectionChanged;
			this.optionSelector1.OnSelectionChanged += this.OptionSelector1_OnSelectionChanged;
			if (this.btnOn != null)
			{
				this.btnOn.ViewComponent.IsVisible = true;
			}
			if (this.pnlTargeting != null)
			{
				this.pnlTargeting.ViewComponent.IsVisible = this.tileEntity.TriggerType == PowerTrigger.TriggerTypes.Motion;
				if (this.TileEntity.TriggerType == PowerTrigger.TriggerTypes.Motion)
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
			break;
		}
		case PowerTrigger.TriggerTypes.TimerRelay:
		{
			this.optionSelector1.Title = this.startTimeText;
			this.optionSelector1.ClearItems();
			for (int k = 0; k < 48; k++)
			{
				int num = k / 2;
				bool flag = k % 2 == 1;
				this.optionSelector1.AddItem(num.ToString("00") + (flag ? ":30" : ":00"));
			}
			this.optionSelector1.OnSelectionChanged -= this.OptionSelector1_OnSelectionChanged;
			this.optionSelector1.OnSelectionChanged += this.OptionSelector1_OnSelectionChanged;
			this.optionSelector2.Title = this.endTimeText;
			this.optionSelector2.ClearItems();
			for (int l = 0; l < 48; l++)
			{
				int num2 = l / 2;
				bool flag2 = l % 2 == 1;
				this.optionSelector2.AddItem(num2.ToString("00") + (flag2 ? ":30" : ":00"));
			}
			this.optionSelector2.OnSelectionChanged -= this.OptionSelector2_OnSelectionChanged;
			this.optionSelector2.OnSelectionChanged += this.OptionSelector2_OnSelectionChanged;
			this.optionSelector1.SetIndex((int)this.TileEntity.Property1);
			this.optionSelector2.SetIndex((int)this.TileEntity.Property2);
			if (this.btnOn != null)
			{
				this.btnOn.ViewComponent.IsVisible = false;
				return;
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06007F80 RID: 32640 RVA: 0x00308B3B File Offset: 0x00306D3B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OptionSelector1_OnSelectionChanged(XUiController _sender, int newSelectedIndex)
	{
		this.TileEntity.Property1 = (byte)newSelectedIndex;
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x06007F81 RID: 32641 RVA: 0x00308B55 File Offset: 0x00306D55
	[PublicizedFrom(EAccessModifier.Private)]
	public void OptionSelector2_OnSelectionChanged(XUiController _sender, int newSelectedIndex)
	{
		this.TileEntity.Property2 = (byte)newSelectedIndex;
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x04005EA8 RID: 24232
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005EA9 RID: 24233
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnOn;

	// Token: 0x04005EAA RID: 24234
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnOn_Background;

	// Token: 0x04005EAB RID: 24235
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblOnOff;

	// Token: 0x04005EAC RID: 24236
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprOnOff;

	// Token: 0x04005EAD RID: 24237
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController pnlTargeting;

	// Token: 0x04005EAE RID: 24238
	[PublicizedFrom(EAccessModifier.Private)]
	public string startTimeText;

	// Token: 0x04005EAF RID: 24239
	[PublicizedFrom(EAccessModifier.Private)]
	public string endTimeText;

	// Token: 0x04005EB0 RID: 24240
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 onColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x04005EB1 RID: 24241
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 offColor = Color.white;

	// Token: 0x04005EB2 RID: 24242
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetSelf;

	// Token: 0x04005EB3 RID: 24243
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetAllies;

	// Token: 0x04005EB4 RID: 24244
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetStrangers;

	// Token: 0x04005EB5 RID: 24245
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetZombies;

	// Token: 0x04005EB6 RID: 24246
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredTrigger tileEntity;

	// Token: 0x04005EB8 RID: 24248
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastOn;

	// Token: 0x04005EB9 RID: 24249
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector1;

	// Token: 0x04005EBA RID: 24250
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector2;

	// Token: 0x04005EBB RID: 24251
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector3;

	// Token: 0x04005EBC RID: 24252
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> delayStrings = new List<string>();

	// Token: 0x04005EBD RID: 24253
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> durationStrings = new List<string>();

	// Token: 0x04005EBE RID: 24254
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
