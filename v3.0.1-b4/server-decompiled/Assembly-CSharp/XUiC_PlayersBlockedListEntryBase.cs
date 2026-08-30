using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E17 RID: 3607
[Preserve]
public class XUiC_PlayersBlockedListEntryBase : XUiController
{
	// Token: 0x17000C70 RID: 3184
	// (get) Token: 0x06006EF3 RID: 28403 RVA: 0x002ADC3B File Offset: 0x002ABE3B
	// (set) Token: 0x06006EF4 RID: 28404 RVA: 0x002ADC43 File Offset: 0x002ABE43
	public PlatformUserIdentifierAbs PlayerId
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.playerId;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.playerId = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C71 RID: 3185
	// (get) Token: 0x06006EF5 RID: 28405 RVA: 0x002ADC53 File Offset: 0x002ABE53
	[XuiXmlBinding("hasentry")]
	public bool HasEntry
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.PlayerId != null;
		}
	}

	// Token: 0x06006EF6 RID: 28406 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006EF7 RID: 28407 RVA: 0x002ADC60 File Offset: 0x002ABE60
	public virtual void UpdateEntry(PlatformUserIdentifierAbs _playerId)
	{
		BlockedPlayerList.ListEntry playerStateInfo = BlockedPlayerList.Instance.GetPlayerStateInfo(_playerId);
		if (playerStateInfo != null)
		{
			this.PlayerId = _playerId;
			this.PlayerName.UpdatePlayerData(playerStateInfo.PlayerData, true, null);
			return;
		}
		this.Clear();
	}

	// Token: 0x06006EF8 RID: 28408 RVA: 0x002ADC9D File Offset: 0x002ABE9D
	public void Clear()
	{
		this.PlayerId = null;
		this.PlayerName.ClearPlayerData();
	}

	// Token: 0x06006EF9 RID: 28409 RVA: 0x002ADCB1 File Offset: 0x002ABEB1
	[XuiBindEvent("OnPress", "btnReportPlayer")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void reportPlayerPressed(XUiController _sender, int _mouseButton)
	{
		if (PlatformManager.MultiPlatform.PlayerReporting == null || this.PlayerId == null)
		{
			return;
		}
		XUiC_ReportPlayer.Open(BlockedPlayerList.Instance.GetPlayerStateInfo(this.PlayerId).PlayerData, EnumReportCategory.None, "");
	}

	// Token: 0x040053D1 RID: 21457
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs playerId;

	// Token: 0x040053D2 RID: 21458
	[XuiBindParent(true)]
	public readonly XUiC_BlockedPlayersList BlockList;

	// Token: 0x040053D3 RID: 21459
	[XuiBindComponent("playerName", true)]
	public readonly XUiC_PlayerName PlayerName;

	// Token: 0x040053D4 RID: 21460
	[XuiBindComponent("btnReportPlayer", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnReportPlayer;
}
