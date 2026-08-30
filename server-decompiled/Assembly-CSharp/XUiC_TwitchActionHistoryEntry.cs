using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02001096 RID: 4246
[Preserve]
public class XUiC_TwitchActionHistoryEntry : XUiController
{
	// Token: 0x17000FC5 RID: 4037
	// (get) Token: 0x0600871C RID: 34588 RVA: 0x0033806F File Offset: 0x0033626F
	// (set) Token: 0x0600871D RID: 34589 RVA: 0x00338077 File Offset: 0x00336277
	public TwitchActionHistoryEntry HistoryItem
	{
		get
		{
			return this.historyItem;
		}
		set
		{
			base.ViewComponent.Enabled = value != null;
			this.historyItem = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000FC6 RID: 4038
	// (get) Token: 0x0600871E RID: 34590 RVA: 0x00338096 File Offset: 0x00336296
	// (set) Token: 0x0600871F RID: 34591 RVA: 0x0033809E File Offset: 0x0033629E
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x06008720 RID: 34592 RVA: 0x003380A8 File Offset: 0x003362A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.historyItem != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2472102578U)
		{
			if (num <= 1320097209U)
			{
				if (num != 765459171U)
				{
					if (num == 1320097209U)
					{
						if (bindingName == "username")
						{
							if (flag)
							{
								if (this.historyItem.IsRefunded)
								{
									value = this.historyItem.UserName;
								}
								else
								{
									value = string.Format("[{0}]{1}[-]", this.historyItem.UserColor, this.historyItem.UserName);
								}
							}
							else
							{
								value = "";
							}
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
			else if (num != 1656712805U)
			{
				if (num == 2472102578U)
				{
					if (bindingName == "command")
					{
						value = (flag ? this.historyItem.Command : "");
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
		else if (num <= 3284901973U)
		{
			if (num != 2511012101U)
			{
				if (num == 3284901973U)
				{
					if (bindingName == "command_with_cost")
					{
						value = (flag ? this.historyItem.Command : "");
						return true;
					}
				}
			}
			else if (bindingName == "commandcolor")
			{
				if (flag)
				{
					if (this.historyItem.Action != null)
					{
						if (this.historyItem.IsRefunded)
						{
							value = this.disabledColor;
						}
						else if (this.historyItem.Action.IsPositive)
						{
							value = this.positiveColor;
						}
						else
						{
							value = this.negativeColor;
						}
					}
					else if (this.historyItem.Vote != null)
					{
						value = this.historyItem.Vote.TitleColor;
					}
					else if (this.historyItem.EventEntry != null)
					{
						value = "255,255,255,255";
					}
				}
				return true;
			}
		}
		else if (num != 3644377122U)
		{
			if (num == 3898356536U)
			{
				if (bindingName == "cost")
				{
					value = (flag ? this.historyItem.Action.CurrentCost.ToString() : "");
					return true;
				}
			}
		}
		else if (bindingName == "textstatecolor")
		{
			value = "255,255,255,255";
			if (flag)
			{
				value = (this.historyItem.IsRefunded ? this.disabledColor : this.enabledColor);
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06008721 RID: 34593 RVA: 0x002DD73C File Offset: 0x002DB93C
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06008722 RID: 34594 RVA: 0x00338377 File Offset: 0x00336577
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		base.OnHovered(_isOver);
		if (this.historyItem == null)
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

	// Token: 0x06008723 RID: 34595 RVA: 0x003383A6 File Offset: 0x003365A6
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchActionHistoryEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x06008724 RID: 34596 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008725 RID: 34597 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008726 RID: 34598 RVA: 0x003383C8 File Offset: 0x003365C8
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

	// Token: 0x04006464 RID: 25700
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04006465 RID: 25701
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04006466 RID: 25702
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04006467 RID: 25703
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04006468 RID: 25704
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x04006469 RID: 25705
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x0400646A RID: 25706
	public new bool Selected;

	// Token: 0x0400646B RID: 25707
	public bool IsHovered;

	// Token: 0x0400646C RID: 25708
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchActionHistoryEntry historyItem;

	// Token: 0x0400646E RID: 25710
	public XUiC_TwitchActionHistoryEntryList Owner;
}
