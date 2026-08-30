using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EBE RID: 3774
[Preserve]
public class XUiC_ActiveBuffEntry : XUiC_SelectableEntry
{
	// Token: 0x17000DE0 RID: 3552
	// (get) Token: 0x060075E3 RID: 30179 RVA: 0x002C3E6F File Offset: 0x002C206F
	// (set) Token: 0x060075E4 RID: 30180 RVA: 0x002C3E78 File Offset: 0x002C2078
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

	// Token: 0x17000DE1 RID: 3553
	// (get) Token: 0x060075E5 RID: 30181 RVA: 0x002C3F66 File Offset: 0x002C2166
	// (set) Token: 0x060075E6 RID: 30182 RVA: 0x002C3F6E File Offset: 0x002C216E
	public XUiC_BuffInfoWindow InfoWindow { get; set; }

	// Token: 0x060075E7 RID: 30183 RVA: 0x002C3F78 File Offset: 0x002C2178
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

	// Token: 0x060075E8 RID: 30184 RVA: 0x002C3FF4 File Offset: 0x002C21F4
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

	// Token: 0x060075E9 RID: 30185 RVA: 0x002C4134 File Offset: 0x002C2334
	public override void Init()
	{
		base.Init();
		this.background = (XUiV_Sprite)base.GetChildById("background").ViewComponent;
		base.OnScroll += this.HandleOnScroll;
		this.IsDirty = true;
	}

	// Token: 0x060075EA RID: 30186 RVA: 0x002C4170 File Offset: 0x002C2370
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

	// Token: 0x060075EB RID: 30187 RVA: 0x002C41BC File Offset: 0x002C23BC
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

	// Token: 0x060075EC RID: 30188 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060075ED RID: 30189 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x04005731 RID: 22321
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005732 RID: 22322
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName = "";

	// Token: 0x04005733 RID: 22323
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityUINotification notification;

	// Token: 0x04005734 RID: 22324
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue overridenBuff;
}
