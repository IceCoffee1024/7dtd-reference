using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B6 RID: 4278
[Preserve]
public class XUiC_TwitchLeaderboardEntry : XUiController
{
	// Token: 0x17001008 RID: 4104
	// (get) Token: 0x0600880E RID: 34830 RVA: 0x00341409 File Offset: 0x0033F609
	// (set) Token: 0x0600880F RID: 34831 RVA: 0x00341411 File Offset: 0x0033F611
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

	// Token: 0x17001009 RID: 4105
	// (get) Token: 0x06008810 RID: 34832 RVA: 0x00341430 File Offset: 0x0033F630
	// (set) Token: 0x06008811 RID: 34833 RVA: 0x00341438 File Offset: 0x0033F638
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x06008812 RID: 34834 RVA: 0x00341444 File Offset: 0x0033F644
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

	// Token: 0x06008813 RID: 34835 RVA: 0x002E0AD8 File Offset: 0x002DECD8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06008814 RID: 34836 RVA: 0x002C75E5 File Offset: 0x002C57E5
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008815 RID: 34837 RVA: 0x003414CB File Offset: 0x0033F6CB
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchLeaderboardEntryList>().SelectedEntry = this;
	}

	// Token: 0x06008816 RID: 34838 RVA: 0x0008A995 File Offset: 0x00088B95
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008817 RID: 34839 RVA: 0x003414E0 File Offset: 0x0033F6E0
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

	// Token: 0x0400655F RID: 25951
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04006560 RID: 25952
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04006561 RID: 25953
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04006562 RID: 25954
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04006563 RID: 25955
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x04006564 RID: 25956
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x04006565 RID: 25957
	public new bool Selected;

	// Token: 0x04006566 RID: 25958
	public bool IsHovered;

	// Token: 0x04006567 RID: 25959
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchLeaderboardEntry leaderboardEntry;

	// Token: 0x04006569 RID: 25961
	public XUiC_TwitchLeaderboardEntryList Owner;
}
