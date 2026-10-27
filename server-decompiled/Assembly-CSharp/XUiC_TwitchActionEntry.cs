using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B5 RID: 4277
[Preserve]
public class XUiC_TwitchActionEntry : XUiController
{
	// Token: 0x17000FFF RID: 4095
	// (get) Token: 0x060087A0 RID: 34720 RVA: 0x0033D632 File Offset: 0x0033B832
	public bool isEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.action.IsInPreset(this.Owner.CurrentPreset) && this.action.Enabled;
		}
	}

	// Token: 0x17001000 RID: 4096
	// (get) Token: 0x060087A1 RID: 34721 RVA: 0x0033D659 File Offset: 0x0033B859
	// (set) Token: 0x060087A2 RID: 34722 RVA: 0x0033D661 File Offset: 0x0033B861
	public TwitchAction Action
	{
		get
		{
			return this.action;
		}
		set
		{
			base.ViewComponent.Enabled = value != null;
			this.action = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17001001 RID: 4097
	// (get) Token: 0x060087A3 RID: 34723 RVA: 0x0033D680 File Offset: 0x0033B880
	// (set) Token: 0x060087A4 RID: 34724 RVA: 0x0033D688 File Offset: 0x0033B888
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x17001002 RID: 4098
	// (get) Token: 0x060087A5 RID: 34725 RVA: 0x0033D691 File Offset: 0x0033B891
	// (set) Token: 0x060087A6 RID: 34726 RVA: 0x0033D699 File Offset: 0x0033B899
	public bool Tracked { get; set; }

	// Token: 0x060087A7 RID: 34727 RVA: 0x0033D6A4 File Offset: 0x0033B8A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.action != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1838951150U)
		{
			if (num <= 782028412U)
			{
				if (num != 765459171U)
				{
					if (num == 782028412U)
					{
						if (bindingName == "actioncommand")
						{
							value = (flag ? this.action.Command : "");
							return true;
						}
					}
				}
				else if (bindingName == "rowstatecolor")
				{
					value = (this.Selected ? "255,255,255,255" : (this.IsHovered ? this.hoverColor : this.rowColor));
					return true;
				}
			}
			else if (num != 1129104269U)
			{
				if (num != 1656712805U)
				{
					if (num == 1838951150U)
					{
						if (bindingName == "actionicon")
						{
							value = "";
							if (flag && this.action.DisplayCategory != null)
							{
								value = this.action.DisplayCategory.Icon;
							}
							return true;
						}
					}
				}
				else if (bindingName == "rowstatesprite")
				{
					value = (this.Selected ? "ui_game_select_row" : "menu_empty");
					return true;
				}
			}
			else if (bindingName == "showicon")
			{
				value = ((this.Owner != null) ? (this.Owner.TwitchEntryListWindow.ActionCategory == "").ToString() : "true");
				return true;
			}
		}
		else if (num <= 2938772023U)
		{
			if (num != 2511012101U)
			{
				if (num == 2938772023U)
				{
					if (bindingName == "actiontitle")
					{
						value = (flag ? (this.action.Title + this.GetModifiedWithColor()) : "");
						return true;
					}
				}
			}
			else if (bindingName == "commandcolor")
			{
				if (flag)
				{
					if (this.isEnabled)
					{
						if (this.action.IsPositive)
						{
							value = this.positiveColor;
						}
						else
						{
							value = this.negativeColor;
						}
					}
					else
					{
						value = this.disabledColor;
					}
				}
				return true;
			}
		}
		else if (num != 3106195591U)
		{
			if (num != 3291327539U)
			{
				if (num == 3644377122U)
				{
					if (bindingName == "textstatecolor")
					{
						value = "255,255,255,255";
						if (flag)
						{
							value = (this.isEnabled ? this.enabledColor : this.disabledColor);
						}
						return true;
					}
				}
			}
			else if (bindingName == "actiondescription")
			{
				value = (flag ? this.action.Description : "");
				return true;
			}
		}
		else if (bindingName == "iconcolor")
		{
			value = "255,255,255,255";
			if (flag)
			{
				value = (this.isEnabled ? this.enabledColor : this.disabledColor);
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060087A8 RID: 34728 RVA: 0x002E23C8 File Offset: 0x002E05C8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060087A9 RID: 34729 RVA: 0x0033D9A2 File Offset: 0x0033BBA2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		base.OnHovered(_isOver);
		if (this.Action == null)
		{
			this.IsHovered = false;
			return;
		}
		if (this.IsHovered != _isOver)
		{
			this.IsHovered = _isOver;
			base.RefreshBindings();
		}
	}

	// Token: 0x060087AA RID: 34730 RVA: 0x002C8B51 File Offset: 0x002C6D51
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060087AB RID: 34731 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060087AC RID: 34732 RVA: 0x0033D9D4 File Offset: 0x0033BBD4
	public string GetModifiedWithColor()
	{
		if (this.Action != null)
		{
			int num = this.Action.ModifiedCost - this.Action.DefaultCost;
			if (num > 0)
			{
				return "[FF0000]*[-]";
			}
			if (num < 0)
			{
				return "[00FF00]*[-]";
			}
		}
		return "";
	}

	// Token: 0x060087AD RID: 34733 RVA: 0x0033DA1A File Offset: 0x0033BC1A
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchActionEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x060087AE RID: 34734 RVA: 0x0033DA3C File Offset: 0x0033BC3C
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "enabled_color")
		{
			this.enabledColor = value;
			return true;
		}
		if (name == "disabled_color")
		{
			this.disabledColor = value;
			return true;
		}
		if (name == "positive_color")
		{
			this.positiveColor = value;
			return true;
		}
		if (name == "negative_color")
		{
			this.negativeColor = value;
			return true;
		}
		if (name == "row_color")
		{
			this.rowColor = value;
			return true;
		}
		if (!(name == "hover_color"))
		{
			return base.ParseAttribute(name, value);
		}
		this.hoverColor = value;
		return true;
	}

	// Token: 0x040064D0 RID: 25808
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x040064D1 RID: 25809
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x040064D2 RID: 25810
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x040064D3 RID: 25811
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x040064D4 RID: 25812
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x040064D5 RID: 25813
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x040064D6 RID: 25814
	public new bool Selected;

	// Token: 0x040064D7 RID: 25815
	public bool IsHovered;

	// Token: 0x040064D8 RID: 25816
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction action;

	// Token: 0x040064DA RID: 25818
	public XUiC_TwitchActionEntryList Owner;
}
