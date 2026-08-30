using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200107B RID: 4219
[Preserve]
public class XUiC_TwitchActionEntry : XUiController
{
	// Token: 0x17000F86 RID: 3974
	// (get) Token: 0x0600860C RID: 34316 RVA: 0x00332F6E File Offset: 0x0033116E
	public bool isEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.action.IsInPreset(this.Owner.CurrentPreset) && this.action.Enabled;
		}
	}

	// Token: 0x17000F87 RID: 3975
	// (get) Token: 0x0600860D RID: 34317 RVA: 0x00332F95 File Offset: 0x00331195
	// (set) Token: 0x0600860E RID: 34318 RVA: 0x00332F9D File Offset: 0x0033119D
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

	// Token: 0x17000F88 RID: 3976
	// (get) Token: 0x0600860F RID: 34319 RVA: 0x00332FBC File Offset: 0x003311BC
	// (set) Token: 0x06008610 RID: 34320 RVA: 0x00332FC4 File Offset: 0x003311C4
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x17000F89 RID: 3977
	// (get) Token: 0x06008611 RID: 34321 RVA: 0x00332FCD File Offset: 0x003311CD
	// (set) Token: 0x06008612 RID: 34322 RVA: 0x00332FD5 File Offset: 0x003311D5
	public bool Tracked { get; set; }

	// Token: 0x06008613 RID: 34323 RVA: 0x00332FE0 File Offset: 0x003311E0
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

	// Token: 0x06008614 RID: 34324 RVA: 0x002D97B0 File Offset: 0x002D79B0
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06008615 RID: 34325 RVA: 0x003332DE File Offset: 0x003314DE
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

	// Token: 0x06008616 RID: 34326 RVA: 0x002C0D09 File Offset: 0x002BEF09
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008617 RID: 34327 RVA: 0x0008A069 File Offset: 0x00088269
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008618 RID: 34328 RVA: 0x00333310 File Offset: 0x00331510
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

	// Token: 0x06008619 RID: 34329 RVA: 0x00333356 File Offset: 0x00331556
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchActionEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x0600861A RID: 34330 RVA: 0x00333378 File Offset: 0x00331578
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

	// Token: 0x04006388 RID: 25480
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04006389 RID: 25481
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x0400638A RID: 25482
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x0400638B RID: 25483
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x0400638C RID: 25484
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x0400638D RID: 25485
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x0400638E RID: 25486
	public new bool Selected;

	// Token: 0x0400638F RID: 25487
	public bool IsHovered;

	// Token: 0x04006390 RID: 25488
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchAction action;

	// Token: 0x04006392 RID: 25490
	public XUiC_TwitchActionEntryList Owner;
}
