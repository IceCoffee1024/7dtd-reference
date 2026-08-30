using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A9 RID: 4265
[Preserve]
public class XUiC_TwitchVoteInfoEntry : XUiController
{
	// Token: 0x17000FD9 RID: 4057
	// (get) Token: 0x060087E4 RID: 34788 RVA: 0x0033E11F File Offset: 0x0033C31F
	// (set) Token: 0x060087E5 RID: 34789 RVA: 0x0033E127 File Offset: 0x0033C327
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

	// Token: 0x17000FDA RID: 4058
	// (get) Token: 0x060087E6 RID: 34790 RVA: 0x0033E146 File Offset: 0x0033C346
	// (set) Token: 0x060087E7 RID: 34791 RVA: 0x0033E14E File Offset: 0x0033C34E
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x17000FDB RID: 4059
	// (get) Token: 0x060087E8 RID: 34792 RVA: 0x0033E157 File Offset: 0x0033C357
	// (set) Token: 0x060087E9 RID: 34793 RVA: 0x0033E15F File Offset: 0x0033C35F
	public bool Tracked { get; set; }

	// Token: 0x060087EA RID: 34794 RVA: 0x0033E168 File Offset: 0x0033C368
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

	// Token: 0x060087EB RID: 34795 RVA: 0x002DD73C File Offset: 0x002DB93C
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060087EC RID: 34796 RVA: 0x0033E3AC File Offset: 0x0033C5AC
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

	// Token: 0x060087ED RID: 34797 RVA: 0x0033E3DB File Offset: 0x0033C5DB
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchVoteInfoEntryList>().SelectedEntry = this;
		this.TwitchInfoUIHandler.SetEntry(this);
	}

	// Token: 0x060087EE RID: 34798 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060087EF RID: 34799 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060087F0 RID: 34800 RVA: 0x0033E3FC File Offset: 0x0033C5FC
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

	// Token: 0x0400653C RID: 25916
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x0400653D RID: 25917
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x0400653E RID: 25918
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x0400653F RID: 25919
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04006540 RID: 25920
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x04006541 RID: 25921
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x04006542 RID: 25922
	public new bool Selected;

	// Token: 0x04006543 RID: 25923
	public bool IsHovered;

	// Token: 0x04006544 RID: 25924
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVote vote;

	// Token: 0x04006546 RID: 25926
	public XUiC_TwitchVoteInfoEntryList Owner;
}
