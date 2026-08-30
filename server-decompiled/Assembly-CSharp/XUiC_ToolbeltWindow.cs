using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001089 RID: 4233
[Preserve]
public class XUiC_ToolbeltWindow : XUiController
{
	// Token: 0x06008682 RID: 34434 RVA: 0x00333C8C File Offset: 0x00331E8C
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

	// Token: 0x06008683 RID: 34435 RVA: 0x00333CE1 File Offset: 0x00331EE1
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory1_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.entityPlayer.EmptyToolbelt(0, 10);
	}

	// Token: 0x06008684 RID: 34436 RVA: 0x00333CFB File Offset: 0x00331EFB
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory2_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.entityPlayer.EmptyToolbelt(10, 20);
	}

	// Token: 0x06008685 RID: 34437 RVA: 0x00333D18 File Offset: 0x00331F18
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

	// Token: 0x06008686 RID: 34438 RVA: 0x00333EC8 File Offset: 0x003320C8
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.localPlayer == null)
		{
			this.localPlayer = this.xui.playerUI.entityPlayer;
		}
		this.currentValue = (this.lastValue = XUiM_Player.GetLevelPercent(this.localPlayer));
	}

	// Token: 0x06008687 RID: 34439 RVA: 0x00333F1C File Offset: 0x0033211C
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

	// Token: 0x040063E9 RID: 25577
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer localPlayer;

	// Token: 0x040063EA RID: 25578
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime updateTime;

	// Token: 0x040063EB RID: 25579
	[PublicizedFrom(EAccessModifier.Private)]
	public float lmpPositionAdjustment = 0.05f;

	// Token: 0x040063EC RID: 25580
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastValue;

	// Token: 0x040063ED RID: 25581
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentValue;

	// Token: 0x040063EE RID: 25582
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastDeficitValue;

	// Token: 0x040063EF RID: 25583
	[PublicizedFrom(EAccessModifier.Private)]
	public float deltaTime;

	// Token: 0x040063F0 RID: 25584
	[PublicizedFrom(EAccessModifier.Private)]
	public string standardXPColor = "255,255,255,255";

	// Token: 0x040063F1 RID: 25585
	[PublicizedFrom(EAccessModifier.Private)]
	public string updatingXPColor = "255,255,255,255";

	// Token: 0x040063F2 RID: 25586
	[PublicizedFrom(EAccessModifier.Private)]
	public string expDeficitColor = "255,255,255,255";

	// Token: 0x040063F3 RID: 25587
	[PublicizedFrom(EAccessModifier.Private)]
	public float xpFillSpeed = 2.5f;

	// Token: 0x040063F4 RID: 25588
	[PublicizedFrom(EAccessModifier.Private)]
	public CachedStringFormatterFloat bindingXp = new CachedStringFormatterFloat(null);
}
