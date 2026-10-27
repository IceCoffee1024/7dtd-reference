using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E46 RID: 3654
[Preserve]
public class XUiC_PlayersBlockedListEntryBase : XUiController
{
	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x06006FB0 RID: 28592 RVA: 0x002B5A6B File Offset: 0x002B3C6B
	// (set) Token: 0x06006FB1 RID: 28593 RVA: 0x002B5A73 File Offset: 0x002B3C73
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

	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x06006FB2 RID: 28594 RVA: 0x002B5A83 File Offset: 0x002B3C83
	[XuiXmlBinding("hasentry")]
	public bool HasEntry
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.PlayerId != null;
		}
	}

	// Token: 0x06006FB3 RID: 28595 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006FB4 RID: 28596 RVA: 0x002B5A90 File Offset: 0x002B3C90
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

	// Token: 0x06006FB5 RID: 28597 RVA: 0x002B5ACD File Offset: 0x002B3CCD
	public void Clear()
	{
		this.PlayerId = null;
		this.PlayerName.ClearPlayerData();
	}

	// Token: 0x06006FB6 RID: 28598 RVA: 0x002B5AE1 File Offset: 0x002B3CE1
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

	// Token: 0x040054D1 RID: 21713
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs playerId;

	// Token: 0x040054D2 RID: 21714
	[XuiBindParent(true)]
	public readonly XUiC_BlockedPlayersList BlockList;

	// Token: 0x040054D3 RID: 21715
	[XuiBindComponent("playerName", true)]
	public readonly XUiC_PlayerName PlayerName;

	// Token: 0x040054D4 RID: 21716
	[XuiBindComponent("btnReportPlayer", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnReportPlayer;
}
