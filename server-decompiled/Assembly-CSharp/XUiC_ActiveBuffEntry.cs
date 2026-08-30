using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EA9 RID: 3753
[Preserve]
public class XUiC_ActiveBuffEntry : XUiC_SelectableEntry
{
	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x06007542 RID: 30018 RVA: 0x002C094B File Offset: 0x002BEB4B
	// (set) Token: 0x06007543 RID: 30019 RVA: 0x002C0954 File Offset: 0x002BEB54
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

	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x06007544 RID: 30020 RVA: 0x002C0A42 File Offset: 0x002BEC42
	// (set) Token: 0x06007545 RID: 30021 RVA: 0x002C0A4A File Offset: 0x002BEC4A
	public XUiC_BuffInfoWindow InfoWindow { get; set; }

	// Token: 0x06007546 RID: 30022 RVA: 0x002C0A54 File Offset: 0x002BEC54
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

	// Token: 0x06007547 RID: 30023 RVA: 0x002C0AD0 File Offset: 0x002BECD0
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

	// Token: 0x06007548 RID: 30024 RVA: 0x002C0C10 File Offset: 0x002BEE10
	public override void Init()
	{
		base.Init();
		this.background = (XUiV_Sprite)base.GetChildById("background").ViewComponent;
		base.OnScroll += this.HandleOnScroll;
		this.IsDirty = true;
	}

	// Token: 0x06007549 RID: 30025 RVA: 0x002C0C4C File Offset: 0x002BEE4C
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

	// Token: 0x0600754A RID: 30026 RVA: 0x002C0C98 File Offset: 0x002BEE98
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

	// Token: 0x0600754B RID: 30027 RVA: 0x002C0D09 File Offset: 0x002BEF09
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x0600754C RID: 30028 RVA: 0x0008A069 File Offset: 0x00088269
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0400568C RID: 22156
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x0400568D RID: 22157
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName = "";

	// Token: 0x0400568E RID: 22158
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityUINotification notification;

	// Token: 0x0400568F RID: 22159
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue overridenBuff;
}
