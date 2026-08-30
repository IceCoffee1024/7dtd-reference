using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EB6 RID: 3766
[Preserve]
public class XUiC_BuffInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000DDE RID: 3550
	// (get) Token: 0x060075F2 RID: 30194 RVA: 0x002C454C File Offset: 0x002C274C
	// (set) Token: 0x060075F3 RID: 30195 RVA: 0x002C4554 File Offset: 0x002C2754
	public EntityUINotification Notification
	{
		get
		{
			return this.notification;
		}
		set
		{
			this.overridenBuff = null;
			this.notification = value;
			this.IsDirty = true;
			this.buffName = ((this.notification != null && this.notification.Buff != null) ? Localization.Get(this.notification.Buff.BuffClass.Name, false, null) : "");
			if (value != null && value.Buff != null)
			{
				EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
				for (int i = 0; i < entityPlayer.Buffs.ActiveBuffs.Count; i++)
				{
					if (!entityPlayer.Buffs.ActiveBuffs[i].BuffClass.Hidden && !entityPlayer.Buffs.ActiveBuffs[i].Paused)
					{
						this.overridenBuff = entityPlayer.Buffs.ActiveBuffs[i];
						return;
					}
				}
			}
		}
	}

	// Token: 0x060075F4 RID: 30196 RVA: 0x002C463C File Offset: 0x002C283C
	public override void Init()
	{
		base.Init();
		this.itemPreview = base.GetChildById("itemPreview");
		this.windowName = base.GetChildById("windowName");
		this.windowIcon = base.GetChildById("windowIcon");
		this.description = base.GetChildById("descriptionText");
		this.stats = base.GetChildById("statText");
		this.actionItemList = (XUiC_ItemActionList)base.GetChildById("itemActions");
		this.statButton = base.GetChildById("statButton");
		if (this.statButton != null)
		{
			this.statButton.OnPress += this.StatButton_OnPress;
		}
		this.descriptionButton = base.GetChildById("descriptionButton");
		if (this.descriptionButton != null)
		{
			this.descriptionButton.OnPress += this.DescriptionButton_OnPress;
		}
	}

	// Token: 0x060075F5 RID: 30197 RVA: 0x002C471A File Offset: 0x002C291A
	[PublicizedFrom(EAccessModifier.Private)]
	public void DescriptionButton_OnPress(XUiController _sender, int _mouseButton)
	{
		((XUiV_Button)this.statButton.ViewComponent).Selected = false;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = true;
		this.showStats = false;
		this.IsDirty = true;
	}

	// Token: 0x060075F6 RID: 30198 RVA: 0x002C4756 File Offset: 0x002C2956
	[PublicizedFrom(EAccessModifier.Private)]
	public void StatButton_OnPress(XUiController _sender, int _mouseButton)
	{
		((XUiV_Button)this.statButton.ViewComponent).Selected = true;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = false;
		this.showStats = true;
		this.IsDirty = true;
	}

	// Token: 0x060075F7 RID: 30199 RVA: 0x002C4792 File Offset: 0x002C2992
	public override void Deselect()
	{
		if (this.selectedEntry != null)
		{
			this.selectedEntry.IsSelected = false;
		}
	}

	// Token: 0x060075F8 RID: 30200 RVA: 0x0008A091 File Offset: 0x00088291
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060075F9 RID: 30201 RVA: 0x002C47A8 File Offset: 0x002C29A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.notification != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2465532423U)
		{
			if (num <= 1988587286U)
			{
				if (num != 1569673958U)
				{
					if (num == 1988587286U)
					{
						if (bindingName == "buffstatus")
						{
							value = ((flag && this.notification.Buff.Paused) ? Localization.Get("TwitchCooldownStatus_Paused", false, null) : "");
							return true;
						}
					}
				}
				else if (bindingName == "buffdescription")
				{
					value = (flag ? this.notification.Buff.BuffClass.Description : "");
					return true;
				}
			}
			else if (num != 2030550547U)
			{
				if (num == 2465532423U)
				{
					if (bindingName == "buffname")
					{
						value = (flag ? Localization.Get(this.notification.Buff.BuffClass.LocalizedName, false, null) : "");
						return true;
					}
				}
			}
			else if (bindingName == "buffcolor")
			{
				Color32 color = (flag ? this.notification.GetColor() : Color.white);
				value = string.Format("{0},{1},{2},{3}", new object[] { color.r, color.g, color.b, color.a });
				return true;
			}
		}
		else if (num <= 3154262838U)
		{
			if (num != 2929532957U)
			{
				if (num == 3154262838U)
				{
					if (bindingName == "showdescription")
					{
						value = (!this.showStats).ToString();
						return true;
					}
				}
			}
			else if (bindingName == "bufficon")
			{
				value = (flag ? this.notification.Icon : "");
				return true;
			}
		}
		else if (num != 3257770903U)
		{
			if (num == 4276755783U)
			{
				if (bindingName == "buffstats")
				{
					value = (flag ? XUiM_PlayerBuffs.GetInfoFromBuff(this.xui.playerUI.entityPlayer, this.notification, this.overridenBuff) : "");
					return true;
				}
			}
		}
		else if (bindingName == "showstats")
		{
			value = this.showStats.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060075FA RID: 30202 RVA: 0x002C4A40 File Offset: 0x002C2C40
	public void SetBuff(XUiC_ActiveBuffEntry buffEntry)
	{
		this.ensureReferencesSet();
		if (this.emptyInfoWindow != null && this.itemInfoWindow != null && buffEntry == null)
		{
			if (!this.itemInfoWindow.ViewComponent.IsVisible)
			{
				this.emptyInfoWindow.ViewComponent.IsVisible = true;
			}
			return;
		}
		this.selectedEntry = buffEntry;
		this.Notification = buffEntry.Notification;
		EntityUINotification entityUINotification = this.notification;
		this.actionItemList.SetCraftingActionList(XUiC_ItemActionList.ItemActionListTypes.Buff, buffEntry);
		if (this.selectedEntry != null)
		{
			base.RefreshBindings();
		}
		this.IsDirty = true;
	}

	// Token: 0x060075FB RID: 30203 RVA: 0x002C4AC6 File Offset: 0x002C2CC6
	public void SetBuffInfo(XUiC_ActiveBuffEntry buff)
	{
		if (buff != null)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.SetBuff(buff);
	}

	// Token: 0x060075FC RID: 30204 RVA: 0x002C4AE0 File Offset: 0x002C2CE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ensureReferencesSet()
	{
		if (this.emptyInfoWindow == null)
		{
			this.emptyInfoWindow = (XUiC_InfoWindow)this.xui.FindWindowGroupByName("backpack").GetChildById("emptyInfoPanel");
		}
		if (this.itemInfoWindow == null)
		{
			this.itemInfoWindow = (XUiC_ItemInfoWindow)this.xui.FindWindowGroupByName("backpack").GetChildById("itemInfoPanel");
		}
	}

	// Token: 0x040056F6 RID: 22262
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityUINotification notification;

	// Token: 0x040056F7 RID: 22263
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue overridenBuff;

	// Token: 0x040056F8 RID: 22264
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffEntry selectedEntry;

	// Token: 0x040056F9 RID: 22265
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName = "";

	// Token: 0x040056FA RID: 22266
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController itemPreview;

	// Token: 0x040056FB RID: 22267
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x040056FC RID: 22268
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x040056FD RID: 22269
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x040056FE RID: 22270
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController stats;

	// Token: 0x040056FF RID: 22271
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController craftingTime;

	// Token: 0x04005700 RID: 22272
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x04005701 RID: 22273
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x04005702 RID: 22274
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemInfoWindow itemInfoWindow;

	// Token: 0x04005703 RID: 22275
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showStats;

	// Token: 0x04005704 RID: 22276
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController statButton;

	// Token: 0x04005705 RID: 22277
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController descriptionButton;

	// Token: 0x04005706 RID: 22278
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 valueColor = new Color32(222, 206, 163, byte.MaxValue);
}
