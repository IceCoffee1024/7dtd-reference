using System;
using UnityEngine.Scripting;

// Token: 0x02000785 RID: 1925
[Preserve]
public class VersionAuthorizer : AuthorizerAbs
{
	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x0600395F RID: 14687 RVA: 0x00177B5A File Offset: 0x00175D5A
	public override int Order
	{
		get
		{
			return 70;
		}
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06003960 RID: 14688 RVA: 0x00177B5E File Offset: 0x00175D5E
	public override string AuthorizerName
	{
		get
		{
			return "VersionCheck";
		}
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06003961 RID: 14689 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003962 RID: 14690 RVA: 0x00177B68 File Offset: 0x00175D68
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
