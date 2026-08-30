using System;
using System.Collections.Generic;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FBD RID: 4029
[Preserve]
public class XUiC_PowerTriggerOptions : XUiController
{
	// Token: 0x17000EE0 RID: 3808
	// (get) Token: 0x06007F63 RID: 32611 RVA: 0x00304F74 File Offset: 0x00303174
	// (set) Token: 0x06007F64 RID: 32612 RVA: 0x00304F7C File Offset: 0x0030317C
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

	// Token: 0x17000EE1 RID: 3809
	// (get) Token: 0x06007F65 RID: 32613 RVA: 0x00304F85 File Offset: 0x00303185
	// (set) Token: 0x06007F66 RID: 32614 RVA: 0x00304F8D File Offset: 0x0030318D
	public XUiC_PowerTriggerWindowGroup Owner
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06007F67 RID: 32615 RVA: 0x00304F98 File Offset: 0x00303198
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

	// Token: 0x06007F68 RID: 32616 RVA: 0x0030540C File Offset: 0x0030360C
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

	// Token: 0x06007F69 RID: 32617 RVA: 0x00305468 File Offset: 0x00303668
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

	// Token: 0x06007F6A RID: 32618 RVA: 0x003054C4 File Offset: 0x003036C4
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

	// Token: 0x06007F6B RID: 32619 RVA: 0x00305520 File Offset: 0x00303720
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

	// Token: 0x06007F6C RID: 32620 RVA: 0x0030557B File Offset: 0x0030377B
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOn_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x06007F6D RID: 32621 RVA: 0x00305588 File Offset: 0x00303788
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

	// Token: 0x06007F6E RID: 32622 RVA: 0x003055FC File Offset: 0x003037FC
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

	// Token: 0x06007F6F RID: 32623 RVA: 0x00305660 File Offset: 0x00303860
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

	// Token: 0x06007F70 RID: 32624 RVA: 0x003056C0 File Offset: 0x003038C0
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

	// Token: 0x06007F71 RID: 32625 RVA: 0x00305B63 File Offset: 0x00303D63
	[PublicizedFrom(EAccessModifier.Private)]
	public void OptionSelector1_OnSelectionChanged(XUiController _sender, int newSelectedIndex)
	{
		this.TileEntity.Property1 = (byte)newSelectedIndex;
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x06007F72 RID: 32626 RVA: 0x00305B7D File Offset: 0x00303D7D
	[PublicizedFrom(EAccessModifier.Private)]
	public void OptionSelector2_OnSelectionChanged(XUiController _sender, int newSelectedIndex)
	{
		this.TileEntity.Property2 = (byte)newSelectedIndex;
		this.TileEntity.ResetTrigger();
	}

	// Token: 0x04005E7F RID: 24191
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005E80 RID: 24192
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnOn;

	// Token: 0x04005E81 RID: 24193
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnOn_Background;

	// Token: 0x04005E82 RID: 24194
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblOnOff;

	// Token: 0x04005E83 RID: 24195
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprOnOff;

	// Token: 0x04005E84 RID: 24196
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController pnlTargeting;

	// Token: 0x04005E85 RID: 24197
	[PublicizedFrom(EAccessModifier.Private)]
	public string startTimeText;

	// Token: 0x04005E86 RID: 24198
	[PublicizedFrom(EAccessModifier.Private)]
	public string endTimeText;

	// Token: 0x04005E87 RID: 24199
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 onColor = new Color32(250, byte.MaxValue, 163, byte.MaxValue);

	// Token: 0x04005E88 RID: 24200
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 offColor = Color.white;

	// Token: 0x04005E89 RID: 24201
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetSelf;

	// Token: 0x04005E8A RID: 24202
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetAllies;

	// Token: 0x04005E8B RID: 24203
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetStrangers;

	// Token: 0x04005E8C RID: 24204
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnTargetZombies;

	// Token: 0x04005E8D RID: 24205
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPoweredTrigger tileEntity;

	// Token: 0x04005E8F RID: 24207
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastOn;

	// Token: 0x04005E90 RID: 24208
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector1;

	// Token: 0x04005E91 RID: 24209
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector2;

	// Token: 0x04005E92 RID: 24210
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionsSelector optionSelector3;

	// Token: 0x04005E93 RID: 24211
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> delayStrings = new List<string>();

	// Token: 0x04005E94 RID: 24212
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> durationStrings = new List<string>();

	// Token: 0x04005E95 RID: 24213
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
