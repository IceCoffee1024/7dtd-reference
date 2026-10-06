using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E3B RID: 3643
[Preserve]
public class XUiC_PlayersBlockedListEntryBase : XUiController
{
	// Token: 0x17000CA4 RID: 3236
	// (get) Token: 0x06006F8B RID: 28555 RVA: 0x002B44FF File Offset: 0x002B26FF
	// (set) Token: 0x06006F8C RID: 28556 RVA: 0x002B4507 File Offset: 0x002B2707
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

	// Token: 0x17000CA5 RID: 3237
	// (get) Token: 0x06006F8D RID: 28557 RVA: 0x002B4517 File Offset: 0x002B2717
	[XuiXmlBinding("hasentry")]
	public bool HasEntry
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.PlayerId != null;
		}
	}

	// Token: 0x06006F8E RID: 28558 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006F8F RID: 28559 RVA: 0x002B4524 File Offset: 0x002B2724
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

	// Token: 0x06006F90 RID: 28560 RVA: 0x002B4561 File Offset: 0x002B2761
	public void Clear()
	{
		this.PlayerId = null;
		this.PlayerName.ClearPlayerData();
	}

	// Token: 0x06006F91 RID: 28561 RVA: 0x002B4575 File Offset: 0x002B2775
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

	// Token: 0x040054AC RID: 21676
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs playerId;

	// Token: 0x040054AD RID: 21677
	[XuiBindParent(true)]
	public readonly XUiC_BlockedPlayersList BlockList;

	// Token: 0x040054AE RID: 21678
	[XuiBindComponent("playerName", true)]
	public readonly XUiC_PlayerName PlayerName;

	// Token: 0x040054AF RID: 21679
	[XuiBindComponent("btnReportPlayer", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnReportPlayer;
}
