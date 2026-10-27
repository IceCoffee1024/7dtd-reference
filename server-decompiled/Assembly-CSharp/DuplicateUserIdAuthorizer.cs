using System;
using System.Collections.ObjectModel;
using UnityEngine.Scripting;

// Token: 0x0200078E RID: 1934
[Preserve]
public class DuplicateUserIdAuthorizer : AuthorizerAbs
{
	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x060039B3 RID: 14771 RVA: 0x001791E9 File Offset: 0x001773E9
	public override int Order
	{
		get
		{
			return 60;
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x060039B4 RID: 14772 RVA: 0x001791ED File Offset: 0x001773ED
	public override string AuthorizerName
	{
		get
		{
			return "DuplicateUserId";
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x060039B5 RID: 14773 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039B6 RID: 14774 RVA: 0x001791F4 File Offset: 0x001773F4
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		ReadOnlyCollection<ClientInfo> list = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List;
		for (int i = 0; i < list.Count; i++)
		{
			ClientInfo clientInfo = list[i];
			if (clientInfo != _clientInfo)
			{
				if (!_clientInfo.PlatformId.Equals(clientInfo.PlatformId))
				{
					PlatformUserIdentifierAbs crossplatformId = _clientInfo.CrossplatformId;
					if (crossplatformId == null || !crossplatformId.Equals(clientInfo.CrossplatformId))
					{
						goto IL_0089;
					}
				}
				GameUtils.KickPlayerForClientInfo(clientInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.DuplicatePlayerID, 0, default(DateTime), ""));
				return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.DuplicatePlayerID, 0, default(DateTime), "")));
			}
			IL_0089:;
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
