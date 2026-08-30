using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001070 RID: 4208
[Preserve]
public class XUiC_ToolbeltWindow : XUiController
{
	// Token: 0x06008590 RID: 34192 RVA: 0x0032F464 File Offset: 0x0032D664
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildById("btnClearInventory1");
		if (xuiController != null)
		{
			xuiController.OnPress += this.BtnClearInventory1_OnPress;
		}
		xuiController = base.GetChildById("btnClearInventory2");
		if (xuiController != null)
		{
			xuiController.OnPress += this.BtnClearInventory2_OnPress;
		}
	}

	// Token: 0x06008591 RID: 34193 RVA: 0x0032F4B9 File Offset: 0x0032D6B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory1_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.entityPlayer.EmptyToolbelt(0, 10);
	}

	// Token: 0x06008592 RID: 34194 RVA: 0x0032F4D3 File Offset: 0x0032D6D3
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory2_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.entityPlayer.EmptyToolbelt(10, 20);
	}

	// Token: 0x06008593 RID: 34195 RVA: 0x0032F4F0 File Offset: 0x0032D6F0
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

	// Token: 0x06008594 RID: 34196 RVA: 0x0032F6A0 File Offset: 0x0032D8A0
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.localPlayer == null)
		{
			this.localPlayer = this.xui.playerUI.entityPlayer;
		}
		this.currentValue = (this.lastValue = XUiM_Player.GetLevelPercent(this.localPlayer));
	}

	// Token: 0x06008595 RID: 34197 RVA: 0x0032F6F4 File Offset: 0x0032D8F4
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

	// Token: 0x04006322 RID: 25378
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer localPlayer;

	// Token: 0x04006323 RID: 25379
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime updateTime;

	// Token: 0x04006324 RID: 25380
	[PublicizedFrom(EAccessModifier.Private)]
	public float lmpPositionAdjustment = 0.05f;

	// Token: 0x04006325 RID: 25381
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastValue;

	// Token: 0x04006326 RID: 25382
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentValue;

	// Token: 0x04006327 RID: 25383
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastDeficitValue;

	// Token: 0x04006328 RID: 25384
	[PublicizedFrom(EAccessModifier.Private)]
	public float deltaTime;

	// Token: 0x04006329 RID: 25385
	[PublicizedFrom(EAccessModifier.Private)]
	public string standardXPColor = "255,255,255,255";

	// Token: 0x0400632A RID: 25386
	[PublicizedFrom(EAccessModifier.Private)]
	public string updatingXPColor = "255,255,255,255";

	// Token: 0x0400632B RID: 25387
	[PublicizedFrom(EAccessModifier.Private)]
	public string expDeficitColor = "255,255,255,255";

	// Token: 0x0400632C RID: 25388
	[PublicizedFrom(EAccessModifier.Private)]
	public float xpFillSpeed = 2.5f;

	// Token: 0x0400632D RID: 25389
	[PublicizedFrom(EAccessModifier.Private)]
	public CachedStringFormatterFloat bindingXp = new CachedStringFormatterFloat(null);
}
