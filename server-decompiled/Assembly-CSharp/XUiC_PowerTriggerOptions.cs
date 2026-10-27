using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FDC RID: 4060
[Preserve]
public class XUiC_PowerTriggerOptions : XUiController
{
	// Token: 0x17000F12 RID: 3858
	// (get) Token: 0x06007FC1 RID: 32705 RVA: 0x00309A68 File Offset: 0x00307C68
	// (set) Token: 0x06007FC2 RID: 32706 RVA: 0x00309A70 File Offset: 0x00307C70
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

	// Token: 0x17000F13 RID: 3859
	// (get) Token: 0x06007FC3 RID: 32707 RVA: 0x00309A79 File Offset: 0x00307C79
	// (set) Token: 0x06007FC4 RID: 32708 RVA: 0x00309A81 File Offset: 0x00307C81
	public XUiC_PowerTriggerWindowGroup Owner
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06007FC5 RID: 32709 RVA: 0x00309A8C File Offset: 0x00307C8C
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

	// Token: 0x06007FC6 RID: 32710 RVA: 0x00309F00 File Offset: 0x00308100
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

	// Token: 0x06007FC7 RID: 32711 RVA: 0x00309F5C File Offset: 0x0030815C
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

	// Token: 0x06007FC8 RID: 32712 RVA: 0x00309FB8 File Offset: 0x003081B8
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

	// Token: 0x06007FC9 RID: 32713 RVA: 0x0030A014 File Offset: 0x00308214
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

	// Token: 0x06007FCA RID: 32714 RVA: 0x0030A06F File Offset: 0x0030826F
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOn_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x06007FCB RID: 32715 RVA: 0x0030A07C File Offset: 0x0030827C
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

	// Token: 0x06007FCC RID: 32716 RVA: 0x0030A0F0 File Offset: 0x003082F0
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

	// Token: 0x06007FCD RID: 32717 RVA: 0x0030A154 File Offset: 0x00308354
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

	// Token: 0x06007FCE RID: 32718 RVA: 0x0030A1B4 File Offset: 0x003083B4
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

	// Token: 0x06007FCF RID: 32719 RVA: 0x0030A657 File Offset: 0x00308857
	[PublicizedFrom(EAccessModifier.Private)]
	public void OptionSelector1_OnSelectionChanged(XUiController _sender, int newSelectedIndex)
	{
		this.TileEntity.Property1 = (byte)newSelectedIndex;
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x06007FD0 RID: 32720 RVA: 0x0030A671 File Offset: 0x00308871
	[PublicizedFrom(EAccessModifier.Private)]
	public void OptionSelector2_OnSelectionChanged(XUiController _sender, int newSelectedIndex)
	{
		this.TileEntity.Property2 = (byte)newSelectedIndex;
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x04005EE3 RID: 24291
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005EE4 RID: 24292
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnOn;

	// Token: 0x04005EE5 RID: 24293
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnOn_Background;

	// Token: 0x04005EE6 RID: 24294
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblOnOff;

	// Token: 0x04005EE7 RID: 24295
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprOnOff;

	// Token: 0x04005EE8 RID: 24296
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController pnlTargeting;

	// Token: 0x04005EE9 RID: 24297
	[PublicizedFrom(EAccessModifier.Private)]
	public string startTimeText;

	// Token: 0x04005EEA RID: 24298
	[PublicizedFrom(EAccessModifier.Private)]
	public string endTimeText;

	// Token: 0x04005EEB RID: 24299
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 onColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x04005EEC RID: 24300
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 offColor = Color.white;

	// Token: 0x04005EED RID: 24301
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetSelf;

	// Token: 0x04005EEE RID: 24302
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetAllies;

	// Token: 0x04005EEF RID: 24303
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetStrangers;

	// Token: 0x04005EF0 RID: 24304
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetZombies;

	// Token: 0x04005EF1 RID: 24305
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredTrigger tileEntity;

	// Token: 0x04005EF3 RID: 24307
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastOn;

	// Token: 0x04005EF4 RID: 24308
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector1;

	// Token: 0x04005EF5 RID: 24309
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector2;

	// Token: 0x04005EF6 RID: 24310
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector3;

	// Token: 0x04005EF7 RID: 24311
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> delayStrings = new List<string>();

	// Token: 0x04005EF8 RID: 24312
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> durationStrings = new List<string>();

	// Token: 0x04005EF9 RID: 24313
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
