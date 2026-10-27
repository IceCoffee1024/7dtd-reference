using System;
using UnityEngine.Scripting;

// Token: 0x0200078F RID: 1935
[Preserve]
public class VersionAuthorizer : AuthorizerAbs
{
	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x060039B8 RID: 14776 RVA: 0x001792AA File Offset: 0x001774AA
	public override int Order
	{
		get
		{
			return 70;
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x060039B9 RID: 14777 RVA: 0x001792AE File Offset: 0x001774AE
	public override string AuthorizerName
	{
		get
		{
			return "VersionCheck";
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x060039BA RID: 14778 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039BB RID: 14779 RVA: 0x001792B8 File Offset: 0x001774B8
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
