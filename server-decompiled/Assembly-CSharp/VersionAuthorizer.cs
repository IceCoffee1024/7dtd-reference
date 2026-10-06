using System;
using UnityEngine.Scripting;

// Token: 0x02000787 RID: 1927
[Preserve]
public class VersionAuthorizer : AuthorizerAbs
{
	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x060039A1 RID: 14753 RVA: 0x00178312 File Offset: 0x00176512
	public override int Order
	{
		get
		{
			return 70;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x060039A2 RID: 14754 RVA: 0x00178316 File Offset: 0x00176516
	public override string AuthorizerName
	{
		get
		{
			return "VersionCheck";
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x060039A3 RID: 14755 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039A4 RID: 14756 RVA: 0x00178320 File Offset: 0x00176520
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
