using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010C6 RID: 4294
[Preserve]
public class XUiC_TwitchLeaderboardEntry : XUiController
{
	// Token: 0x17001013 RID: 4115
	// (get) Token: 0x06008861 RID: 34913 RVA: 0x0034348D File Offset: 0x0034168D
	// (set) Token: 0x06008862 RID: 34914 RVA: 0x00343495 File Offset: 0x00341695
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

	// Token: 0x17001014 RID: 4116
	// (get) Token: 0x06008863 RID: 34915 RVA: 0x003434B4 File Offset: 0x003416B4
	// (set) Token: 0x06008864 RID: 34916 RVA: 0x003434BC File Offset: 0x003416BC
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x06008865 RID: 34917 RVA: 0x003434C8 File Offset: 0x003416C8
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

	// Token: 0x06008866 RID: 34918 RVA: 0x002E23C8 File Offset: 0x002E05C8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06008867 RID: 34919 RVA: 0x002C8B51 File Offset: 0x002C6D51
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008868 RID: 34920 RVA: 0x0034354F File Offset: 0x0034174F
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchLeaderboardEntryList>().SelectedEntry = this;
	}

	// Token: 0x06008869 RID: 34921 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600886A RID: 34922 RVA: 0x00343564 File Offset: 0x00341764
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

	// Token: 0x0400659E RID: 26014
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x0400659F RID: 26015
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x040065A0 RID: 26016
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x040065A1 RID: 26017
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x040065A2 RID: 26018
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x040065A3 RID: 26019
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x040065A4 RID: 26020
	public new bool Selected;

	// Token: 0x040065A5 RID: 26021
	public bool IsHovered;

	// Token: 0x040065A6 RID: 26022
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchLeaderboardEntry leaderboardEntry;

	// Token: 0x040065A8 RID: 26024
	public XUiC_TwitchLeaderboardEntryList Owner;
}
