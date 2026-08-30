using System;
using UnityEngine.Scripting;

// Token: 0x02000782 RID: 1922
[Preserve]
public class BansAndWhitelistAuthorizer : AuthorizerAbs
{
	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06003936 RID: 14646 RVA: 0x00176121 File Offset: 0x00174321
	public override int Order
	{
		get
		{
			return 500;
		}
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06003937 RID: 14647 RVA: 0x00176128 File Offset: 0x00174328
	public override string AuthorizerName
	{
		get
		{
			return "BansAndWhitelist";
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06003938 RID: 14648 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06003939 RID: 14649 RVA: 0x0017612F File Offset: 0x0017432F
	public override bool AuthorizerActive
	{
		get
		{
			return GameManager.Instance.adminTools != null;
		}
	}

	// Token: 0x0600393A RID: 14650 RVA: 0x00176140 File Offset: 0x00174340
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
