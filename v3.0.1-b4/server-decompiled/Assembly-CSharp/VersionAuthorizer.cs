using System;
using UnityEngine.Scripting;

// Token: 0x0200077B RID: 1915
[Preserve]
public class VersionAuthorizer : AuthorizerAbs
{
	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06003907 RID: 14599 RVA: 0x001758DA File Offset: 0x00173ADA
	public override int Order
	{
		get
		{
			return 70;
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06003908 RID: 14600 RVA: 0x001758DE File Offset: 0x00173ADE
	public override string AuthorizerName
	{
		get
		{
			return "VersionCheck";
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06003909 RID: 14601 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600390A RID: 14602 RVA: 0x001758E8 File Offset: 0x00173AE8
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		if (!string.Equals(Constants.cVersionInformation.LongStringNoBuild, _clientInfo.compatibilityVersion, StringComparison.Ordinal))
		{
			EAuthorizerSyncResult eauthorizerSyncResult = EAuthorizerSyncResult.SyncDeny;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.VersionMismatch;
			int num = 0;
			string longStringNoBuild = Constants.cVersionInformation.LongStringNoBuild;
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), longStringNoBuild)));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
