using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010A9 RID: 4265
[Preserve]
public class XUiC_ToolbeltWindow : XUiController
{
	// Token: 0x06008723 RID: 34595 RVA: 0x00339AC0 File Offset: 0x00337CC0
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < ToolbeltConfig.CurrentConfig.BarsPerPage; i++)
		{
			XUiController childById = base.GetChildById("btnClearInventory" + i.ToString());
			if (childById != null)
			{
				int barIdx = i;
				childById.OnPress += delegate(XUiController _sender, int _mouseButton)
				{
					this.BtnClearInventory_OnPress(barIdx);
				};
			}
		}
	}

	// Token: 0x06008724 RID: 34596 RVA: 0x00339B2C File Offset: 0x00337D2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory_OnPress(int idx)
	{
		XUiC_Toolbelt childByType = this.xui.playerUI.entityPlayer.PlayerUI.xui.GetChildByType<XUiC_Toolbelt>();
		if (childByType == null)
		{
			return;
		}
		int num = childByType.CurrentPage * ToolbeltConfig.CurrentConfig.SlotsPerPage + idx * ToolbeltConfig.CurrentConfig.SlotsPerBar;
		int num2 = num + ToolbeltConfig.CurrentConfig.SlotsPerBar;
		this.xui.playerUI.entityPlayer.EmptyToolbelt(num, num2);
	}

	// Token: 0x06008725 RID: 34597 RVA: 0x00339BA0 File Offset: 0x00337DA0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.deltaTime = _dt;
		if ((DateTime.Now - this.updateTime).TotalSeconds > 0.5)
		{
			this.updateTime = DateTime.Now;
		}
		base.RefreshBindings();
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		base.ViewComponent.IsVisible = (!(this.localPlayer.AttachedToEntity != null) || !(this.localPlayer.AttachedToEntity is EntityVehicle)) && !this.localPlayer.IsDead() && (windowManager.IsHUDEnabled() || (this.xui.DragAndDropWindow.InMenu && windowManager.IsHUDPartialHidden()));
		if (this.CustomAttributes.ContainsKey("standard_xp_color"))
		{
			this.standardXPColor = this.CustomAttributes["standard_xp_color"].ToString();
		}
		else
		{
			this.standardXPColor = "128,4,128";
		}
		if (this.CustomAttributes.ContainsKey("updating_xp_color"))
		{
			this.updatingXPColor = this.CustomAttributes["updating_xp_color"].ToString();
		}
		else
		{
			this.updatingXPColor = "128,4,128";
		}
		if (this.CustomAttributes.ContainsKey("deficit_xp_color"))
		{
			this.expDeficitColor = this.CustomAttributes["deficit_xp_color"].ToString();
		}
		else
		{
			this.expDeficitColor = "222,20,20";
		}
		if (this.CustomAttributes.ContainsKey("xp_fill_speed"))
		{
			this.xpFillSpeed = StringParsers.ParseFloat(this.CustomAttributes["xp_fill_speed"].ToString(), 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x06008726 RID: 34598 RVA: 0x00339D50 File Offset: 0x00337F50
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.localPlayer == null)
		{
			this.localPlayer = this.xui.playerUI.entityPlayer;
		}
		this.currentValue = (this.lastValue = XUiM_Player.GetLevelPercent(this.localPlayer));
	}

	// Token: 0x06008727 RID: 34599 RVA: 0x00339DA4 File Offset: 0x00337FA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "xp")
		{
			value = "0";
			if (this.localPlayer != null)
			{
				if (this.localPlayer.Progression.ExpDeficit > 0)
				{
					float num = Math.Max(this.lastDeficitValue, 0f) * 1.01f;
					value = this.bindingXp.Format(num);
					this.currentValue = (float)this.localPlayer.Progression.ExpDeficit / (float)this.localPlayer.Progression.GetExpForNextLevel();
					if (this.currentValue != this.lastDeficitValue)
					{
						this.lastDeficitValue = Mathf.Lerp(this.lastDeficitValue, this.currentValue, Time.deltaTime * this.xpFillSpeed);
						if (Mathf.Abs(this.currentValue - this.lastDeficitValue) < 0.005f)
						{
							this.lastDeficitValue = this.currentValue;
						}
					}
				}
				else
				{
					float num2 = Math.Max(this.lastValue, 0f) * 1.01f;
					value = this.bindingXp.Format(num2);
					this.currentValue = XUiM_Player.GetLevelPercent(this.localPlayer);
					if (this.currentValue != this.lastValue)
					{
						this.lastValue = Mathf.Lerp(this.lastValue, this.currentValue, Time.deltaTime * this.xpFillSpeed);
						if (Mathf.Abs(this.currentValue - this.lastValue) < 0.005f)
						{
							this.lastValue = this.currentValue;
						}
					}
				}
			}
			return true;
		}
		if (bindingName == "xpcolor")
		{
			if (this.localPlayer != null)
			{
				if (this.localPlayer.Progression.ExpDeficit > 0)
				{
					value = this.expDeficitColor;
				}
				else
				{
					value = ((this.currentValue == this.lastValue) ? this.standardXPColor : this.updatingXPColor);
				}
			}
			else
			{
				value = this.standardXPColor;
			}
			return true;
		}
		if (!(bindingName == "showxp"))
		{
			if (!(bindingName == "creativewindowopen"))
			{
				return base.GetBindingValueInternal(ref value, bindingName);
			}
			value = this.xui.playerUI.windowManager.IsWindowOpen("creative").ToString();
			return true;
		}
		else
		{
			if (this.localPlayer != null)
			{
				value = ((Progression.ShowXPType == Progression.ShowXPTypes.All || Progression.ShowXPType == Progression.ShowXPTypes.BarOnly) && Progression.XPGain > 0f).ToString();
				return true;
			}
			value = "false";
			return true;
		}
	}

	// Token: 0x04006468 RID: 25704
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer localPlayer;

	// Token: 0x04006469 RID: 25705
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime updateTime;

	// Token: 0x0400646A RID: 25706
	[PublicizedFrom(EAccessModifier.Private)]
	public float lmpPositionAdjustment = 0.05f;

	// Token: 0x0400646B RID: 25707
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastValue;

	// Token: 0x0400646C RID: 25708
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentValue;

	// Token: 0x0400646D RID: 25709
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastDeficitValue;

	// Token: 0x0400646E RID: 25710
	[PublicizedFrom(EAccessModifier.Private)]
	public float deltaTime;

	// Token: 0x0400646F RID: 25711
	[PublicizedFrom(EAccessModifier.Private)]
	public string standardXPColor = "255,255,255,255";

	// Token: 0x04006470 RID: 25712
	[PublicizedFrom(EAccessModifier.Private)]
	public string updatingXPColor = "255,255,255,255";

	// Token: 0x04006471 RID: 25713
	[PublicizedFrom(EAccessModifier.Private)]
	public string expDeficitColor = "255,255,255,255";

	// Token: 0x04006472 RID: 25714
	[PublicizedFrom(EAccessModifier.Private)]
	public float xpFillSpeed = 2.5f;

	// Token: 0x04006473 RID: 25715
	[PublicizedFrom(EAccessModifier.Private)]
	public CachedStringFormatterFloat bindingXp = new CachedStringFormatterFloat(null);
}
