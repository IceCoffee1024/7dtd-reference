using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ED8 RID: 3800
[Preserve]
public class XUiC_ActiveBuffEntry : XUiC_SelectableEntry
{
	// Token: 0x17000E01 RID: 3585
	// (get) Token: 0x06007600 RID: 30208 RVA: 0x002C8793 File Offset: 0x002C6993
	// (set) Token: 0x06007601 RID: 30209 RVA: 0x002C879C File Offset: 0x002C699C
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
			EntityUINotification entityUINotification = this.notification;
			this.buffName = ((((entityUINotification != null) ? entityUINotification.Buff : null) != null) ? this.notification.Buff.BuffClass.LocalizedName : "");
			base.ViewComponent.Enabled = value != null;
			if (((value != null) ? value.Buff : null) != null)
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

	// Token: 0x17000E02 RID: 3586
	// (get) Token: 0x06007602 RID: 30210 RVA: 0x002C888A File Offset: 0x002C6A8A
	// (set) Token: 0x06007603 RID: 30211 RVA: 0x002C8892 File Offset: 0x002C6A92
	public XUiC_BuffInfoWindow InfoWindow { get; set; }

	// Token: 0x06007604 RID: 30212 RVA: 0x002C889C File Offset: 0x002C6A9C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		if (isSelected)
		{
			this.InfoWindow.SetBuffInfo(this);
		}
		if (this.background != null)
		{
			this.background.Color = (isSelected ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			this.background.SpriteName = (isSelected ? "ui_game_select_row" : "menu_empty");
		}
	}

	// Token: 0x06007605 RID: 30213 RVA: 0x002C8918 File Offset: 0x002C6B18
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.notification != null;
		if (bindingName == "buffname")
		{
			value = (flag ? this.buffName : "");
			return true;
		}
		if (bindingName == "buffdisplayinfo")
		{
			value = (flag ? XUiM_PlayerBuffs.GetBuffDisplayInfo(this.notification, this.overridenBuff) : "");
			return true;
		}
		if (bindingName == "bufficon")
		{
			value = (flag ? this.notification.Icon : "");
			return true;
		}
		if (bindingName == "buffcolor")
		{
			Color32 color = (flag ? this.notification.GetColor() : Color.white);
			value = string.Format("{0},{1},{2},{3}", new object[] { color.r, color.g, color.b, color.a });
			return true;
		}
		if (!(bindingName == "fontcolor"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = ((flag && this.notification.Buff.Paused) ? "128,128,128,255" : "255,255,255,255");
		return true;
	}

	// Token: 0x06007606 RID: 30214 RVA: 0x002C8A58 File Offset: 0x002C6C58
	public override void Init()
	{
		base.Init();
		this.background = (XUiV_Sprite)base.GetChildById("background").ViewComponent;
		base.OnScroll += this.HandleOnScroll;
		this.IsDirty = true;
	}

	// Token: 0x06007607 RID: 30215 RVA: 0x002C8A94 File Offset: 0x002C6C94
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging pager = ((XUiC_ActiveBuffList)base.Parent).pager;
			if (pager == null)
			{
				return;
			}
			pager.PageDown();
			return;
		}
		else
		{
			XUiC_Paging pager2 = ((XUiC_ActiveBuffList)base.Parent).pager;
			if (pager2 == null)
			{
				return;
			}
			pager2.PageUp();
			return;
		}
	}

	// Token: 0x06007608 RID: 30216 RVA: 0x002C8AE0 File Offset: 0x002C6CE0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		if (this.background != null && !base.IsSelected)
		{
			if (_isOver && this.notification != null)
			{
				this.background.Color = new Color32(96, 96, 96, byte.MaxValue);
			}
			else
			{
				this.background.Color = new Color32(64, 64, 64, byte.MaxValue);
			}
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x06007609 RID: 30217 RVA: 0x002C8B51 File Offset: 0x002C6D51
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x0600760A RID: 30218 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0400578D RID: 22413
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x0400578E RID: 22414
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName = "";

	// Token: 0x0400578F RID: 22415
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityUINotification notification;

	// Token: 0x04005790 RID: 22416
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue overridenBuff;
}
