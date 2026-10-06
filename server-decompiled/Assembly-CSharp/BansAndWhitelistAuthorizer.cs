using System;
using UnityEngine.Scripting;

// Token: 0x0200078E RID: 1934
[Preserve]
public class BansAndWhitelistAuthorizer : AuthorizerAbs
{
	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x060039D0 RID: 14800 RVA: 0x00178B59 File Offset: 0x00176D59
	public override int Order
	{
		get
		{
			return 500;
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x060039D1 RID: 14801 RVA: 0x00178B60 File Offset: 0x00176D60
	public override string AuthorizerName
	{
		get
		{
			return "BansAndWhitelist";
		}
	}

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x060039D2 RID: 14802 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x060039D3 RID: 14803 RVA: 0x00178B67 File Offset: 0x00176D67
	public override bool AuthorizerActive
	{
		get
		{
			return GameManager.Instance.adminTools != null;
		}
	}

	// Token: 0x060039D4 RID: 14804 RVA: 0x00178B78 File Offset: 0x00176D78
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		AdminTools adminTools = GameManager.Instance.adminTools;
		DateTime dateTime;
		string text;
		if (adminTools.Blacklist.IsBanned(_clientInfo.PlatformId, out dateTime, out text))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.Banned, 0, dateTime, text)));
		}
		DateTime dateTime2;
		string text2;
		if (_clientInfo.CrossplatformId != null && adminTools.Blacklist.IsBanned(_clientInfo.CrossplatformId, out dateTime2, out text2))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.Banned, 0, dateTime2, text2)));
		}
		if (adminTools.Whitelist.IsWhiteListEnabled() && !adminTools.Whitelist.IsWhitelisted(_clientInfo) && !adminTools.Users.HasEntry(_clientInfo))
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.NotOnWhitelist, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
