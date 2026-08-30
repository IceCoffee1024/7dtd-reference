using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200108C RID: 4236
[Preserve]
public class XUiC_TwitchLeaderboardEntry : XUiController
{
	// Token: 0x17000F9A RID: 3994
	// (get) Token: 0x060086CD RID: 34509 RVA: 0x00338DC9 File Offset: 0x00336FC9
	// (set) Token: 0x060086CE RID: 34510 RVA: 0x00338DD1 File Offset: 0x00336FD1
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

	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x060086CF RID: 34511 RVA: 0x00338DF0 File Offset: 0x00336FF0
	// (set) Token: 0x060086D0 RID: 34512 RVA: 0x00338DF8 File Offset: 0x00336FF8
	public XUiC_TwitchInfoWindowGroup TwitchInfoUIHandler { get; set; }

	// Token: 0x060086D1 RID: 34513 RVA: 0x00338E04 File Offset: 0x00337004
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

	// Token: 0x060086D2 RID: 34514 RVA: 0x002D97B0 File Offset: 0x002D79B0
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060086D3 RID: 34515 RVA: 0x002C0D09 File Offset: 0x002BEF09
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060086D4 RID: 34516 RVA: 0x00338E8B File Offset: 0x0033708B
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		base.GetParentByType<XUiC_TwitchLeaderboardEntryList>().SelectedEntry = this;
	}

	// Token: 0x060086D5 RID: 34517 RVA: 0x0008A069 File Offset: 0x00088269
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060086D6 RID: 34518 RVA: 0x00338EA0 File Offset: 0x003370A0
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

	// Token: 0x04006456 RID: 25686
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04006457 RID: 25687
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04006458 RID: 25688
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04006459 RID: 25689
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x0400645A RID: 25690
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x0400645B RID: 25691
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x0400645C RID: 25692
	public new bool Selected;

	// Token: 0x0400645D RID: 25693
	public bool IsHovered;

	// Token: 0x0400645E RID: 25694
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchLeaderboardEntry leaderboardEntry;

	// Token: 0x04006460 RID: 25696
	public XUiC_TwitchLeaderboardEntryList Owner;
}
