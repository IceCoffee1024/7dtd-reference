using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B7 RID: 4279
[Preserve]
public class XUiC_TwitchActionHistoryEntry : XUiController
{
	// Token: 0x17001005 RID: 4101
	// (get) Token: 0x060087BE RID: 34750 RVA: 0x0033DF0B File Offset: 0x0033C10B
	// (set) Token: 0x060087BF RID: 34751 RVA: 0x0033DF13 File Offset: 0x0033C113
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

	// Token: 0x17001006 RID: 4102
	// (get) Token: 0x060087C0 RID: 34752 RVA: 0x0033DF32 File Offset: 0x0033C132
	// (set) Token: 0x060087C1 RID: 34753 RVA: 0x0033DF3A File Offset: 0x0033C13A
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x060087C2 RID: 34754 RVA: 0x0033DF44 File Offset: 0x0033C144
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

	// Token: 0x060087C3 RID: 34755 RVA: 0x002E23C8 File Offset: 0x002E05C8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060087C4 RID: 34756 RVA: 0x0033E213 File Offset: 0x0033C413
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

	// Token: 0x060087C5 RID: 34757 RVA: 0x0033E242 File Offset: 0x0033C442
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchActionHistoryEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x060087C6 RID: 34758 RVA: 0x002C8B51 File Offset: 0x002C6D51
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060087C7 RID: 34759 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060087C8 RID: 34760 RVA: 0x0033E264 File Offset: 0x0033C464
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

	// Token: 0x040064E5 RID: 25829
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x040064E6 RID: 25830
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x040064E7 RID: 25831
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x040064E8 RID: 25832
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x040064E9 RID: 25833
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x040064EA RID: 25834
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x040064EB RID: 25835
	public new bool Selected;

	// Token: 0x040064EC RID: 25836
	public bool IsHovered;

	// Token: 0x040064ED RID: 25837
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchActionHistoryEntry historyItem;

	// Token: 0x040064EF RID: 25839
	public XUiC_TwitchActionHistoryEntryList Owner;
}
