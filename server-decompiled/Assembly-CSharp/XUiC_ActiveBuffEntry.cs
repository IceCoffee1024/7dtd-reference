using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ECD RID: 3789
[Preserve]
public class XUiC_ActiveBuffEntry : XUiC_SelectableEntry
{
	// Token: 0x17000DFB RID: 3579
	// (get) Token: 0x060075DB RID: 30171 RVA: 0x002C7227 File Offset: 0x002C5427
	// (set) Token: 0x060075DC RID: 30172 RVA: 0x002C7230 File Offset: 0x002C5430
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

	// Token: 0x17000DFC RID: 3580
	// (get) Token: 0x060075DD RID: 30173 RVA: 0x002C731E File Offset: 0x002C551E
	// (set) Token: 0x060075DE RID: 30174 RVA: 0x002C7326 File Offset: 0x002C5526
	public XUiC_BuffInfoWindow InfoWindow { get; set; }

	// Token: 0x060075DF RID: 30175 RVA: 0x002C7330 File Offset: 0x002C5530
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

	// Token: 0x060075E0 RID: 30176 RVA: 0x002C73AC File Offset: 0x002C55AC
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

	// Token: 0x060075E1 RID: 30177 RVA: 0x002C74EC File Offset: 0x002C56EC
	public override void Init()
	{
		base.Init();
		this.background = (XUiV_Sprite)base.GetChildById("background").ViewComponent;
		base.OnScroll += this.HandleOnScroll;
		this.IsDirty = true;
	}

	// Token: 0x060075E2 RID: 30178 RVA: 0x002C7528 File Offset: 0x002C5728
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

	// Token: 0x060075E3 RID: 30179 RVA: 0x002C7574 File Offset: 0x002C5774
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

	// Token: 0x060075E4 RID: 30180 RVA: 0x002C75E5 File Offset: 0x002C57E5
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060075E5 RID: 30181 RVA: 0x0008A995 File Offset: 0x00088B95
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x04005768 RID: 22376
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005769 RID: 22377
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName = "";

	// Token: 0x0400576A RID: 22378
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityUINotification notification;

	// Token: 0x0400576B RID: 22379
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue overridenBuff;
}
