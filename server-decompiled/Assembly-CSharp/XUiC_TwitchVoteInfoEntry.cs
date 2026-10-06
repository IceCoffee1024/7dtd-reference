using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010BA RID: 4282
[Preserve]
public class XUiC_TwitchVoteInfoEntry : XUiController
{
	// Token: 0x1700100E RID: 4110
	// (get) Token: 0x06008833 RID: 34867 RVA: 0x00341F37 File Offset: 0x00340137
	// (set) Token: 0x06008834 RID: 34868 RVA: 0x00341F3F File Offset: 0x0034013F
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

	// Token: 0x1700100F RID: 4111
	// (get) Token: 0x06008835 RID: 34869 RVA: 0x00341F5E File Offset: 0x0034015E
	// (set) Token: 0x06008836 RID: 34870 RVA: 0x00341F66 File Offset: 0x00340166
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x17001010 RID: 4112
	// (get) Token: 0x06008837 RID: 34871 RVA: 0x00341F6F File Offset: 0x0034016F
	// (set) Token: 0x06008838 RID: 34872 RVA: 0x00341F77 File Offset: 0x00340177
	public bool Tracked { get; set; }

	// Token: 0x06008839 RID: 34873 RVA: 0x00341F80 File Offset: 0x00340180
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

	// Token: 0x0600883A RID: 34874 RVA: 0x002E0AD8 File Offset: 0x002DECD8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x0600883B RID: 34875 RVA: 0x003421C4 File Offset: 0x003403C4
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

	// Token: 0x0600883C RID: 34876 RVA: 0x003421F3 File Offset: 0x003403F3
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchVoteInfoEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x0600883D RID: 34877 RVA: 0x002C75E5 File Offset: 0x002C57E5
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x0600883E RID: 34878 RVA: 0x0008A995 File Offset: 0x00088B95
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600883F RID: 34879 RVA: 0x00342214 File Offset: 0x00340414
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

	// Token: 0x0400657E RID: 25982
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x0400657F RID: 25983
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04006580 RID: 25984
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04006581 RID: 25985
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04006582 RID: 25986
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x04006583 RID: 25987
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x04006584 RID: 25988
	public new bool Selected;

	// Token: 0x04006585 RID: 25989
	public bool IsHovered;

	// Token: 0x04006586 RID: 25990
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVote vote;

	// Token: 0x04006588 RID: 25992
	public XUiC_TwitchVoteInfoEntryList Owner;
}
