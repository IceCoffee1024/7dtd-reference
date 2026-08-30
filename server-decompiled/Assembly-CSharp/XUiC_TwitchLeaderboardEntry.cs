using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A5 RID: 4261
[Preserve]
public class XUiC_TwitchLeaderboardEntry : XUiController
{
	// Token: 0x17000FD3 RID: 4051
	// (get) Token: 0x060087BF RID: 34751 RVA: 0x0033D5F1 File Offset: 0x0033B7F1
	// (set) Token: 0x060087C0 RID: 34752 RVA: 0x0033D5F9 File Offset: 0x0033B7F9
	public TwitchLeaderboardEntry LeaderboardEntry
	{
		get
		{
			return this.leaderboardEntry;
		}
		set
		{
			base.ViewComponent.Enabled = value != null;
			this.leaderboardEntry = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000FD4 RID: 4052
	// (get) Token: 0x060087C1 RID: 34753 RVA: 0x0033D618 File Offset: 0x0033B818
	// (set) Token: 0x060087C2 RID: 34754 RVA: 0x0033D620 File Offset: 0x0033B820
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x060087C3 RID: 34755 RVA: 0x0033D62C File Offset: 0x0033B82C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.leaderboardEntry != null;
		if (bindingName == "username")
		{
			value = (flag ? string.Format("[{0}]{1}[-]", this.leaderboardEntry.UserColor, this.leaderboardEntry.UserName) : "");
			return true;
		}
		if (!(bindingName == "kills"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = (flag ? this.leaderboardEntry.Kills.ToString() : "");
		return true;
	}

	// Token: 0x060087C4 RID: 34756 RVA: 0x002DD73C File Offset: 0x002DB93C
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060087C5 RID: 34757 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060087C6 RID: 34758 RVA: 0x0033D6B3 File Offset: 0x0033B8B3
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchLeaderboardEntryList>().SelectedEntry = this;
	}

	// Token: 0x060087C7 RID: 34759 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060087C8 RID: 34760 RVA: 0x0033D6C8 File Offset: 0x0033B8C8
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

	// Token: 0x0400651D RID: 25885
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x0400651E RID: 25886
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x0400651F RID: 25887
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04006520 RID: 25888
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04006521 RID: 25889
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x04006522 RID: 25890
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x04006523 RID: 25891
	public new bool Selected;

	// Token: 0x04006524 RID: 25892
	public bool IsHovered;

	// Token: 0x04006525 RID: 25893
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchLeaderboardEntry leaderboardEntry;

	// Token: 0x04006527 RID: 25895
	public XUiC_TwitchLeaderboardEntryList Owner;
}
