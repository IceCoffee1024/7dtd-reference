using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02001094 RID: 4244
[Preserve]
public class XUiC_TwitchActionEntry : XUiController
{
	// Token: 0x17000FBF RID: 4031
	// (get) Token: 0x060086FE RID: 34558 RVA: 0x00337796 File Offset: 0x00335996
	public bool isEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.action.IsInPreset(this.Owner.CurrentPreset) && this.action.Enabled;
		}
	}

	// Token: 0x17000FC0 RID: 4032
	// (get) Token: 0x060086FF RID: 34559 RVA: 0x003377BD File Offset: 0x003359BD
	// (set) Token: 0x06008700 RID: 34560 RVA: 0x003377C5 File Offset: 0x003359C5
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

	// Token: 0x17000FC1 RID: 4033
	// (get) Token: 0x06008701 RID: 34561 RVA: 0x003377E4 File Offset: 0x003359E4
	// (set) Token: 0x06008702 RID: 34562 RVA: 0x003377EC File Offset: 0x003359EC
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x17000FC2 RID: 4034
	// (get) Token: 0x06008703 RID: 34563 RVA: 0x003377F5 File Offset: 0x003359F5
	// (set) Token: 0x06008704 RID: 34564 RVA: 0x003377FD File Offset: 0x003359FD
	public bool Tracked { get; set; }

	// Token: 0x06008705 RID: 34565 RVA: 0x00337808 File Offset: 0x00335A08
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

	// Token: 0x06008706 RID: 34566 RVA: 0x002DD73C File Offset: 0x002DB93C
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06008707 RID: 34567 RVA: 0x00337B06 File Offset: 0x00335D06
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

	// Token: 0x06008708 RID: 34568 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008709 RID: 34569 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600870A RID: 34570 RVA: 0x00337B38 File Offset: 0x00335D38
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

	// Token: 0x0600870B RID: 34571 RVA: 0x00337B7E File Offset: 0x00335D7E
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchActionEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x0600870C RID: 34572 RVA: 0x00337BA0 File Offset: 0x00335DA0
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

	// Token: 0x0400644F RID: 25679
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04006450 RID: 25680
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04006451 RID: 25681
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04006452 RID: 25682
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04006453 RID: 25683
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x04006454 RID: 25684
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x04006455 RID: 25685
	public new bool Selected;

	// Token: 0x04006456 RID: 25686
	public bool IsHovered;

	// Token: 0x04006457 RID: 25687
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction action;

	// Token: 0x04006459 RID: 25689
	public XUiC_TwitchActionEntryList Owner;
}
