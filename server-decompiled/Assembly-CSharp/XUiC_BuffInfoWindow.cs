using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EDA RID: 3802
[Preserve]
public class XUiC_BuffInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000E15 RID: 3605
	// (get) Token: 0x0600768E RID: 30350 RVA: 0x002CAE48 File Offset: 0x002C9048
	// (set) Token: 0x0600768F RID: 30351 RVA: 0x002CAE50 File Offset: 0x002C9050
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

	// Token: 0x06007690 RID: 30352 RVA: 0x002CAF38 File Offset: 0x002C9138
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

	// Token: 0x06007691 RID: 30353 RVA: 0x002CB016 File Offset: 0x002C9216
	[PublicizedFrom(EAccessModifier.Private)]
	public void DescriptionButton_OnPress(XUiController _sender, int _mouseButton)
	{
		((XUiV_Button)this.statButton.ViewComponent).Selected = false;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = true;
		this.showStats = false;
		this.IsDirty = true;
	}

	// Token: 0x06007692 RID: 30354 RVA: 0x002CB052 File Offset: 0x002C9252
	[PublicizedFrom(EAccessModifier.Private)]
	public void StatButton_OnPress(XUiController _sender, int _mouseButton)
	{
		((XUiV_Button)this.statButton.ViewComponent).Selected = true;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = false;
		this.showStats = true;
		this.IsDirty = true;
	}

	// Token: 0x06007693 RID: 30355 RVA: 0x002CB08E File Offset: 0x002C928E
	public override void Deselect()
	{
		if (this.selectedEntry != null)
		{
			this.selectedEntry.IsSelected = false;
		}
	}

	// Token: 0x06007694 RID: 30356 RVA: 0x0008A9BD File Offset: 0x00088BBD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007695 RID: 30357 RVA: 0x002CB0A4 File Offset: 0x002C92A4
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

	// Token: 0x06007696 RID: 30358 RVA: 0x002CB33C File Offset: 0x002C953C
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

	// Token: 0x06007697 RID: 30359 RVA: 0x002CB3C2 File Offset: 0x002C95C2
	public void SetBuffInfo(XUiC_ActiveBuffEntry buff)
	{
		if (buff != null)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.SetBuff(buff);
	}

	// Token: 0x06007698 RID: 30360 RVA: 0x002CB3DC File Offset: 0x002C95DC
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

	// Token: 0x040057D3 RID: 22483
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityUINotification notification;

	// Token: 0x040057D4 RID: 22484
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue overridenBuff;

	// Token: 0x040057D5 RID: 22485
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffEntry selectedEntry;

	// Token: 0x040057D6 RID: 22486
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName = "";

	// Token: 0x040057D7 RID: 22487
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController itemPreview;

	// Token: 0x040057D8 RID: 22488
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x040057D9 RID: 22489
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x040057DA RID: 22490
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x040057DB RID: 22491
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController stats;

	// Token: 0x040057DC RID: 22492
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController craftingTime;

	// Token: 0x040057DD RID: 22493
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x040057DE RID: 22494
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x040057DF RID: 22495
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemInfoWindow itemInfoWindow;

	// Token: 0x040057E0 RID: 22496
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showStats;

	// Token: 0x040057E1 RID: 22497
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController statButton;

	// Token: 0x040057E2 RID: 22498
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController descriptionButton;

	// Token: 0x040057E3 RID: 22499
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 valueColor = new Color32(222, 206, 163, byte.MaxValue);
}
