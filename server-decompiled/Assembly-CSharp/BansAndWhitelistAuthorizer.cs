using System;
using UnityEngine.Scripting;

// Token: 0x02000796 RID: 1942
[Preserve]
public class BansAndWhitelistAuthorizer : AuthorizerAbs
{
	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x060039E7 RID: 14823 RVA: 0x00179AF1 File Offset: 0x00177CF1
	public override int Order
	{
		get
		{
			return 500;
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x060039E8 RID: 14824 RVA: 0x00179AF8 File Offset: 0x00177CF8
	public override string AuthorizerName
	{
		get
		{
			return "BansAndWhitelist";
		}
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x060039E9 RID: 14825 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x060039EA RID: 14826 RVA: 0x00179AFF File Offset: 0x00177CFF
	public override bool AuthorizerActive
	{
		get
		{
			return GameManager.Instance.adminTools != null;
		}
	}

	// Token: 0x060039EB RID: 14827 RVA: 0x00179B10 File Offset: 0x00177D10
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
