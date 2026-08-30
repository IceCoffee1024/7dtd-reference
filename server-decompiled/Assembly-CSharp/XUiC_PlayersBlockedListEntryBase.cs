using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E2C RID: 3628
[Preserve]
public class XUiC_PlayersBlockedListEntryBase : XUiController
{
	// Token: 0x17000C89 RID: 3209
	// (get) Token: 0x06006F93 RID: 28563 RVA: 0x002B114F File Offset: 0x002AF34F
	// (set) Token: 0x06006F94 RID: 28564 RVA: 0x002B1157 File Offset: 0x002AF357
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

	// Token: 0x17000C8A RID: 3210
	// (get) Token: 0x06006F95 RID: 28565 RVA: 0x002B1167 File Offset: 0x002AF367
	[XuiXmlBinding("hasentry")]
	public bool HasEntry
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.PlayerId != null;
		}
	}

	// Token: 0x06006F96 RID: 28566 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006F97 RID: 28567 RVA: 0x002B1174 File Offset: 0x002AF374
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

	// Token: 0x06006F98 RID: 28568 RVA: 0x002B11B1 File Offset: 0x002AF3B1
	public void Clear()
	{
		this.PlayerId = null;
		this.PlayerName.ClearPlayerData();
	}

	// Token: 0x06006F99 RID: 28569 RVA: 0x002B11C5 File Offset: 0x002AF3C5
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

	// Token: 0x04005475 RID: 21621
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs playerId;

	// Token: 0x04005476 RID: 21622
	[XuiBindParent(true)]
	public readonly XUiC_BlockedPlayersList BlockList;

	// Token: 0x04005477 RID: 21623
	[XuiBindComponent("playerName", true)]
	public readonly XUiC_PlayerName PlayerName;

	// Token: 0x04005478 RID: 21624
	[XuiBindComponent("btnReportPlayer", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnReportPlayer;
}
