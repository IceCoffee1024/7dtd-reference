using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010CA RID: 4298
[Preserve]
public class XUiC_TwitchVoteInfoEntry : XUiController
{
	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x06008886 RID: 34950 RVA: 0x00343FBB File Offset: 0x003421BB
	// (set) Token: 0x06008887 RID: 34951 RVA: 0x00343FC3 File Offset: 0x003421C3
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

	// Token: 0x1700101A RID: 4122
	// (get) Token: 0x06008888 RID: 34952 RVA: 0x00343FE2 File Offset: 0x003421E2
	// (set) Token: 0x06008889 RID: 34953 RVA: 0x00343FEA File Offset: 0x003421EA
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x1700101B RID: 4123
	// (get) Token: 0x0600888A RID: 34954 RVA: 0x00343FF3 File Offset: 0x003421F3
	// (set) Token: 0x0600888B RID: 34955 RVA: 0x00343FFB File Offset: 0x003421FB
	public bool Tracked { get; set; }

	// Token: 0x0600888C RID: 34956 RVA: 0x00344004 File Offset: 0x00342204
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

	// Token: 0x0600888D RID: 34957 RVA: 0x002E23C8 File Offset: 0x002E05C8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x0600888E RID: 34958 RVA: 0x00344248 File Offset: 0x00342448
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

	// Token: 0x0600888F RID: 34959 RVA: 0x00344277 File Offset: 0x00342477
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchVoteInfoEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x06008890 RID: 34960 RVA: 0x002C8B51 File Offset: 0x002C6D51
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008891 RID: 34961 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008892 RID: 34962 RVA: 0x00344298 File Offset: 0x00342498
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

	// Token: 0x040065BD RID: 26045
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x040065BE RID: 26046
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x040065BF RID: 26047
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x040065C0 RID: 26048
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x040065C1 RID: 26049
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x040065C2 RID: 26050
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x040065C3 RID: 26051
	public new bool Selected;

	// Token: 0x040065C4 RID: 26052
	public bool IsHovered;

	// Token: 0x040065C5 RID: 26053
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVote vote;

	// Token: 0x040065C7 RID: 26055
	public XUiC_TwitchVoteInfoEntryList Owner;
}
