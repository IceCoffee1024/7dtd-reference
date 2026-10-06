using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A5 RID: 4261
[Preserve]
public class XUiC_TwitchActionEntry : XUiController
{
	// Token: 0x17000FF4 RID: 4084
	// (get) Token: 0x0600874D RID: 34637 RVA: 0x0033B5AE File Offset: 0x003397AE
	public bool isEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.action.IsInPreset(this.Owner.CurrentPreset) && this.action.Enabled;
		}
	}

	// Token: 0x17000FF5 RID: 4085
	// (get) Token: 0x0600874E RID: 34638 RVA: 0x0033B5D5 File Offset: 0x003397D5
	// (set) Token: 0x0600874F RID: 34639 RVA: 0x0033B5DD File Offset: 0x003397DD
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

	// Token: 0x17000FF6 RID: 4086
	// (get) Token: 0x06008750 RID: 34640 RVA: 0x0033B5FC File Offset: 0x003397FC
	// (set) Token: 0x06008751 RID: 34641 RVA: 0x0033B604 File Offset: 0x00339804
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x17000FF7 RID: 4087
	// (get) Token: 0x06008752 RID: 34642 RVA: 0x0033B60D File Offset: 0x0033980D
	// (set) Token: 0x06008753 RID: 34643 RVA: 0x0033B615 File Offset: 0x00339815
	public bool Tracked { get; set; }

	// Token: 0x06008754 RID: 34644 RVA: 0x0033B620 File Offset: 0x00339820
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

	// Token: 0x06008755 RID: 34645 RVA: 0x002E0AD8 File Offset: 0x002DECD8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06008756 RID: 34646 RVA: 0x0033B91E File Offset: 0x00339B1E
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

	// Token: 0x06008757 RID: 34647 RVA: 0x002C75E5 File Offset: 0x002C57E5
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008758 RID: 34648 RVA: 0x0008A995 File Offset: 0x00088B95
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008759 RID: 34649 RVA: 0x0033B950 File Offset: 0x00339B50
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

	// Token: 0x0600875A RID: 34650 RVA: 0x0033B996 File Offset: 0x00339B96
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchActionEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x0600875B RID: 34651 RVA: 0x0033B9B8 File Offset: 0x00339BB8
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

	// Token: 0x04006491 RID: 25745
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04006492 RID: 25746
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04006493 RID: 25747
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04006494 RID: 25748
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04006495 RID: 25749
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x04006496 RID: 25750
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x04006497 RID: 25751
	public new bool Selected;

	// Token: 0x04006498 RID: 25752
	public bool IsHovered;

	// Token: 0x04006499 RID: 25753
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction action;

	// Token: 0x0400649B RID: 25755
	public XUiC_TwitchActionEntryList Owner;
}
