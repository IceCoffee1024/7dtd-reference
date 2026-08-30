using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02001090 RID: 4240
[Preserve]
public class XUiC_TwitchVoteInfoEntry : XUiController
{
	// Token: 0x17000FA0 RID: 4000
	// (get) Token: 0x060086F2 RID: 34546 RVA: 0x003398F7 File Offset: 0x00337AF7
	// (set) Token: 0x060086F3 RID: 34547 RVA: 0x003398FF File Offset: 0x00337AFF
	public TwitchVote Vote
	{
		get
		{
			return this.vote;
		}
		set
		{
			base.ViewComponent.Enabled = value != null;
			this.vote = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000FA1 RID: 4001
	// (get) Token: 0x060086F4 RID: 34548 RVA: 0x0033991E File Offset: 0x00337B1E
	// (set) Token: 0x060086F5 RID: 34549 RVA: 0x00339926 File Offset: 0x00337B26
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x17000FA2 RID: 4002
	// (get) Token: 0x060086F6 RID: 34550 RVA: 0x0033992F File Offset: 0x00337B2F
	// (set) Token: 0x060086F7 RID: 34551 RVA: 0x00339937 File Offset: 0x00337B37
	public bool Tracked { get; set; }

	// Token: 0x060086F8 RID: 34552 RVA: 0x00339940 File Offset: 0x00337B40
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.vote != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1656712805U)
		{
			if (num != 765459171U)
			{
				if (num != 1129104269U)
				{
					if (num == 1656712805U)
					{
						if (bindingName == "rowstatesprite")
						{
							value = (this.Selected ? "ui_game_select_row" : "menu_empty");
							return true;
						}
					}
				}
				else if (bindingName == "showicon")
				{
					value = ((this.Owner != null) ? (this.Owner.TwitchEntryListWindow.VoteCategory == "").ToString() : "true");
					return true;
				}
			}
			else if (bindingName == "rowstatecolor")
			{
				value = (this.Selected ? "255,255,255,255" : (this.IsHovered ? this.hoverColor : this.rowColor));
				return true;
			}
		}
		else if (num <= 2801367993U)
		{
			if (num != 2104701544U)
			{
				if (num == 2801367993U)
				{
					if (bindingName == "votetitle")
					{
						value = (flag ? this.vote.VoteDescription : "");
						return true;
					}
				}
			}
			else if (bindingName == "votecolor")
			{
				if (flag)
				{
					if (this.vote.Enabled)
					{
						value = ((this.vote.TitleColor == "") ? this.enabledColor : this.vote.TitleColor);
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
			if (num == 4217867520U)
			{
				if (bindingName == "voteicon")
				{
					value = "";
					if (flag && this.vote.MainVoteType != null)
					{
						value = this.vote.MainVoteType.Icon;
					}
					return true;
				}
			}
		}
		else if (bindingName == "iconcolor")
		{
			value = "255,255,255,255";
			if (flag)
			{
				value = (this.vote.Enabled ? this.enabledColor : this.disabledColor);
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060086F9 RID: 34553 RVA: 0x002D97B0 File Offset: 0x002D79B0
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060086FA RID: 34554 RVA: 0x00339B84 File Offset: 0x00337D84
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		base.OnHovered(_isOver);
		if (this.Vote == null)
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

	// Token: 0x060086FB RID: 34555 RVA: 0x00339BB3 File Offset: 0x00337DB3
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchVoteInfoEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x060086FC RID: 34556 RVA: 0x002C0D09 File Offset: 0x002BEF09
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060086FD RID: 34557 RVA: 0x0008A069 File Offset: 0x00088269
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060086FE RID: 34558 RVA: 0x00339BD4 File Offset: 0x00337DD4
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

	// Token: 0x04006475 RID: 25717
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04006476 RID: 25718
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04006477 RID: 25719
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04006478 RID: 25720
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04006479 RID: 25721
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x0400647A RID: 25722
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x0400647B RID: 25723
	public new bool Selected;

	// Token: 0x0400647C RID: 25724
	public bool IsHovered;

	// Token: 0x0400647D RID: 25725
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVote vote;

	// Token: 0x0400647F RID: 25727
	public XUiC_TwitchVoteInfoEntryList Owner;
}
