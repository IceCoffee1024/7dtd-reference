using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EE5 RID: 3813
[Preserve]
public class XUiC_BuffInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000E19 RID: 3609
	// (get) Token: 0x060076B3 RID: 30387 RVA: 0x002CC304 File Offset: 0x002CA504
	// (set) Token: 0x060076B4 RID: 30388 RVA: 0x002CC30C File Offset: 0x002CA50C
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

	// Token: 0x060076B5 RID: 30389 RVA: 0x002CC3F4 File Offset: 0x002CA5F4
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

	// Token: 0x060076B6 RID: 30390 RVA: 0x002CC4D2 File Offset: 0x002CA6D2
	[PublicizedFrom(EAccessModifier.Private)]
	public void DescriptionButton_OnPress(XUiController _sender, int _mouseButton)
	{
		((XUiV_Button)this.statButton.ViewComponent).Selected = false;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = true;
		this.showStats = false;
		this.IsDirty = true;
	}

	// Token: 0x060076B7 RID: 30391 RVA: 0x002CC50E File Offset: 0x002CA70E
	[PublicizedFrom(EAccessModifier.Private)]
	public void StatButton_OnPress(XUiController _sender, int _mouseButton)
	{
		((XUiV_Button)this.statButton.ViewComponent).Selected = true;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = false;
		this.showStats = true;
		this.IsDirty = true;
	}

	// Token: 0x060076B8 RID: 30392 RVA: 0x002CC54A File Offset: 0x002CA74A
	public override void Deselect()
	{
		if (this.selectedEntry != null)
		{
			this.selectedEntry.IsSelected = false;
		}
	}

	// Token: 0x060076B9 RID: 30393 RVA: 0x0008ABF1 File Offset: 0x00088DF1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x060076BA RID: 30394 RVA: 0x002CC560 File Offset: 0x002CA760
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

	// Token: 0x060076BB RID: 30395 RVA: 0x002CC7F8 File Offset: 0x002CA9F8
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

	// Token: 0x060076BC RID: 30396 RVA: 0x002CC87E File Offset: 0x002CAA7E
	public void SetBuffInfo(XUiC_ActiveBuffEntry buff)
	{
		if (buff != null)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.SetBuff(buff);
	}

	// Token: 0x060076BD RID: 30397 RVA: 0x002CC898 File Offset: 0x002CAA98
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

	// Token: 0x040057F8 RID: 22520
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityUINotification notification;

	// Token: 0x040057F9 RID: 22521
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue overridenBuff;

	// Token: 0x040057FA RID: 22522
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ActiveBuffEntry selectedEntry;

	// Token: 0x040057FB RID: 22523
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName = "";

	// Token: 0x040057FC RID: 22524
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController itemPreview;

	// Token: 0x040057FD RID: 22525
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x040057FE RID: 22526
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x040057FF RID: 22527
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x04005800 RID: 22528
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController stats;

	// Token: 0x04005801 RID: 22529
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController craftingTime;

	// Token: 0x04005802 RID: 22530
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x04005803 RID: 22531
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x04005804 RID: 22532
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemInfoWindow itemInfoWindow;

	// Token: 0x04005805 RID: 22533
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showStats;

	// Token: 0x04005806 RID: 22534
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController statButton;

	// Token: 0x04005807 RID: 22535
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController descriptionButton;

	// Token: 0x04005808 RID: 22536
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 valueColor = new Color32(222, 206, 163, byte.MaxValue);
}
