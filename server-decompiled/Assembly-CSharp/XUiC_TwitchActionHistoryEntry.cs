using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A7 RID: 4263
[Preserve]
public class XUiC_TwitchActionHistoryEntry : XUiController
{
	// Token: 0x17000FFA RID: 4090
	// (get) Token: 0x0600876B RID: 34667 RVA: 0x0033BE87 File Offset: 0x0033A087
	// (set) Token: 0x0600876C RID: 34668 RVA: 0x0033BE8F File Offset: 0x0033A08F
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

	// Token: 0x17000FFB RID: 4091
	// (get) Token: 0x0600876D RID: 34669 RVA: 0x0033BEAE File Offset: 0x0033A0AE
	// (set) Token: 0x0600876E RID: 34670 RVA: 0x0033BEB6 File Offset: 0x0033A0B6
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x0600876F RID: 34671 RVA: 0x0033BEC0 File Offset: 0x0033A0C0
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

	// Token: 0x06008770 RID: 34672 RVA: 0x002E0AD8 File Offset: 0x002DECD8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06008771 RID: 34673 RVA: 0x0033C18F File Offset: 0x0033A38F
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

	// Token: 0x06008772 RID: 34674 RVA: 0x0033C1BE File Offset: 0x0033A3BE
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchActionHistoryEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x06008773 RID: 34675 RVA: 0x002C75E5 File Offset: 0x002C57E5
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008774 RID: 34676 RVA: 0x0008A995 File Offset: 0x00088B95
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008775 RID: 34677 RVA: 0x0033C1E0 File Offset: 0x0033A3E0
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

	// Token: 0x040064A6 RID: 25766
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x040064A7 RID: 25767
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x040064A8 RID: 25768
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x040064A9 RID: 25769
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x040064AA RID: 25770
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x040064AB RID: 25771
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x040064AC RID: 25772
	public new bool Selected;

	// Token: 0x040064AD RID: 25773
	public bool IsHovered;

	// Token: 0x040064AE RID: 25774
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchActionHistoryEntry historyItem;

	// Token: 0x040064B0 RID: 25776
	public XUiC_TwitchActionHistoryEntryList Owner;
}
